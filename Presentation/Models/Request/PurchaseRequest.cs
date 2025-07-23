using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Terret_Billing.Presentation.Models.Request
{
        public class PurchaseRequest
        {
            public Purchase Purchase { get; set; }
            public List<PurchaseItem> PurchaseItems { get; set; }
        }

   
}
