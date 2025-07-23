using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using MySql.Data.MySqlClient;
using Terret_Billing.Application.Logging;
using Terret_Billing.Application.Services.Interfaces;
using Terret_Billing.Domain.Entities;
using Terret_Billing.Infrastructure.Helpers;
using Terret_Billing.Infrastructure.Data.Repositories;
namespace Terret_Billing.Application.Services
{
    /// <summary>
    /// Implementation of IUserService using Dapper for database operations
    /// </summary>
    public class UserService : IUserService
    {
        private readonly string _connectionString;

        public UserService(string connectionString)
        {
            _connectionString = connectionString ?? throw new ArgumentNullException(nameof(connectionString));
        }

        /// <summary>
        /// Get all users
        /// </summary>
        public async Task<IEnumerable<User>> GetAllUsersAsync()
        {
            try
            {
                using (var connection = new MySqlConnection(_connectionString))
                {
                    await connection.OpenAsync();
                    
                    var query = @"SELECT id, user_name, email, phone_number, user_type, profile_image, created_by, years_of_experience, created_on 
                                FROM users";
                    
                    var users = await connection.QueryAsync<User>(query);
                    Terret_Billing.Application.Logging.Logger.LogInfo($"Retrieved {users.Count()} users from database");
                    
                    return users;
                }
            }
            catch (Exception ex)
            {
                Terret_Billing.Application.Logging.Logger.LogError("Error retrieving users", ex);
                throw;
            }
        }

        /// <summary>
        /// Get user by ID
        /// </summary>
        public async Task<User> GetUserByIdAsync(int userId)
        {
            try
            {
                using (var connection = new MySqlConnection(_connectionString))
                {
                    await connection.OpenAsync();
                    
                    var query = @"SELECT id, user_name, email, phone_number, user_type, profile_image, created_by, years_of_experience, created_on 
                                FROM users 
                                WHERE id = @UserId";
                    
                    var parameters = new { UserId = userId };
                    
                    var user = await connection.QueryFirstOrDefaultAsync<User>(query, parameters);
                    
                    if (user != null)
                    {
                        Terret_Billing.Application.Logging.Logger.LogInfo($"Retrieved user with ID {userId}");
                    }
                    else
                    {
                        Terret_Billing.Application.Logging.Logger.LogWarning($"User with ID {userId} not found");
                    }
                    
                    return user;
                }
            }
            catch (Exception ex)
            {
                Terret_Billing.Application.Logging.Logger.LogError($"Error retrieving user with ID {userId}", ex);
                throw;
            }
        }

        /// <summary>
        /// Create a new user
        /// </summary>
        public async Task<int> CreateUserAsync(User user)
        {
            if (user == null) throw new ArgumentNullException(nameof(user));
            
            try
            {
                using (var connection = new MySqlConnection(_connectionString))
                {
                    await connection.OpenAsync();
                    
                    //// Check if username or email already exists
                    //var checkQuery = @"SELECT COUNT(*) FROM users WHERE user_name = @UserName OR email = @Email";
                    //var checkParams = new { UserName = user.user_name, Email = user.email };
                    //var exists = await connection.ExecuteScalarAsync<int>(checkQuery, checkParams);
                    
                    //if (exists > 0)
                    //{
                    //    Terret_Billing.Application.Logging.Logger.LogWarning($"Username or email already exists: {user.user_name}, {user.email}");
                    //    return 0;
                    //}
                    
                    var parameters = new DynamicParameters();
                    parameters.Add("@p_user_name", user.user_name);
                    parameters.Add("@p_email", user.email);
                    parameters.Add("@p_phone_number", user.phone_number);
                    parameters.Add("@p_password", user.password);
                    parameters.Add("@p_user_type", user.user_type);
                    parameters.Add("@p_profile_image", user.profile_image);
                    parameters.Add("@p_created_by", user.created_by);
                    parameters.Add("@p_years_of_exp", user.years_of_experience);
                    parameters.Add("@p_address", user.address);
                    parameters.Add("@p_gender", user.gender);
                    parameters.Add("@p_aadhar_front", user.aadhar_front);
                    parameters.Add("@p_aadhar_back", user.aadhar_back);
                    parameters.Add("@p_pancard", user.pancard);
                    parameters.Add("@p_resume", user.resume);
                    parameters.Add("@p_certificate", user.certificate);
                    parameters.Add("@p_others", user.others);
                    parameters.Add("@p_salary", user.salary);
                    parameters.Add("@p_firm_id", user.firm_id);
                    parameters.Add("@p_assigned_branch", user.assigned_branch);

                    var userId = await connection.ExecuteScalarAsync<int>(
                        "InsertUser",
                        parameters,
                        commandType: CommandType.StoredProcedure
                    );
                    Terret_Billing.Application.Logging.Logger.LogInfo($"InsertUser returned userId: {userId}");
                    System.Windows.MessageBox.Show($"InsertUser returned userId: {userId}", "Debug", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Information);
                    return userId;
                }
            }
            catch (Exception ex)
            {
                Terret_Billing.Application.Logging.Logger.LogError("Error creating user", ex);
                throw;
            }
        }

