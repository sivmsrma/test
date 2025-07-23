using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZXing.PDF417.Internal;

namespace Terret_Billing.Domain.Entities
{
    public class PurchaseViewReport
    {
        public string BillNo { get; set; }

        public string PartyName { get; set; }

        public string Metal { get; set; }

        public string ItemName { get; set; }

        public string Purity { get; set; }

        public int Pcs { get; set; }

        public decimal GrossWt { get; set; }

        public decimal DiaCt { get; set; }

        public decimal StoneCt { get; set; }

        public decimal NetWt { get; set; }

        public decimal Rate { get; set; }

        public decimal Amount { get; set; }

        public decimal StoneAmt { get; set; }

        public decimal DiaAmt { get; set; }

        public decimal NetPrice { get; set; }

        public string TaxType { get; set; }

        public decimal tax { get; set; }

        public decimal TotalAmt { get; set; }

        public decimal TotalPaidAmt { get; set; }


        public DateTime updated_time { get; set; } = DateTime.Now;

        public string? firm_id { get; set; }


    }
}
