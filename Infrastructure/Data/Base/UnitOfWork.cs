using System;
using System.Data;

namespace Terret_Billing.Infrastructure.Data.Base
{
    public class UnitOfWork : IDisposable
    {
        private readonly IDbConnection _connection;
        private IDbTransaction _transaction;
        public UnitOfWork(IDbConnection connection)
        {
            _connection = connection;
            _connection.Open();
            _transaction = _connection.BeginTransaction();
        }
        public void Commit() => _transaction?.Commit();
        public void Rollback() => _transaction?.Rollback();
        public void Dispose() { _transaction?.Dispose(); _connection?.Dispose(); }
        public IDbTransaction Transaction => _transaction;
        public IDbConnection Connection => _connection;
    }
}