        /// <summary>
        /// Update an existing user
        /// </summary>
        public async Task<bool> UpdateUserAsync(User user)
        {
            if (user == null) throw new ArgumentNullException(nameof(user));
            if (user.id <= 0) throw new ArgumentException("User ID must be greater than zero", nameof(user));
            
            try
            {
                using (var connection = new MySqlConnection(_connectionString))
                {
                    await connection.OpenAsync();
                    
                    // Check if username or email already exists for another user
                    var checkQuery = @"SELECT COUNT(*) FROM users WHERE (user_name = @UserName OR email = @Email) AND id != @Id";
                    var checkParams = new { UserName = user.user_name, Email = user.email, Id = user.id };
                    var exists = await connection.ExecuteScalarAsync<int>(checkQuery, checkParams);
                    
                    if (exists > 0)
                    {
                        Terret_Billing.Application.Logging.Logger.LogWarning($"Username or email already exists for another user: {user.user_name}, {user.email}");
                        return false;
                    }
                    
                    var query = @"UPDATE users 
                                SET user_name = @user_name, 
                                    email = @email, 
                                    phone_number = @phone_number,
                                    user_type = @user_type,
                                    profile_image = @profile_image,
                                    years_of_experience = @years_of_experience
                                WHERE id = @id";
                    
                    var rowsAffected = await connection.ExecuteAsync(query, user);
                    
                    if (rowsAffected > 0)
                    {
                        Terret_Billing.Application.Logging.Logger.LogInfo($"Updated user with ID {user.id}");
                        return true;
                    }
                    else
                    {
                        Terret_Billing.Application.Logging.Logger.LogWarning($"User with ID {user.id} not found for update");
                        return false;
                    }
                }
            }
            catch (Exception ex)
            {
                Terret_Billing.Application.Logging.Logger.LogError($"Error updating user with ID {user.id}", ex);
                throw;
            }
        }

