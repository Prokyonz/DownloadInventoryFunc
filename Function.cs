using Amazon;
using Amazon.Lambda.Core;
using Amazon.S3;
using Amazon.S3.Transfer;
using Newtonsoft.Json;
using Npgsql;
using NPOI.HSSF.UserModel;
using NPOI.POIFS.Crypt;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using RestSharp;
using System.Data;
using System.Net;
using System.Text.RegularExpressions;

// Assembly attribute to enable the Lambda function's JSON input to be converted into a .NET class.
[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.SystemTextJson.DefaultLambdaJsonSerializer))]

namespace DownloadInventoryFunc;

public class Function
{
    const string connectionString = "Server=postgresql-113549-0.cloudclusters.net;Port=19060;Database=flipkartpay;User Id=flipkartpay;Password=Mayur@8692;";
    /// <summary>
    /// A simple function that takes a string and does a ToUpper
    /// </summary>
    /// <param name="input"></param>
    /// <param name="context"></param>
    /// <returns></returns>
    public string FunctionHandler(string input, ILambdaContext context)
    {
        var cookiesList = GetCookieMaster();
        cookiesList.ForEach(cookiemst =>
        {
            try
            {
                var response = GetCSRFToken(cookiemst, null);
                //if cookie is expired
                if (response == null)
                {
                    //login to flipkart
                    var loginResponse = Login(cookiemst.username, cookiemst.password_encrypt);
                    if (loginResponse.ResponseStatus == ResponseStatus.Completed)
                    {
                        SellerResponse responseSeller = JsonConvert.DeserializeObject<SellerResponse>(loginResponse.Content.ToString());

                        if (responseSeller.code == 1000)
                        {
                            string strCookiesData = string.Empty;
                            foreach (string eCookie in cookiemst.cookiestring.Split(';'))
                            {
                                if (eCookie.Contains("="))
                                {
                                    string[] split = eCookie.Split('=');
                                    strCookiesData = strCookiesData + split[0].ToString().Trim() + "=" + split[1].ToString().Trim() + "; ";
                                }
                            }
                            UpdateCookieString(strCookiesData, cookiemst.cookiesmasterid);
                        }
                    }

                }
                var inventoryResponse = DownloadInventory(cookiemst);
                string json = inventoryResponse.Content.ToString();
                StockFileResponseList stockFileResponseList = JsonConvert.DeserializeObject<StockFileResponseList>(json);
                if (stockFileResponseList != null)
                {
                    var responselist = stockFileResponseList.stock_file_response_list.OrderByDescending(x => x.uploaded_on).FirstOrDefault();
                    if (responselist != null)
                    {
                        string filePath = DownloadStockFile(cookiemst, responselist);

                        UploadFileOnAWSS3(filePath);

                        DataTable dt = ExcelToDataTable(filePath, cookiemst.licensekey);

                        InsertSKULicenseMaster(dt, "SKUListingMaster");
                    }
                }
            }
            catch
            { }
            finally
            {
                // Define the values for the new entry
                DateTime syncdate = DateTime.Now;
                string sellerid = cookiemst.sellerid;
                string licenseid = cookiemst.licensekey;
                string description = "my description";
                bool isexecutedfunc1 = true;
                ExecutionStatus func1status = ExecutionStatus.Execute;
                string func1error = string.Empty;
                FunctionName function = FunctionName.Function1;
                string filepath = @"D:\123.xls";

                // Call the function to insert the new entry into the database
                InsertAutoSyncHistoryEntry(syncdate, sellerid, licenseid, description, isexecutedfunc1, func1status, func1error, FunctionName.Function2, filepath);
            }
        });
        return input.ToUpper();
    }

