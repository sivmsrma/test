using System;

namespace Terret_Billing.Domain.Entities
{
    public class GoldAndSilverTaggingEntry
    {
        public long Id { get; set; }
        public string Firm_Id { get; set; }
        public string User_Name { get; set; }
        public string Stock_Type { get; set; }
        public string Particular { get; set; }
        public string Party_Name { get; set; }
        public string Invoice_No { get; set; }
        public DateTime Entry_Date { get; set; }
        public string Image { get; set; }
        public string Category { get; set; }
        public string SubCategory { get; set; }
        public string Design { get; set; }
        public string Purity { get; set; }
        public string HSN_No { get; set; }
        public string SizeVal { get; set; }
        public string Size { get; set; }
        public int Pcs { get; set; }
        public decimal Gross_Wt { get; set; }
        public decimal Less_Wt { get; set; }
        public decimal Net_Wt { get; set; }
        public decimal Net_Rate { get; set; }
        public decimal Net_Amount { get; set; }
        public decimal Other_Charges { get; set; }
        public decimal Final_Amount { get; set; }
        public string Drop_Down { get; set; }
        public string Remark { get; set; }
        public string SrNo { get; set; }
        public string Barcode { get; set; }
        public int Item_Id { get; set; }
        public string CommentVal { get; set; }
        public long? Local_Id { get; set; }
        public bool? IsDataPostOnServer { get; set; }
        public string Metal_type { get; set; }
        public long StockId { get; set; }
        public long CreatedBy { get; set; }
        public bool IsSelected { get; set; }





    }



}
