using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DownloadInventoryFunc
{
    public class StockFileResponse
    {

        public string file_link { get; set; }
        public string file_operation_type { get; set; }
        public string file_name { get; set; }
        public int? error_count { get; set; }
        public int? total_count { get; set; }
        public string file_format { get; set; }
        public long uploaded_on { get; set; }
        public string? feed_state { get; set; }
        public bool error_rows_exists { get; set; }
    }
    public class StockFileResponseList
    {
        public List<StockFileResponse> stock_file_response_list { get; set; }
    }
}