    #region Database Operations
    private List<CookieMaster> GetCookieMaster()
    {
        NpgsqlConnection conn = new NpgsqlConnection(connectionString);
        conn.Open();

        // Start a transaction as it is required to work with cursors in PostgreSQL
        NpgsqlTransaction tran = conn.BeginTransaction();

        // Define a command to call stored procedure show_cities_multiple
        NpgsqlCommand command = new NpgsqlCommand("SELECT * FROM public.tblcookiemaster", conn);
        command.CommandType = CommandType.Text;

        // Execute the stored procedure and obtain the first result set
        NpgsqlDataReader dr = command.ExecuteReader();

        List<CookieMaster> cookieList = new List<CookieMaster>();
        if (dr.Read())
        {
            cookieList.Add(new CookieMaster()
            {
                cookiesmasterid = Convert.ToInt32(dr[0]),
                licensekey = dr[1].ToString(),
                sellerid = dr[2].ToString(),
                cookiestring = dr[3].ToString()
            });
            dr.NextResult();
        }
        dr.Close();
        tran.Commit();
        conn.Close();

        return cookieList;
    }

    private bool UpdateCookieString(string cookieString, int cookieID)
    {
        NpgsqlConnection conn = new NpgsqlConnection(connectionString);
        conn.Open();

        NpgsqlTransaction tran = conn.BeginTransaction();


        NpgsqlCommand command = new NpgsqlCommand($"UPDATE public.tblcookiemaster SET cookiestring = '{cookieString}' WHERE cookiesmasterid = {cookieID}", conn);
        command.CommandType = CommandType.Text;

        // Execute the stored procedure 
        command.ExecuteNonQuery();

        tran.Commit();
        conn.Close();

        return true;
    }

    private void InsertSKULicenseMaster(DataTable dataTable, string tableName)
    {
        string pattern = "[^a-zA-Z0-9]+";
        string replacement = "";
        // Insert the DataTable into the PostgreSQL table
        using (NpgsqlConnection conn = new NpgsqlConnection(connectionString))
        {
            conn.Open();

            // Create the new table in the PostgreSQL database
            var columns = string.Join(",", dataTable.Columns.Cast<DataColumn>().Select(c => $"{Regex.Replace(c.ColumnName, pattern, replacement)} TEXT"));
            string createTableSql = $"CREATE TABLE IF NOT EXISTS {tableName} ({columns})";

            using (NpgsqlCommand cmd = new NpgsqlCommand(createTableSql, conn))
            {
                cmd.ExecuteNonQuery();
            }

            var columnsList = string.Join(",", dataTable.Columns.Cast<DataColumn>().Select(c => $"{Regex.Replace(c.ColumnName, pattern, replacement)}"));
            foreach (DataRow row in dataTable.Rows)
            {
                string Sql = $"INSERT INTO {tableName} ({columnsList})";
                Sql += "VALUES(";
                for (int i = 0; i < dataTable.Columns.Count; i++)
                {
                    Sql += "'" + row.ItemArray[i].ToString().Replace("'", "") + "',";
                }
                Sql = Sql.Remove(Sql.Length - 1);
                Sql += ")";

                using (NpgsqlCommand cmd = new NpgsqlCommand(Sql, conn))
                {
                    cmd.ExecuteNonQuery();
                }
            }
        }
    }

    public void InsertAutoSyncHistoryEntry(DateTime syncdate, string sellerid, string licenseid, string description, bool isexecutedfuncsync, ExecutionStatus funcsyncstatus, string funcsyncerror, FunctionName funcName, string filepath)
    {
        // Open a new database connection
        using (var conn = new NpgsqlConnection(connectionString))
        {
            conn.Open();

            // Define the SQL command to insert a new entry into the "AutoSyncHistory" table
            using (var cmd = new NpgsqlCommand())
            {
                cmd.Connection = conn;
                cmd.CommandText = @"INSERT INTO public.""AutoSyncHistory"" (syncdate, sellerid, licenseid, description, isexecutedfuncsync, funcsyncstatus, funcsyncerror, funcsyncexecutiondate,funcname, filepath) ";
                cmd.CommandText += " VALUES (@syncdate, @sellerid, @licenseid, @description, @isexecutedfuncsync, @funcsyncstatus, @funcsyncerror, @funcsyncexecutiondate,  @funcname, @filepath)";

                // Add parameters to the SQL command to avoid SQL injection attacks
                cmd.Parameters.AddWithValue("syncdate", syncdate);
                cmd.Parameters.AddWithValue("sellerid", sellerid);
                cmd.Parameters.AddWithValue("licenseid", licenseid);
                cmd.Parameters.AddWithValue("description", description);
                cmd.Parameters.AddWithValue("isexecutedfuncsync", isexecutedfuncsync);
                cmd.Parameters.AddWithValue("funcsyncstatus", funcsyncstatus);
                cmd.Parameters.AddWithValue("funcsyncerror", funcsyncerror);
                cmd.Parameters.AddWithValue("funcname", funcName);
                cmd.Parameters.AddWithValue("filepath", filepath);
                cmd.Parameters.AddWithValue("funcsyncexecutiondate", DateTime.Now);

                // Execute the SQL command
                cmd.ExecuteNonQuery();
            }
        }
    }

