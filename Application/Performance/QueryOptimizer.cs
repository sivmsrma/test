using System.Data;
using System.Threading.Tasks;
using Dapper;

namespace Terret_Billing.Application.Performance
{
    public static class QueryOptimizer
    {
        public static async Task<T> QueryFirstOrDefaultOptimizedAsync<T>(IDbConnection conn, string sql, object param = null)
        {
            // Use optimal Dapper settings, command timeout, buffered, etc.
            return await conn.QueryFirstOrDefaultAsync<T>(sql, param, commandTimeout: 30);
        }
    }
}
