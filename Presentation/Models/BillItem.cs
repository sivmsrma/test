using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Terret_Billing.Presentation.Models
{
    public class BillItem
    {
        public long? Id { get; set; }
        public long BillId { get; set; }
        public int ItemId { get; set; }

        // Item Details
        public string Barcode { get; set; }
        public string Description { get; set; }
        public string Purity { get; set; }
        public string HSNCode { get; set; }
        public int PCS { get; set; }

        // Weight Details
        public decimal GrossWt { get; set; }
        public decimal LessWt { get; set; }
        public decimal NetWt { get; set; }

        // Diamond Details
        public decimal DiamondCt { get; set; }
        public decimal DiamondRate { get; set; }
        public decimal DiaCharge { get; set; }

        // Stone Details
        public decimal StoneCt { get; set; }
        public decimal StoneCharge { get; set; }

        // Metal Details
        public decimal FinalWeight { get; set; }
        public string MetalType { get; set; }
        public string HUID { get; set; }
        public string Hallmark { get; set; }
        public decimal HmTax { get; set; }

        // Pricing
        public decimal Quantity { get; set; }
        public decimal Rate { get; set; }
        public decimal Amount { get; set; }
        public decimal TaxRate { get; set; }
        public decimal TaxAmount { get; set; }
        public decimal MakingCharge { get; set; }
        public decimal NetPrice { get; set; }
        public decimal FinalAmount { get; set; }

        // Sync Info
        public long? LocalId { get; set; }
        public int? ServerId { get; set; }
        public DateTime CreatedAt { get; set; }

        // Navigation property
        public virtual Bill Bill { get; set; }
    }
}
