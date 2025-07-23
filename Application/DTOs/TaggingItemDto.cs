using System;

namespace Terret_Billing.Application.DTOs
{
    public class TaggingItemDto
    {
        public string Barcode { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Item { get; set; } = string.Empty;
        public string Purity { get; set; } = string.Empty;
        public string HSN { get; set; } = string.Empty;
        public int PCS { get; set; }
        public decimal GrossWt { get; set; }
        public decimal LessWt { get; set; }
        public decimal NetWt { get; set; }
        public decimal DiamondCt { get; set; }
        public decimal DiamondRate { get; set; }
        public decimal DiaCharge { get; set; }
        public decimal StoneCt { get; set; }
        public decimal FAmount { get; set; }
        public decimal FinalWeight { get; set; }
        public decimal Amount { get; set; }
        public decimal TaxAmount { get; set; }
        public decimal MakingCharges { get; set; }
        public decimal HMCharges { get; set; }
        public string MetalType { get; set; } = string.Empty;
        public string StockType { get; set; } = string.Empty;
        public decimal NetPrice { get; set; }
        public decimal BillRoundOff { get; set; }
        public int Id { get; set; }
        public string HUID { get; set; }
        public string Hallmark { get; set; }
        public decimal HmTax { get; set; }
        public decimal Quantity { get; set; }
        public decimal StoneCharge { get; set; }
        public decimal Rate { get; set; }
        public decimal TaxRate { get; set; }
         public decimal MakingCharge { get; set; }
        public decimal FinalAmount { get; set; }
    }
}
