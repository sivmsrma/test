using BCrypt.Net;

namespace Terret_Billing.Application.Security
{
    public static class PasswordHasher
    {
        public static string Hash(string password) => BCrypt.Net.BCrypt.HashPassword(password);
        public static bool Verify(string password, string hash) => BCrypt.Net.BCrypt.Verify(password, hash);
    }
}
