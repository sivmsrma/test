using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Terret_Billing.Presentation.Models.Request
{
    public class BillRequest
    {
            public Bill Bill { get; set; }
            public List<BillPayment> BillPayment { get; set; }
            public List<BillItem> BillItem { get; set; }
        
    }
}
