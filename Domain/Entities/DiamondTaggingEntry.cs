using System;

namespace Terret_Billing.Domain.Entities
{
    public class DiamondTaggingEntry
    {
        // Primary Key
        public long id { get; set; }

        // Properties matching the database schema exactly
        public string Firm_id { get; set; }
        public string user_name { get; set; }
        public string Stock_Type { get; set; }
        public string Particular { get; set; }
        public string Party_Name { get; set; }
        public string PartyName { get; set; }
        public string Invoice_No { get; set; }
        public string InvoiceNo { get; set; }
        public DateTime? Date { get; set; }
        public string Image { get; set; }
        public string Category { get; set; }
        public string SubCategory { get; set; }
        public string Design { get; set; }
        public string Purity { get; set; }
        public string HSN_No { get; set; }
        public string HsnNo { get; set; }
        public string HUID_No { get; set; }
        public string Size { get; set; }
        public int? pcs { get; set; }
        public int? Pieces { get; set; }
        public decimal? Gross_wt { get; set; }
        public int? Less_wt { get; set; }
        public decimal? Diamond_Ct { get; set; }
        public decimal? Diamond_wt_gm { get; set; }
        public decimal? Net_wt { get; set; }
        public decimal? Diamond_Rate { get; set; }
        public decimal? Diamond_Amt { get; set; }
        public decimal? Net_Rate { get; set; }
        public decimal? Net_Amt { get; set; }
        public decimal? Stone_Ct { get; set; }
        public decimal? Stone_Amt { get; set; }
        public string Description { get; set; }
        public string Drop_Down { get; set; }
        public decimal? Other_Charges { get; set; }
        public decimal? Value { get; set; }
        public decimal? Final_Amount { get; set; }
        public decimal? Gross_Amount { get; set; }
        public string Remark { get; set; }
        public string SrNo { get; set; }
        public string Barcode { get; set; }
        public int? Item_id { get; set; }
        public string Comment { get; set; }
        public bool? IsDataPostOnServer { get; set; }
        public long? Local_Id { get; set; }
        public string Metal_Type { get; internal set; }
        public long CreatedBy { get; set; }
        public long Stock_id { get; set; }
        public bool IsSelected { get; set; }




        public DiamondTaggingEntry()
        {
            // Default constructor
        }

        public static DiamondTaggingEntry Create(
            string firmId,
            string userName,
            string stockType,
            string particular,
            string partyName,
            string invoiceNo,
            DateTime? date = null,
            string image = null,
            string category = null,
            string subCategory = null,
            string design = null,
            string purity = null,
            string hsnNo = null,
            string huidNo = null,
            string size = null,
            int? pcs = null,
            decimal? grossWt = null,
            int? lessWt = null,
            decimal? diamondCt = null,
            decimal? diamondWtGm = null,
            decimal? netWt = null,
            decimal? diamondRate = null,
            decimal? diamondAmt = null,
            decimal? netRate = null,
            decimal? netAmt = null,
            decimal? stoneCt = null,
            decimal? stoneAmt = null,
            string description = null,
            string dropDown = null,
            decimal? otherCharges = null,
            decimal? value = null,
            decimal? finalAmount = null,
            decimal? grossAmount = null,
            string remark = null,
            string srNo = null,
            string barcode = null,
            int? itemId = null,
            string comment = null,
            bool? isDataPostOnServer = null,
            long? localId = null,
            long? createdby = null,
            long? stock_id = null)
        {
            return new DiamondTaggingEntry
            {
                id = 0,
                Firm_id = firmId,
                user_name = userName,
                Stock_Type = stockType,
                Particular = particular,
                Party_Name = partyName,
                Invoice_No = invoiceNo,
                Date = date,
                Image = image,
                Category = category,
                SubCategory = subCategory,
                Design = design,
                Purity = purity,
                HSN_No = hsnNo,
                HUID_No = huidNo,
                Size = size,
                pcs = pcs,
                Gross_wt = grossWt,
                Less_wt = lessWt,
                Diamond_Ct = diamondCt,
                Diamond_wt_gm = diamondWtGm,
                Net_wt = netWt,
                Diamond_Rate = diamondRate,
                Diamond_Amt = diamondAmt,
                Net_Rate = netRate,
                Net_Amt = netAmt,
                Stone_Ct = stoneCt,
                Stone_Amt = stoneAmt,
                Description = description,
                Drop_Down = dropDown,
                Other_Charges = otherCharges,
                Value = value,
                Final_Amount = finalAmount,
                Gross_Amount = grossAmount,
                Remark = remark,
                SrNo = srNo,
                Barcode = barcode,
                Item_id = itemId,
                Comment = comment,
                IsDataPostOnServer = isDataPostOnServer,
                Local_Id = localId,
                CreatedBy = (long)createdby,
                Stock_id = (long)stock_id
            };
        }

