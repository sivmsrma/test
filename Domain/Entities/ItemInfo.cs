using System;

namespace Terret_Billing.Domain.Entities
{
    public class ItemInfo
    {
        public long item_id { get; set; }
        public string metal { get; set; }
        public string category { get; set; }
        public string sub_category { get; set; }
        public string design { get; set; }
        public long hsn_id { get; set; }
        public string hsn_code { get; set; }
        public string short_name { get; set; }
        public string firm_id { get; set; }
        public int IsDataPostOnServer { get; set; }
    }
} 