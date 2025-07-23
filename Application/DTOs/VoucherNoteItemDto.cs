using System;

namespace Terret_Billing.Application.DTOs
{
    public class VoucherNoteItemDto
    {
        public string MetalType { get; set; }
        public string Barcode { get; set; }
        public string Category { get; set; }
        public string SubCategory { get; set; }
        public string Design { get; set; }
        public string Purity { get; set; }
        public string HSN_No { get; set; }
        public string HUID_No { get; set; }
        public string Size { get; set; }
        public int pcs { get; set; }
        public decimal Gross_wt { get; set; }
        public decimal Less_wt { get; set; }
        public decimal Net_wt { get; set; }
        public decimal DiamondCt { get; set; }
        public decimal DiamondWtGm { get; set; }
        public decimal TunchPercent { get; set; }
        public decimal TunchWt { get; set; }
        public decimal WastePercent { get; set; }
        public decimal WasteFAmount { get; set; }
        public decimal StoneCt { get; set; }
        public decimal StoneWtGm { get; set; }
        public decimal FinalWt { get; set; }
        public string Comment { get; set; }
    }
} 