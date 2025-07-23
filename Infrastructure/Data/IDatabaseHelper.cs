using System.Collections.Generic;
using System.Data;
using System.Data.Common;

namespace Terret_Billing.Infrastructure.Data
{
    public interface IDatabaseHelper
    {
        DbConnection CreateConnection();

        DbCommand CreateStoredProcedureCommand(string procedureName, DbConnection connection);

        List<string> GetConnectionString();
    }
} 