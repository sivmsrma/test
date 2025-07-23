using System;
using System.Data;

namespace Terret_Billing.Infrastructure.Data.Base
{
    public abstract class BaseRepository<T> where T : class
    {
        protected readonly IDbConnection _connection;

        protected BaseRepository(IDbConnection connection)
        {
            _connection = connection;
        }

        // Common CRUD methods can be added here
    }
}
