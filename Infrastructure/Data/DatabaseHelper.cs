using System;
using System.Data;
using MySql.Data.MySqlClient;
using System.Configuration;
using Dapper;
using Terret_Billing.Application.Logging;
using System.Collections.Generic;
using System.Data.Common;

namespace Terret_Billing.Infrastructure.Data
{
    public class DatabaseHelper : IDatabaseHelper
    {
        private static readonly string ConnectionString = ConfigurationManager.ConnectionStrings["BillingDb"].ConnectionString;
        private static readonly string ConnectionStringServer = ConfigurationManager.ConnectionStrings["BillingDbServer"]?.ConnectionString;

        public DbConnection CreateConnection()
        {
            try
            {
                var connection = new MySqlConnection(ConnectionString);
                connection.Open();
                return connection;
            }
            catch (Exception ex)
            {
                Logger.LogError("Failed to create database connection", ex);
                throw;
            }
        }

        public static MySqlCommand CreateCommand(string commandText, MySqlConnection connection)
        {
            var command = new MySqlCommand(commandText, connection);
            command.CommandType = CommandType.Text;
            return command;
        }

        public static MySqlCommand CreateCommand(string commandText, MySqlConnection connection, CommandType commandType)
        {
            var command = new MySqlCommand(commandText, connection);
            command.CommandType = commandType;
            return command;
        }

        public DbCommand CreateStoredProcedureCommand(string procedureName, DbConnection connection)
        {
            var command = connection.CreateCommand();
            command.CommandText = procedureName;
            command.CommandType = CommandType.StoredProcedure;
            return command;
        }

        public void ExecuteNonQuery(string sql, object parameters = null)
        {
            try
            {
                using (var connection = CreateConnection())
                {
                    connection.Execute(sql, parameters);
                }
            }
            catch (Exception ex)
            {
                Logger.LogError($"Failed to execute non-query: {sql}", ex);
                throw;
            }
        }

        public void ExecuteStoredProcedure(string procedureName, object parameters = null)
        {
            try
            {
                using (var connection = (MySqlConnection)CreateConnection())
                {
                    connection.Execute(procedureName, parameters, commandType: CommandType.StoredProcedure);
                }
            }
            catch (Exception ex)
            {
                Logger.LogError($"Failed to execute stored procedure: {procedureName}", ex);
                throw;
            }
        }

        public T ExecuteScalar<T>(string sql, object parameters = null)
        {
            try
            {
                using (var connection = CreateConnection())
                {
                    return connection.ExecuteScalar<T>(sql, parameters);
                }
            }
            catch (Exception ex)
            {
                Logger.LogError($"Failed to execute scalar: {sql}", ex);
                throw;
            }
        }

        public IEnumerable<T> Query<T>(string sql, object parameters = null)
        {
            try
            {
                using (var connection = CreateConnection())
                {
                    return connection.Query<T>(sql, parameters);
                }
            }
            catch (Exception ex)
            {
                Logger.LogError($"Failed to execute query: {sql}", ex);
                throw;
            }
        }

        public IEnumerable<T> QueryStoredProcedure<T>(string procedureName, object parameters = null)
        {
            try
            {
                using (var connection = CreateConnection())
                {
                    return connection.Query<T>(procedureName, parameters, commandType: CommandType.StoredProcedure);
                }
            }
            catch (Exception ex)
            {
                Logger.LogError($"Failed to execute stored procedure query: {procedureName}", ex);
                throw;
            }
        }

        public List<string> GetConnectionString()
        {
            var list = new List<string> { ConnectionString };
            if (!string.IsNullOrWhiteSpace(ConnectionStringServer))
                list.Add(ConnectionStringServer);
            return list;
        }
    }
} 