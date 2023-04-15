using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DownloadInventoryFunc
{
    public class SellerResponse
    {
        public int code { get; set; }
        public string message { get; set; }
        public SellerData data { get; set; }
    }
    public class SellerData
    {
        public string sellerId { get; set; }
        public string name { get; set; }
        public string description { get; set; }
        public string displayName { get; set; }
        public string businessName { get; set; }
        public string city { get; set; }
        public int pincode { get; set; }
        public string state { get; set; }
        public int redirectState { get; set; }
        public string userId { get; set; }
        public object task { get; set; }
        public string newStatus { get; set; }
        public bool isOnBehalf { get; set; }
        public bool isRefurbSeller { get; set; }
        public string domain { get; set; }
        public string context { get; set; }
        public string refUrlForOnBehalf { get; set; }
        public bool staticHomepage { get; set; }
    }
}
