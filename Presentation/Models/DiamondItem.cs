using System;

namespace Terret_Billing.Presentation.Models
{
    public class DiamondItem
    {
        public string Barcode { get; set; }
        public string Category { get; set; }
        public string SubCategory { get; set; }
        public string Design { get; set; }
        public string HsnCode { get; set; }
        public string Size { get; set; }
        public int Pcs { get; set; }
        public decimal Weight { get; set; }
        public decimal Carat { get; set; }
        public string Clarity { get; set; }
        public string Color { get; set; }
        public string Cut { get; set; }
        public string PartyName { get; set; }
        public string InvoiceNo { get; set; }
        public string Comment { get; set; }
        public DateTime Date { get; set; }
    }
}