    #endregion

    #region API 
    public IRestResponse DownloadInventory(CookieMaster cookiesMst)
    {
        var client = new RestClient("https://seller.flipkart.com/napi/listing/stockFileDownloadNUploadHistory");
        client.Timeout = -1;
        var request = new RestRequest(Method.GET);
        request.AddHeader("Accept", "/");
        request.AddHeader("Accept-Language", "en-GB,en-US;q=0.9,en-IN;q=0.8,en;q=0.7,hi;q=0.6");
        request.AddHeader("Connection", "keep-alive");
        request.AddHeader("Referer", "https://seller.flipkart.com/index.html");
        request.AddHeader("Sec-Fetch-Dest", "empty");
        request.AddHeader("Sec-Fetch-Mode", "cors");
        request.AddHeader("Sec-Fetch-Site", "same-origin");
        client.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/111.0.0.0 Safari/537.36";
        request.AddHeader("sec-ch-ua", "\"Google Chrome\";v=\"111\", \"Not(A:Brand\";v=\"8\", \"Chromium\";v=\"111\"");
        request.AddHeader("sec-ch-ua-mobile", "?0");
        request.AddHeader("sec-ch-ua-platform", "\"Windows\"");

        foreach (string Acookie in cookiesMst.cookiestring.Split(';'))
        {
            if (Acookie.Contains("="))
            {
                string[] split = Acookie.Split('=');
                request.AddCookie(split[0].ToString().Trim(), split[1].ToString().Trim());
            }
        }
        return client.Execute(request);

    }

    private string DownloadStockFile(CookieMaster cookiemst, StockFileResponse? responselist)
    {
        string BaseURL = "https://seller.flipkart.com/napi/listing/stockFileDownload?sellerId=" + cookiemst.sellerid + "&fileId=";
        string GetFromHistory = responselist.file_link;
        GetFromHistory = GetFromHistory.Replace("/", "%2F");
        GetFromHistory = GetFromHistory.Replace("?", "%3F");
        GetFromHistory = GetFromHistory.Replace("=", "%3D");
        GetFromHistory = GetFromHistory.Replace("&", "%26");
        string FileName = "&fileName=123.xls";
        string strUrl = BaseURL + GetFromHistory + FileName;

        IRestResponse Test = DownloadStockFile(cookiemst, strUrl);
        byte[] x = Test.RawBytes;

        string filePath = @"D:\123.xls";
        System.IO.File.WriteAllBytes(filePath, x);
        return filePath;
    }

