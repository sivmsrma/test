using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Terret_Billing.Domain.Entities
{
    internal class HSNListItem
    {
        public int hsn_id { get; set; }
        public string firm_id { get; set; }
        public string metal { get; set; }
        public string hsn_code { get; set; }
        public int IsDataPostOnServer { get; set; }
        public int SerialNumber { get; set; }
    }
}
