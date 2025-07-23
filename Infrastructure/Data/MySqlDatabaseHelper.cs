using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Common;
using MySql.Data.MySqlClient;

namespace Terret_Billing.Infrastructure.Data
{
    public class MySqlDatabaseHelper : IDatabaseHelper
    {
        private readonly string _connectionStringLocalDb;
        private readonly string _connectionStringServerDb;
        private const string LOCAL_DB_NAME = "BillingDblocal";
        private const string SERVER_DB_NAME = "BillingDb";

        public MySqlDatabaseHelper()
        {
            _connectionStringLocalDb = GetValidatedConnectionString(LOCAL_DB_NAME);
            _connectionStringServerDb = GetValidatedConnectionString(SERVER_DB_NAME);
        }

        private string GetValidatedConnectionString(string connectionStringName)
        {
            var connectionString = ConfigurationManager.ConnectionStrings[connectionStringName]?.ConnectionString;
            
            if (string.IsNullOrWhiteSpace(connectionString))
            {
                throw new ConfigurationErrorsException($"Connection string '{connectionStringName}' is missing or empty in configuration.");
            }

            // Add default parameters if not present
            var builder = new MySqlConnectionStringBuilder(connectionString);
            if (!builder.ContainsKey("AllowUserVariables")) builder.AllowUserVariables = true;
            if (!builder.ContainsKey("AllowPublicKeyRetrieval")) builder.AllowPublicKeyRetrieval = true;
            if (!builder.ContainsKey("ConvertZeroDateTime")) builder.ConvertZeroDateTime = true;

            return builder.ConnectionString;
        }

        public List<string> GetConnectionString()
        {
            return new List<string>
            {
                _connectionStringLocalDb,
                _connectionStringServerDb
            };
        }

        public DbConnection CreateConnection()
        {
            // Replace with your actual connection string
            return new MySqlConnection("your-connection-string");
        }

        public DbCommand CreateStoredProcedureCommand(string procedureName, DbConnection connection)
        {
            if (string.IsNullOrWhiteSpace(procedureName))
                throw new ArgumentException("Procedure name cannot be empty.", nameof(procedureName));

            if (connection == null)
                throw new ArgumentNullException(nameof(connection));

            var command = connection.CreateCommand();
            command.CommandText = procedureName;
            command.CommandType = CommandType.StoredProcedure;
            return command;
        }
    }
}