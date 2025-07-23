using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Dapper;
using MySql.Data.MySqlClient;
using Terret_Billing.Domain.Entities;
using Terret_Billing.Domain.Interfaces;
using Terret_Billing.Infrastructure.Data.Base;

namespace Terret_Billing.Infrastructure.Data.Repositories
{
    public class UserRepository : BaseRepository<User>, IUserRepository
    {
        private readonly List<string> _connectionString;
        private readonly User _currentUser;


        public UserRepository(IDatabaseHelper databaseHelper) : base(new MySqlConnection(databaseHelper.GetConnectionString()[0]))
        {
            _connectionString = databaseHelper.GetConnectionString();
        }

        public async Task<int> AddAsync(User user)
        {
            // Step 1: Add to local & get ID
            long serverId  = await PostToServerAsync(user, _connectionString[1]);
            if (serverId <= 0) return 0;

            user.server_id = (int)serverId;

            // Step 2: Post to server with local_id reference
              await InsertToLocalAsync(user, _connectionString[0]);
            return user.id;
        }


        public async Task  InsertToLocalAsync(User user, string connectionString)
        {
            using var connection = new MySqlConnection(connectionString);
            await connection.OpenAsync();
            var parameters = BuildDapperParameters(user);
            await connection.ExecuteAsync("sp_InsertUser_local", parameters, commandType: CommandType.StoredProcedure);
          
        }
        public async Task<long> PostToServerAsync(User user, string connectionString)
        {
            using var connection = new MySqlConnection(connectionString);
            await connection.OpenAsync();

            var parameters = BuildDapperParameters(user);
            //foreach (var prop in typeof(User).GetProperties())
            //{
            //    if (!prop.CanRead) continue;
            //    parameters.Add(prop.Name, prop.GetValue(user) ?? DBNull.Value);
            //}

           
                 parameters.Add("inserted_id", dbType: DbType.Int64, direction: ParameterDirection.Output);
            await connection.ExecuteAsync("sp_InsertUserFromLocal_server", parameters, commandType: CommandType.StoredProcedure);
            return parameters.Get<long>("inserted_id");

        }

       



        public async Task<User> GetByIdAsync(int id)
        {
            using (var connection = new MySqlConnection(_connectionString[0]))
            {
                await connection.OpenAsync();
                var parameters = new { Id = id };
                return await connection.QueryFirstOrDefaultAsync<User>(
                    "GetUserById",
                    parameters,
                    commandType: CommandType.StoredProcedure
                );
            }
        }

        public async Task<IEnumerable<User>> GetAllAsync(int id)
        {
            using (var connection = new MySqlConnection(_connectionString[1]))
            {
                await connection.OpenAsync();
                var parameters = new { Id = id };
                return await connection.QueryAsync<User>(
                    "sp_GetAllUsers",
                    parameters,
                    commandType: CommandType.StoredProcedure
                );
            }
        }

