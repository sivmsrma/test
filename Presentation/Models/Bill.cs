using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Terret_Billing.Presentation.Models
{
    public class Bill
    {
        public long Id { get; set; }
        public string BillNo { get; set; }
        public DateTime BillDate { get; set; }
        public int PartyId { get; set; }
        public decimal TotalAmount { get; set; }
        public decimal TaxAmount { get; set; }
        public decimal DiscountAmount { get; set; }
        public decimal NetAmount { get; set; }
        public string FirmId { get; set; }
        public bool IsSynced { get; set; }
        public long LocalId { get; set; }
        public int? ServerId { get; set; }
        public DateTime? SyncDate { get; set; }
    }
}
