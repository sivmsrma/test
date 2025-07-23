using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Terret_Billing.Presentation.Models
{
    // Domain/Models/BillPayment.cs
    public class BillPayment
    {
        public long? Id { get; set; }
        public long BillId { get; set; }
        public decimal Amount { get; set; }
        public string PaymentMode { get; set; }
        public string ReferenceNo { get; set; }
        public DateTime PaymentDate { get; set; }
        public long? LocalId { get; set; }
        public int? ServerId { get; set; }
    }
}
