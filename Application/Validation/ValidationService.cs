using System;

namespace Terret_Billing.Application.Validation
{
    public class ValidationService
    {
        public bool ValidateString(string value)
        {
            return !string.IsNullOrWhiteSpace(value);
        }
        // Add more validation methods as needed
    }
}
