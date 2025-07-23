using System.Data;
using MySql.Data.MySqlClient;

namespace Terret_Billing.Application.Performance
{
    public static class ConnectionPoolHelper
    {
        public static IDbConnection CreateMySqlConnection(string connStr)
        {
            // Pooling enabled by default; can tune pool size via connStr
            return new MySqlConnection(connStr);
        }
    }
}
