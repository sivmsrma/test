using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Terret_Billing.Domain.Entities
{
    public class VoucherNoteItems
    {
        // party details 
        public int Id { get; private set; }
        public string Name { get; private set; }
        public string MobileNumber { get; private set; }
        public string Gender { get; private set; }
        public string Email { get; private set; }
        public string GSTNumber { get; private set; }
        public string PANNumber { get; private set; }
        public string Address { get; private set; }
        public string State { get; private set; }
        public string StateCode { get; private set; }
        public string District { get; private set; }
        public string City { get; private set; }
        public string Village { get; private set; }
        public string ColonyTower { get; private set; }
        public string PinCode { get; private set; }
        public string AccountNumber { get; private set; }
        public string AccountType { get; private set; }
        public string Ifsc { get; private set; }
        public string BankName { get; private set; }
        public string BankBranch { get; private set; }
        public string Narration { get; private set; }
        public DateTime? CreatedAt { get; private set; }
        public DateTime? UpdatedAt { get; private set; }
        public bool IsActive { get; private set; }

        // gold and silver tagging items 

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

        //diamond tagging entry 

        public long id { get; set; }

        // Properties matching the database schema exactly
        public string Firm_id { get; set; }
        public string user_name { get; set; }
        public string PartyName { get; set; }
        public string InvoiceNo { get; set; }
        public DateTime? Date { get; set; }
        public string HsnNo { get; set; }
        public string HUID_No { get; set; }
        public int? pcs { get; set; }
        public int? Pieces { get; set; }
        public decimal? Gross_wt { get; set; }
        public int? Less_wt { get; set; }
        public decimal? Diamond_Ct { get; set; }
        public decimal? Diamond_wt_gm { get; set; }
        public decimal? Net_wt { get; set; }
        public decimal? Diamond_Rate { get; set; }
        public decimal? Diamond_Amt { get; set; }
        public decimal? Net_Amt { get; set; }
        public decimal? Stone_Ct { get; set; }
        public decimal? Stone_Amt { get; set; }
        public string Description { get; set; }
        public decimal? Value { get; set; }
        public decimal? Gross_Amount { get; set; }
        public int? Item_id { get; set; }
        public string Comment { get; set; }
        public string Metal_Type { get; internal set; }
        public long Stock_id { get; set; }

    }
}