    public IRestResponse DownloadStockFile(CookieMaster cookiesMst, string URL)
    {
        var client = new RestClient(URL);
        client.Timeout = -1;
        var request = new RestRequest(Method.GET);
        request.AddHeader("Accept", "*/*");
        request.AddHeader("Accept-Language", "en-GB,en-US;q=0.9,en-IN;q=0.8,en;q=0.7");
        request.AddHeader("Connection", "keep-alive");
        foreach (string Acookie in cookiesMst.cookiestring.Split(';'))
        {
            if (Acookie.Contains("="))
            {
                string[] split = Acookie.Split('=');
                request.AddCookie(split[0].ToString().Trim(), split[1].ToString().Trim());
            }
        }


        request.AddHeader("Referer", "https://seller.flipkart.com/index.html");
        request.AddHeader("Sec-Fetch-Dest", "empty");
        request.AddHeader("Sec-Fetch-Mode", "cors");
        request.AddHeader("Sec-Fetch-Site", "same-origin");
        client.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/111.0.0.0 Safari/537.36";
        request.AddHeader("newrelic", "eyJ2IjpbMCwxXSwiZCI6eyJ0eSI6IkJyb3dzZXIiLCJhYyI6IjMzNzAxMDEiLCJhcCI6IjUyNTkwNTE3OCIsImlkIjoiYTdiZTJkODE0Mzk3NjQ4OSIsInRyIjoiOGQ2ZDc3ODBhODVkMmVhOWYwZGVlM2IyZTAxMzI5YzAiLCJ0aSI6MTY4MTExOTMxNjY0OSwidGsiOiIzMjkzNzk0In19");
        request.AddHeader("sec-ch-ua", "\"Google Chrome\";v=\"111\", \"Not(A:Brand\";v=\"8\", \"Chromium\";v=\"111\"");
        request.AddHeader("sec-ch-ua-mobile", "?0");
        request.AddHeader("sec-ch-ua-platform", "\"Windows\"");
        request.AddHeader("traceparent", "00-8d6d7780a85d2ea9f0dee3b2e01329c0-a7be2d8143976489-01");
        request.AddHeader("tracestate", "3293794@nr=0-1-3370101-525905178-a7be2d8143976489----1681119316649");
        IRestResponse response = client.Execute(request);
        //Console.WriteLine(response.Content);
        return response;
    }

    public IRestResponse Login(string Username, string Password)
    {
        ServicePointManager.SecurityProtocol = (SecurityProtocolType)768 | (SecurityProtocolType)3072; // 768= TLS1.1 and 3072=TLS1.2 // JHL++ 14112018
        ServicePointManager.Expect100Continue = false; // KRD++ 14032018
        IRestResponse restResponse;
        try
        {
            //ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;
            RestClient client = new RestClient("https://seller.flipkart.com/login")
            {
                Timeout = -1
            };
            RestRequest request = new RestRequest(Method.POST);
            request.AddHeader("Connection", "keep-alive");
            request.AddHeader("sec-ch-ua", "\"Google Chrome\";v=\"89\", \"Chromium\";v=\"89\", \";Not A Brand\";v=\"99\"");
            request.AddHeader("Accept", "application/json, text/javascript, */*; q=0.01");
            request.AddHeader("X-Requested-With", "XMLHttpRequest");
            request.AddHeader("sec-ch-ua-mobile", "?0");
            client.UserAgent = "Mozilla/5.0 (Macintosh; Intel Mac OS X 11_2_3) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/89.0.4389.90 Safari/537.36";
            request.AddHeader("Content-Type", "application/json");
            request.AddHeader("Origin", "https://seller.flipkart.com");
            request.AddHeader("Sec-Fetch-Site", "same-origin");
            request.AddHeader("Sec-Fetch-Mode", "cors");
            request.AddHeader("Sec-Fetch-Dest", "empty");
            request.AddHeader("Referer", "https://seller.flipkart.com/sell-online/");
            request.AddHeader("Accept-Language", "en-GB,en-US;q=0.9,en;q=0.8");
            string[] username = new string[] { "{\"authName\":\"flipkart\",\"username\":\"", Username, "\",\"password\":\"", Password, "\",\"userNameType\":\"displayName\"}" };
            string strLoginCredentials = string.Concat(username);
            request.AddParameter("application/json", strLoginCredentials, ParameterType.RequestBody);
            restResponse = client.Execute(request);
        }
        catch (Exception exception)
        {
            Exception ex = exception;
            //ErrorLogServices.InsertErrorLog("Login", ex.Message, "UtilityServices", ex.StackTrace);
            restResponse = null;
        }
        return restResponse;
    }

