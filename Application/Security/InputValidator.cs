using System.Text.RegularExpressions;

namespace Terret_Billing.Application.Security
{
    public static class InputValidator
    {
        public static bool IsSafeString(string input)
        {
            // Basic injection prevention
            return !Regex.IsMatch(input, @"[;'""\\-\-]");
        }
        public static bool IsValidEmail(string email)
        {
            return Regex.IsMatch(email, @"^[^@\s]+@[^@\s]+\.[^@\s]+$");
        }
    }
}
