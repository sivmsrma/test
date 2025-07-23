using System;

namespace Terret_Billing.Domain.Entities
{
    public class Item
    {
        public long ItemId { get; set; }  
        public string MetalType { get; set; }
        public string Category { get; set; }
        public string SubCategory { get; set; }
        public string Design { get; set; }
        public long HsnId { get; set; }  
        public string HsnCode { get; set; }
        public string ShortName { get; set; }  
        public string Barcode { get; set; }
        public string FirmId { get; set; }  
       
    }

}