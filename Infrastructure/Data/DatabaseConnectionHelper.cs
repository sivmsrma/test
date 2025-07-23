using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Common;
using MySql.Data.MySqlClient;

namespace Terret_Billing.Infrastructure.Data
{
    public class DatabaseConnectionHelper : IDatabaseHelper
    {
        private readonly string _connectionString;

        public DatabaseConnectionHelper(string connectionString)
        {
            _connectionString = connectionString ?? throw new ArgumentNullException(nameof(connectionString));
        }

        public List<string> GetConnectionString()
        {
            return new List<string> { _connectionString };
        }

        public DbConnection CreateConnection()
        {
            // Replace with your actual connection string
            return new MySqlConnection("your-connection-string");
        }

        public DbCommand CreateStoredProcedureCommand(string procedureName, DbConnection connection)
        {
            var command = new MySqlCommand(procedureName, (MySqlConnection)connection);
            command.CommandType = CommandType.StoredProcedure;
            return command;
        }
    }
} 