        public async Task<(IEnumerable<User> Users, int TotalCount)> GetAllUsersPagedAsync(int pageNumber, int pageSize, int creatorId)
        {
            using (var connection = new MySqlConnection(_connectionString[1]))
            {
                await connection.OpenAsync();
                using (var cmd = new MySqlCommand("sp_GetAllUsersPaged", connection))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("in_pageNumber", pageNumber);
                    cmd.Parameters.AddWithValue("in_pageSize", pageSize);
                    cmd.Parameters.AddWithValue("in_creatorId", creatorId);

                    var users = new List<User>();
                    int totalCount = 0;

                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        // First result set: users
                        while (await reader.ReadAsync())
                        {
                            var user = new User
                            {
                                id = reader.GetInt32(reader.GetOrdinal("id")),
                                user_name = reader["user_name"]?.ToString(),
                                email = reader["email"]?.ToString(),
                                phone_number = reader["phone_number"]?.ToString(),
                                user_type = reader["user_type"]?.ToString(),
                                assigned_branch = reader["assigned_branch"]?.ToString(),
                                firm_id = reader["firm_id"]?.ToString(),
                                // Add more fields as needed
                            };
                            users.Add(user);
                        }
                        // Move to next result set: total count
                        if (await reader.NextResultAsync() && await reader.ReadAsync())
                        {
                            totalCount = reader.GetInt32(reader.GetOrdinal("TotalCount"));
                        }
                    }
                    return (users, totalCount);
                }
            }
        }

        public async Task UpdateAsync(User user)
        {
            using (var connection = new MySqlConnection(_connectionString[1]))
            {
                await connection.OpenAsync();

                var parameters = new DynamicParameters();
                parameters.Add("UpdatedAt", DateTime.UtcNow);
                var properties = typeof(User).GetProperties(BindingFlags.Public | BindingFlags.Instance)
                    .Where(p => p.CanRead && p.CanWrite);

                foreach (var prop in properties)
                {
                    var paramName = prop.Name;
                    var value = prop.GetValue(user) ?? DBNull.Value;
                    parameters.Add(paramName, value);
                }

                await connection.ExecuteAsync(
                    "UpdateUser",
                    parameters,
                    commandType: CommandType.StoredProcedure
                );
            }
        }
        public async Task DeleteAsync(int id)
        {
            using (var connection = new MySqlConnection(_connectionString[1]))
            {
                await connection.OpenAsync();
                var parameters = new { Id = id };
                await connection.ExecuteAsync(
                    "DeleteUser",
                    parameters,
                    commandType: CommandType.StoredProcedure
                );
            }
        }

        public async Task<bool> ExistsByUsernameAsync(string username)
        {
            using (var connection = new MySqlConnection(_connectionString[1]))
            {
                await connection.OpenAsync();
                var parameters = new { Username = username };
                var count = await connection.ExecuteScalarAsync<int>(
                    "SELECT COUNT(*) FROM users WHERE user_name = @Username",
                    parameters
                );
                return count > 0;
            }
        }

        public async Task<bool> ExistsByEmailAsync(string email)
        {
            using (var connection = new MySqlConnection(_connectionString[1]))
            {
                await connection.OpenAsync();
                var parameters = new { Email = email };
                var count = await connection.ExecuteScalarAsync<int>(
                    "SELECT COUNT(*) FROM users WHERE email = @Email",
                    parameters
                );
                return count > 0;
            }
        }

        public async Task<User> GetByUsernameAsync(string username)
        {
            using (var connection = new MySqlConnection(_connectionString[1]))
            {
                await connection.OpenAsync();
                var parameters = new { Username = username };
                return await connection.QueryFirstOrDefaultAsync<User>(
                    "GetUserByUsername",
                    parameters,
                    commandType: CommandType.StoredProcedure
                );
            }
        }

        public async Task<User> GetByEmailAsync(string email)
        {
            using (var connection = new MySqlConnection(_connectionString[1]))
            {
                await connection.OpenAsync();
                var parameters = new { Email = email };
                return await connection.QueryFirstOrDefaultAsync<User>(
                    "GetUserByEmail",
                    parameters,
                    commandType: CommandType.StoredProcedure
                );
            }
        }

        public async Task<UserPermission> GetUserPermissionsAsync(int userId)
        {
            using (var connection = new MySqlConnection(_connectionString[1]))
            {
                await connection.OpenAsync();
                var parameters = new { UserId = userId };
                return await connection.QueryFirstOrDefaultAsync<UserPermission>(
                    "GetUserPermissions",
                    parameters,
                    commandType: CommandType.StoredProcedure
                );
            }
        }

        public async Task<bool> SaveUserPermissionsAsync(UserPermission permissions)
        {
            using (var connection = new MySqlConnection(_connectionString[1]))
            {
                await connection.OpenAsync();

                var parameters = new DynamicParameters();
                var properties = typeof(UserPermission).GetProperties(BindingFlags.Public | BindingFlags.Instance)
                    .Where(p => p.CanRead && p.CanWrite);

                foreach (var prop in properties)
                {
                    var paramName = prop.Name;
                    var value = prop.GetValue(permissions) ?? DBNull.Value;
                    parameters.Add(paramName, value);
                }

                var rowsAffected = await connection.ExecuteAsync(
                    "SaveUserPermissions",
                    parameters,
                    commandType: CommandType.StoredProcedure
                );

                return rowsAffected > 0;
            }
        }
        


        private DynamicParameters BuildDapperParameters(User user)
        {
            var parameters = new DynamicParameters();
            var properties = typeof(User).GetProperties(BindingFlags.Public | BindingFlags.Instance)
                .Where(p => p.CanRead && p.CanWrite);

            foreach (var prop in properties)
            {
                var paramName = prop.Name;
                var value = prop.GetValue(user);

                if (value == null || value == DBNull.Value)
                {
                    if (prop.PropertyType == typeof(string) || prop.PropertyType == typeof(byte[]))
                        parameters.Add(paramName, null); // Safe for blobs or text
                    else
                        parameters.Add(paramName, null);
                }
                else
                {
                    parameters.Add(paramName, value);
                }
            }

            return parameters;
        }


    }
} 
