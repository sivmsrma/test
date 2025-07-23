using System;
using System.Configuration;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using MySql.Data.MySqlClient;
using Terret_Billing.Application.Interfaces;
using Terret_Billing.Domain.Entities;
using Terret_Billing.Infrastructure.Helpers;

namespace Terret_Billing.Application.Services
{
    public class AuthenticationService : IAuthenticationService
    {
        private  string _connectionString;

        public AuthenticationService()
        {
            _connectionString = ConfigurationManager.ConnectionStrings["BillingDblocal"].ConnectionString;
            _connectionString = ConfigurationManager.ConnectionStrings["BillingDb"].ConnectionString;

        }

        public async Task<User> AuthenticateAsync(string username, string password,string logintype)
        {
            try
            {


                if (logintype == "SuperAdmin")
                {
                    _connectionString = ConfigurationManager.ConnectionStrings["BillingDb"].ConnectionString;
                }
                else if (logintype == "User")
                {
                    _connectionString = ConfigurationManager.ConnectionStrings["BillingDblocal"].ConnectionString;
                }
                else
                {
                    throw new ArgumentException("Invalid login type");
                }
                // Log authentication attempt - using Infrastructure.Helpers.Logger
                Infrastructure.Helpers.Logger.LogInfo($"Authentication attempt for username: {username}");

                using (var connection = new MySqlConnection(_connectionString))
                {

                    // Ensure connection is valid
                    if (connection == null)
                    {
                        Infrastructure.Helpers.Logger.LogError("Connection object is null");
                        throw new InvalidOperationException("Database connection could not be created");
                    }

                    try
                    {
                        await connection.OpenAsync();
                    }
                    catch (MySqlException ex)
                    {
                        Infrastructure.Helpers.Logger.LogError($"Failed to open database connection: {ex.Message}", ex);
                        throw new InvalidOperationException("Could not connect to the database", ex);
                    }

                    // Use a simpler query for troubleshooting
                    
                    var query = "CALL sp_login_user_or_admin(@Username, @Password)";

                    var parameters = new
                    {
                        Username = username,
                        Password = password
                    };
                    try
                    {
                        // Use Dapper to query the database
                        var users = await connection.QueryAsync<User>(query, parameters);
                        var user = users.FirstOrDefault();

                        if (user != null)
                        {
                            Infrastructure.Helpers.Logger.LogInfo($"User found: {user.user_name}, Type: {user.user_type}");
                        }
                        else
                        {
                            Infrastructure.Helpers.Logger.LogInfo("No user found with provided credentials");
                        }

                        return user;
                    }
                    catch (Exception ex)
                    {
                        Infrastructure.Helpers.Logger.LogError($"Database query failed: {ex.Message}", ex);
                        throw new InvalidOperationException("Error executing database query", ex);
                    }
                }
            }
            catch (Exception ex)
            {
                Infrastructure.Helpers.Logger.LogError($"Authentication failed: {ex.Message}", ex);
                throw;
            }
        }

        public bool ValidateCredentials(string username, string password)
        {
            try
            {
                using (var connection = new MySqlConnection(_connectionString))
                {
                    connection.Open();

                    var query = @"SELECT COUNT(1) FROM users 
                               WHERE (email = @Username OR phone_number = @Username OR user_name = @Username) AND password = @Password";

                    var parameters = new { Username = username, Password = password };

                    // Use Dapper to execute scalar query
                    var count = connection.ExecuteScalar<int>(query, parameters);
                    return count > 0;
                }
            }
            catch (Exception ex)
            {
                Logger.LogError("Credential validation failed", ex);
                return false;
            }
        }
    }
}