    public IRestResponse GetCSRFToken(CookieMaster cookiesMst, IRestResponse LoginResponse)
    {
        ServicePointManager.SecurityProtocol = (SecurityProtocolType)768 | (SecurityProtocolType)3072; // 768= TLS1.1 and 3072=TLS1.2 // JHL++ 14112018
        ServicePointManager.Expect100Continue = false; // KRD++ 14032018
        IRestResponse restResponse;
        try
        {
            if (string.IsNullOrEmpty(cookiesMst.sellerid))
            {
                throw new Exception("Seller ID is not set. Seller ID is necessary to call the API.");
            }

            IRestClient client = new RestClient("https://seller.flipkart.com/getFeaturesForSeller")
            {
                Timeout = -1
            };

            var request = new RestRequest();
            request.AddHeader("Connection", "keep-alive");
            request.AddHeader("sec-ch-ua", "\"Google Chrome\";v=\"89\", \"Chromium\";v=\"89\", \";Not A Brand\";v=\"99\"");
            request.AddHeader("Accept", "application/json, text/javascript, */*; q=0.01");
            request.AddHeader("X-Requested-With", "XMLHttpRequest");
            request.AddHeader("sec-ch-ua-mobile", "?0");
            client.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/89.0.4389.114 Safari/537.36";
            request.AddHeader("Sec-Fetch-Site", "same-origin");
            request.AddHeader("Sec-Fetch-Mode", "cors");
            request.AddHeader("Sec-Fetch-Dest", "empty");
            request.AddHeader("Referer", "https://seller.flipkart.com/index.html");
            request.AddHeader("Accept-Language", "en-US,en;q=0.9");

            foreach (string Acookie in cookiesMst.cookiestring.Split(';'))
            {
                if (Acookie.Contains("="))
                {
                    string[] split = Acookie.Split('=');
                    request.AddCookie(split[0].ToString().Trim(), split[1].ToString().Trim());
                }
            }
            request.AddParameter("sellerId", cookiesMst.sellerid);
            IRestResponse response = client.Execute(request);

            if (response == null)
            {
                throw new Exception("API returned null response.");
            }
            restResponse = response;
        }
        catch (Exception exception)
        {

            Exception ex = exception;
            //ErrorLogServices.InsertErrorLog("GetCSRFToken", ex.Message, "APIs", ex.StackTrace);
            restResponse = null;
        }
        return restResponse;
    }
    #endregion

    private void UploadFileOnAWSS3(string filePath)
    {
        var s3Client = new AmazonS3Client(accessKey, secretKey, RegionEndpoint.USWest2);

        // Set the bucket name and key for the new file
        string bucketName = "my-bucket";
        string keyName = "my-folder/my-file.xls";

        // Create a TransferUtility instance to upload the file to S3
        var transferUtility = new TransferUtility(s3Client);

        // Upload the XLS file to S3
        transferUtility.Upload(filePath, bucketName, keyName);
    }

    #region Common Method
    public DataTable ExcelToDataTable(string filePath, string licenseKey)
    {
        // Load the Excel file into a DataTable
        DataTable dataTable = new DataTable();
        using (var fs = new FileStream(filePath, FileMode.Open, FileAccess.Read))
        {
            IWorkbook workbook;
            if (Path.GetExtension(filePath) == ".xls")
                workbook = new HSSFWorkbook(fs);
            else
                workbook = new XSSFWorkbook(fs);

            var sheet = workbook.GetSheetAt(0);
            var headerRow = sheet.GetRow(0);
            foreach (var cell in headerRow.Cells)
            {
                dataTable.Columns.Add(cell.ToString());
            }
            dataTable.Columns.Add("LicenseKey", typeof(string));
            dataTable.Columns.Add("Price", typeof(string));

            for (int i = (sheet.FirstRowNum + 1); i <= sheet.LastRowNum; i++)
            {
                var row = sheet.GetRow(i);
                if (row == null) continue;
                var dataRow = dataTable.NewRow();
                for (int j = row.FirstCellNum; j < row.Cells.Count; j++)
                {
                    dataRow[j] = row.GetCell(j).ToString();
                }
                dataRow[row.Cells.Count] = licenseKey;
                dataRow[row.Cells.Count + 1] = 0;
                dataTable.Rows.Add(dataRow);

            }
        }
        return dataTable;
    }

    #endregion
}