        /// <summary>
        /// Delete a user
        /// </summary>
        public async Task<bool> DeleteUserAsync(int userId)
        {
            if (userId <= 0) throw new ArgumentException("User ID must be greater than zero", nameof(userId));
            
            try
            {
                using (var connection = new MySqlConnection(_connectionString))
                {
                    await connection.OpenAsync();
                    
                    // Begin transaction to ensure both user and permissions are deleted
                    using (var transaction = await connection.BeginTransactionAsync())
                    {
                        try
                        {
                            // Delete user permissions first (foreign key constraint)
                            var deletePermissionsQuery = "DELETE FROM user_permissions WHERE user_id = @UserId";
                            await connection.ExecuteAsync(deletePermissionsQuery, new { UserId = userId }, transaction);
                            
                            // Delete user
                            var deleteUserQuery = "DELETE FROM users WHERE id = @UserId";
                            var rowsAffected = await connection.ExecuteAsync(deleteUserQuery, new { UserId = userId }, transaction);
                            
                            // Commit transaction
                            await transaction.CommitAsync();
                            
                            if (rowsAffected > 0)
                            {
                                Terret_Billing.Application.Logging.Logger.LogInfo($"Deleted user with ID {userId}");
                                return true;
                            }
                            else
                            {
                                Terret_Billing.Application.Logging.Logger.LogWarning($"User with ID {userId} not found for deletion");
                                return false;
                            }
                        }
                        catch (Exception ex)
                        {
                            // Rollback transaction on error
                            await transaction.RollbackAsync();
                            Terret_Billing.Application.Logging.Logger.LogError($"Error in transaction while deleting user with ID {userId}", ex);
                            throw;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Terret_Billing.Application.Logging.Logger.LogError($"Error deleting user with ID {userId}", ex);
                throw;
            }
        }

        /// <summary>
        /// Save user permissions
        /// </summary>
        public async Task<bool> SaveUserPermissionsAsync(UserPermission permissions)
        {
            if (permissions == null) throw new ArgumentNullException(nameof(permissions));
            if (permissions.user_id <= 0) throw new ArgumentException("User ID must be greater than zero", nameof(permissions));
            
            try
            {
                using (var connection = new MySqlConnection(_connectionString))
                {
                    await connection.OpenAsync();
                    
                    // Check if permissions already exist for this user
                    var checkQuery = "SELECT COUNT(*) FROM user_permissions WHERE user_id = @UserId";
                    var exists = await connection.ExecuteScalarAsync<int>(checkQuery, new { UserId = permissions.user_id });
                    
                    string query;
                    int rowsAffected;
                    
                    if (exists > 0)
                    {
                        // Update existing permissions
                        query = @"UPDATE user_permissions 
                                SET branch_id = @branch_id,
                                    can_create_users = @can_create_users,
                                    can_edit_company_settings = @can_edit_company_settings,
                                    can_view_reports = @can_view_reports,
                                    can_create_edit_invoices = @can_create_edit_invoices,
                                    can_manage_inventory = @can_manage_inventory,
                                    is_active = @is_active
                                WHERE user_id = @user_id";
                        
                        rowsAffected = await connection.ExecuteAsync(query, permissions);
                        
                        if (rowsAffected > 0)
                        {
                            Terret_Billing.Application.Logging.Logger.LogInfo($"Updated permissions for user ID {permissions.user_id}");
                        }
                        else
                        {
                            Terret_Billing.Application.Logging.Logger.LogWarning($"Failed to update permissions for user ID {permissions.user_id}");
                        }
                    }
                    else
                    {
                        // Insert new permissions
                        query = @"INSERT INTO user_permissions (user_id, branch_id, can_create_users, can_edit_company_settings, 
                                                            can_view_reports, can_create_edit_invoices, can_manage_inventory, 
                                                            is_active, created_on, created_by)
                                VALUES (@user_id, @branch_id, @can_create_users, @can_edit_company_settings, 
                                        @can_view_reports, @can_create_edit_invoices, @can_manage_inventory, 
                                        @is_active, @created_on, @created_by)";
                        
                        // Set creation timestamp if not already set
                        if (!permissions.created_on.HasValue)
                        {
                            permissions.created_on = DateTime.Now;
                        }
                        
                        rowsAffected = await connection.ExecuteAsync(query, permissions);
                        
                        if (rowsAffected > 0)
                        {
                            Terret_Billing.Application.Logging.Logger.LogInfo($"Created permissions for user ID {permissions.user_id}");
                        }
                        else
                        {
                            Terret_Billing.Application.Logging.Logger.LogWarning($"Failed to create permissions for user ID {permissions.user_id}");
                        }
                    }
                    
                    return rowsAffected > 0;
                }
            }
            catch (Exception ex)
            {
                Terret_Billing.Application.Logging.Logger.LogError($"Error saving permissions for user ID {permissions.user_id}", ex);
                throw;
            }
        }

        /// <summary>
        /// Get user permissions
        /// </summary>
        public async Task<UserPermission> GetUserPermissionsAsync(int userId)
        {
            if (userId <= 0) throw new ArgumentException("User ID must be greater than zero", nameof(userId));
            
            try
            {
                using (var connection = new MySqlConnection(_connectionString))
                {
                    await connection.OpenAsync();
                    
                    var query = @"SELECT id, user_id, branch_id, can_create_users, can_edit_company_settings, 
                                       can_view_reports, can_create_edit_invoices, can_manage_inventory, 
                                       is_active, created_on, created_by
                                FROM user_permissions 
                                WHERE user_id = @UserId";
                    
                    var permissions = await connection.QueryFirstOrDefaultAsync<UserPermission>(query, new { UserId = userId });
                    
                    if (permissions != null)
                    {
                        Terret_Billing.Application.Logging.Logger.LogInfo($"Retrieved permissions for user ID {userId}");
                    }
                    else
                    {
                        Terret_Billing.Application.Logging.Logger.LogWarning($"No permissions found for user ID {userId}");
                    }
                    
                    return permissions;
                }
            }
            catch (Exception ex)
            {
                Terret_Billing.Application.Logging.Logger.LogError($"Error retrieving permissions for user ID {userId}", ex);
                throw;
            }
        }

        public async Task<(IEnumerable<User> Users, int TotalCount)> GetAllUsersPagedAsync(int pageNumber, int pageSize, int creatorId)
        {
            var repo = new UserRepository(new Infrastructure.Data.MySqlDatabaseHelper());
            return await repo.GetAllUsersPagedAsync(pageNumber, pageSize, creatorId);
        }
    }
}