        public void Update(
            string firmId = null,
            string userName = null,
            string stockType = null,
            string particular = null,
            string partyName = null,
            string invoiceNo = null,
            DateTime? date = null,
            string image = null,
            string category = null,
            string subCategory = null,
            string design = null,
            string purity = null,
            string hsnNo = null,
            string huidNo = null,
            string size = null,
            int? pcs = null,
            decimal? grossWt = null,
            int? lessWt = null,
            decimal? diamondCt = null,
            decimal? diamondWtGm = null,
            decimal? netWt = null,
            decimal? diamondRate = null,
            decimal? diamondAmt = null,
            decimal? netRate = null,
            decimal? netAmt = null,
            decimal? stoneCt = null,
            decimal? stoneAmt = null,
            string description = null,
            string dropDown = null,
            decimal? otherCharges = null,
            decimal? value = null,
            decimal? finalAmount = null,
            decimal? grossAmount = null,
            string remark = null,
            string srNo = null,
            string barcode = null,
            int? itemId = null,
            string comment = null,
            bool? isDataPostOnServer = null,
            long? localId = null)
        {
            if (firmId != null) Firm_id = firmId;
            if (userName != null) user_name = userName;
            if (stockType != null) Stock_Type = stockType;
            if (particular != null) Particular = particular;
            if (partyName != null) Party_Name = partyName;
            if (invoiceNo != null) Invoice_No = invoiceNo;
            if (date.HasValue) Date = date;
            if (image != null) Image = image;
            if (category != null) Category = category;
            if (subCategory != null) SubCategory = subCategory;
            if (design != null) Design = design;
            if (purity != null) Purity = purity;
            if (hsnNo != null) HSN_No = hsnNo;
            if (huidNo != null) HUID_No = huidNo;
            if (size != null) Size = size;
            if (pcs.HasValue) this.pcs = pcs;
            if (grossWt.HasValue) Gross_wt = grossWt;
            if (lessWt.HasValue) Less_wt = lessWt;
            if (diamondCt.HasValue) Diamond_Ct = diamondCt;
            if (diamondWtGm.HasValue) Diamond_wt_gm = diamondWtGm;
            if (netWt.HasValue) Net_wt = netWt;
            if (diamondRate.HasValue) Diamond_Rate = diamondRate;
            if (diamondAmt.HasValue) Diamond_Amt = diamondAmt;
            if (netRate.HasValue) Net_Rate = netRate;
            if (netAmt.HasValue) Net_Amt = netAmt;
            if (stoneCt.HasValue) Stone_Ct = stoneCt;
            if (stoneAmt.HasValue) Stone_Amt = stoneAmt;
            if (description != null) Description = description;
            if (dropDown != null) Drop_Down = dropDown;
            if (otherCharges.HasValue) Other_Charges = otherCharges;
            if (value.HasValue) Value = value;
            if (finalAmount.HasValue) Final_Amount = finalAmount;
            if (grossAmount.HasValue) Gross_Amount = grossAmount;
            if (remark != null) Remark = remark;
            if (srNo != null) SrNo = srNo;
            if (barcode != null) Barcode = barcode;
            if (itemId.HasValue) Item_id = itemId;
            if (comment != null) Comment = comment;
            if (isDataPostOnServer.HasValue) IsDataPostOnServer = isDataPostOnServer;
            if (localId.HasValue) Local_Id = localId;
        }

    }
}