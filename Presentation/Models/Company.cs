using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Terret_Billing.Presentation.Models
{
    public class CompanyInfo
    {
        public string Name { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
        public string Mobile { get; set; }
        public string GSTIN { get; set; }
        // aur bhi fields jo aapke DB me hain
    }

    public class VoucherCompanyInfo
    {
        public string ShopName { get; set; }
        public string State { get; set; }
        public string Address { get; set; }
        public string GSTIN { get; set; }
    }
}
