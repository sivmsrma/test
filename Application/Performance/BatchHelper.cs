using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using Dapper;

namespace Terret_Billing.Application.Performance
{
    public static class BatchHelper
    {
        public static async Task<int> BulkInsertAsync<T>(IDbConnection conn, IEnumerable<T> items, string sql)
        {
            return await conn.ExecuteAsync(sql, items);
        }
    }
}
