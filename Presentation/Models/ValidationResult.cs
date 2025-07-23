using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Terret_Billing.Presentation.Models
{
    public class ValidationResult
    {
        public bool IsValid { get; set; } = true;
        public string ErrorField { get; set; }
        public string ErrorMessage { get; set; }
    }

}
