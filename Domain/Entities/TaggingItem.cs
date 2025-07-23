using System;

namespace Terret_Billing.Domain.Entities
{
    public class TaggingItem
    {
        // Database fields
        public long stock_id { get; set; }
        public string stock_type { get; set; }
        public string metal_type { get; set; }
        public DateTime? entry_date { get; set; }
        public string party_name { get; set; }
        public string invoice_number { get; set; }
        public string purity { get; set; }
        public decimal? total_weight { get; set; }
        public decimal? total_carat { get; set; }
        public decimal? completed_weight { get; set; }
        public decimal? completed_carat { get; set; }
        public decimal? pending_weight { get; set; }
        public decimal? pending_carat { get; set; }
        public string firmid { get; set; }
        public bool? IsDataPostOnServer { get; set; }
        public int? PurchaseItemId { get; set; }

        // Additional properties for DTO mapping
        public string Barcode { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string ItemName { get; set; } = string.Empty;
        public string HSNCode { get; set; } = string.Empty;
        public int Quantity { get; set; }
        public decimal GrossWeight { get; set; }
        public decimal NetWeight { get; set; }
        public decimal DiamondCarat { get; set; }
        public decimal DiamondRate { get; set; }
        public decimal StoneCount { get; set; }
        public decimal FinalWeight { get; set; }
        public string MetalType { get; set; } = string.Empty;
        public bool IsPendingWeightZero
        {
            get
            {
                
                return pending_weight == 0.000m;
            }
        }



        public static TaggingItem Create(
            string stock_type,
            string metal_type,
            DateTime? entry_date,
            string party_name,
            string invoice_number,
            string purity,
            decimal? total_weight,
            decimal? total_carat,
            decimal? completed_weight = null,
            decimal? completed_carat = null,
            decimal? pending_weight = null,
            decimal? pending_carat = null,
            string firmid = null,
            bool? IsDataPostOnServer = null,
            int? PurchaseItemId = null)
        {
            if (string.IsNullOrEmpty(firmid))
                throw new ArgumentException("firmid is required", nameof(firmid));

            return new TaggingItem
            {
                stock_type = stock_type,
                metal_type = metal_type,
                entry_date = entry_date,
                party_name = party_name,
                invoice_number = invoice_number,
                purity = purity,
                total_weight = total_weight,
                total_carat = total_carat,
                completed_weight = completed_weight,
                completed_carat = completed_carat,
                pending_weight = pending_weight,
                pending_carat = pending_carat,
                firmid = firmid,
                IsDataPostOnServer = IsDataPostOnServer,
                PurchaseItemId = PurchaseItemId
            };
        }



        public void Update(
            string stock_type = null,
            string metal_type = null,
            DateTime? entry_date = null,
            string party_name = null,
            string invoice_number = null,
            string purity = null,
            decimal? total_weight = null,
            decimal? total_carat = null,
            decimal? completed_weight = null,
            decimal? completed_carat = null,
            decimal? pending_weight = null,
            decimal? pending_carat = null,
            string firmid = null,
            bool? IsDataPostOnServer = null,
            int? PurchaseItemId = null)
        {
            if (stock_type != null) this.stock_type = stock_type;
            if (metal_type != null) this.metal_type = metal_type;
            if (entry_date.HasValue) this.entry_date = entry_date.Value;
            if (party_name != null) this.party_name = party_name;
            if (invoice_number != null) this.invoice_number = invoice_number;
            if (purity != null) this.purity = purity;
            if (total_weight.HasValue) this.total_weight = total_weight.Value;
            if (total_carat.HasValue) this.total_carat = total_carat.Value;
            if (completed_weight.HasValue) this.completed_weight = completed_weight.Value;
            if (completed_carat.HasValue) this.completed_carat = completed_carat.Value;
            if (pending_weight.HasValue) this.pending_weight = pending_weight.Value;
            if (pending_carat.HasValue) this.pending_carat = pending_carat.Value;
            if (firmid != null) this.firmid = firmid;
            if (IsDataPostOnServer.HasValue) this.IsDataPostOnServer = IsDataPostOnServer.Value;
            if (PurchaseItemId.HasValue) this.PurchaseItemId = PurchaseItemId.Value;
        }
    }
}
