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
using Terret_Billing.Domain.Interfaces;
using Terret_Billing.Infrastructure.Data;
using Terret_Billing.Presentation.Models;

namespace Terret_Billing.Application.Services
{
    /// <summary>
    /// Implementation of IBranchService using Dapper for database operations
    /// </summary>
    public class BranchRepository : IBranchRepository
    {    

        

        private readonly List<string> _connectionString;
        private DatabaseHelper databaseHelper;
        private string connectionString;

        public BranchRepository(IDatabaseHelper databaseHelper)
        {
            _connectionString = databaseHelper.GetConnectionString();
        }

        //public BranchRepository(string connectionString)
        //{
        //    this.connectionString = connectionString;
        //}

        /// <summary>
        /// Get all branches
        /// </summary>
        /// 
        public async Task<long> InsertCompanyDetailsAsync(Branch branch, int userId)
        {
            long localId = await PostAddAsyncToServer(branch, _connectionString, userId);

            if (localId > 0)
            {
                branch.server_id = localId; // Optional: Save for further use
                await PostAddAsyncToLocal(branch, _connectionString, userId);
            }           

            return localId;
        }

       

        public async Task<long> PostAddAsyncToServer(Branch branch, List<string> _connectionString, int userId)
        {
            long serverid = 0;
            try
            {
                using (var connection = new MySqlConnection(_connectionString[1]))
                {
                    var parameters = new DynamicParameters(branch);

                    parameters.Add("user_id", userId);
                    parameters.Add("inserted_id", dbType: DbType.Int64, direction: ParameterDirection.Output);

                    await connection.ExecuteAsync("sp_InsertCompanyDetailsAndReturnId_server", parameters, commandType: CommandType.StoredProcedure);
                    serverid = parameters.Get<long>("inserted_id");
                    return serverid;
                }
            }
            catch (Exception ex)
            {
                Terret_Billing.Application.Logging.Logger.LogError("Error inserting company details (server)", ex);
                throw;
            }
            
        }
        public async Task  PostAddAsyncToLocal(Branch branch, List<string> _connectionString, int userId)
        {
            try
            {
                using (var connection = new MySqlConnection(_connectionString[0]))
                {
                    var parameters = new DynamicParameters(branch);

                    parameters.Add("user_id", userId);
                   
                    await connection.ExecuteAsync("sp_InsertCompanyDetailsAndReturnId_local", parameters, commandType: CommandType.StoredProcedure).ConfigureAwait(false);

                    
                }
            }
            catch (Exception ex)
            {
                Terret_Billing.Application.Logging.Logger.LogError("Error inserting company details (local)", ex);
                 
            }
        }


        public async Task<IEnumerable<Branch>> GetAllBranchesAsync()
        {
            try
            {
                using (var connection = new MySqlConnection(_connectionString[0]))
                {
                    await connection.OpenAsync();

                    var query = @"SELECT id, shop_name, manager_name, address, phone_number, email, created_on, created_by 
                                FROM branches";

                    var branches = await connection.QueryAsync<Branch>(query);
                    Logger.LogInfo($"Retrieved {branches.Count()} branches from database");

                    return branches;
                }
            }
            catch (Exception ex)
            {
                Logger.LogError("Error retrieving branches", ex);
                throw;
            }
        }

        /// <summary>
        /// Get branch by ID
        /// </summary>
        public async Task<Branch> GetBranchByIdAsync(int branchId)
        {
            try
            {
                using (var connection = new MySqlConnection(_connectionString[0]))
                {
                    await connection.OpenAsync();

                    var query = @"SELECT id, shop_name, manager_name, address, phone_number, email, created_on, created_by 
                                FROM branches 
                                WHERE id = @BranchId";

                    var parameters = new { BranchId = branchId };

                    var branch = await connection.QueryFirstOrDefaultAsync<Branch>(query, parameters);

                    if (branch != null)
                    {
                        Logger.LogInfo($"Retrieved branch with ID {branchId}");
                    }
                    else
                    {
                        Logger.LogWarning($"Branch with ID {branchId} not found");
                    }

                    return branch;
                }
            }
            catch (Exception ex)
            {
                Logger.LogError($"Error retrieving branch with ID {branchId}", ex);
                throw;
            }
        }

        /// <summary>
        /// Create a new branch
        /// </summary>
        public async Task<int> CreateBranchAsync(Branch branch)
        {
            if (branch == null) throw new ArgumentNullException(nameof(branch));

            try
            {
                using (var connection = new MySqlConnection(_connectionString[0]))
                {
                    await connection.OpenAsync();

                    var query = @"INSERT INTO branches (shop_name, manager_name, address, phone_number, email, created_on, created_by) 
                                VALUES (@shop_name, @manager_name, @address, @phone_number, @email, @created_on, @created_by);
                                SELECT LAST_INSERT_ID();";

                    // Set creation timestamp if not already set
                    if (!branch.created_on.HasValue)
                    {
                        branch.created_on = DateTime.Now;
                    }

                    var branchId = await connection.ExecuteScalarAsync<int>(query, branch);
                    Logger.LogInfo($"Created new branch with ID {branchId}");

                    return branchId;
                }
            }
            catch (Exception ex)
            {
                Logger.LogError("Error creating branch", ex);
                throw;
            }
        }

        /// <summary>
        /// Update an existing branch
        /// </summary>
        public async Task<bool> UpdateBranchAsync(Branch branch)
        {
            if (branch == null) throw new ArgumentNullException(nameof(branch));
            if (branch.id <= 0) throw new ArgumentException("Branch ID must be greater than zero", nameof(branch));

            try
            {
                using (var connection = new MySqlConnection(_connectionString[0]))
                {
                    await connection.OpenAsync();

                    var query = @"UPDATE branches 
                                SET shop_name = @shop_name, 
                                    manager_name = @manager_name, 
                                    address = @address, 
                                    phone_number = @phone_number, 
                                    email = @email 
                                WHERE id = @id";

                    var rowsAffected = await connection.ExecuteAsync(query, branch);

                    if (rowsAffected > 0)
                    {
                        Logger.LogInfo($"Updated branch with ID {branch.id}");
                        return true;
                    }
                    else
                    {
                        Logger.LogWarning($"Branch with ID {branch.id} not found for update");
                        return false;
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.LogError($"Error updating branch with ID {branch.id}", ex);
                throw;
            }
        }

        /// <summary>
        /// Delete a branch
        /// </summary>
        public async Task<bool> DeleteBranchAsync(int branchId)
        {
            if (branchId <= 0) throw new ArgumentException("Branch ID must be greater than zero", nameof(branchId));

            try
            {
                using (var connection = new MySqlConnection(_connectionString[0]))
                {
                    await connection.OpenAsync();

                    var query = "DELETE FROM branches WHERE id = @BranchId";
                    var parameters = new { BranchId = branchId };

                    var rowsAffected = await connection.ExecuteAsync(query, parameters);

                    if (rowsAffected > 0)
                    {
                        Logger.LogInfo($"Deleted branch with ID {branchId}");
                        return true;
                    }
                    else
                    {
                        Logger.LogWarning($"Branch with ID {branchId} not found for deletion");
                        return false;
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.LogError($"Error deleting branch with ID {branchId}", ex);
                throw;
            }
        }

        /// <summary>
        /// Get sales summary for a branch
        /// </summary>
        public async Task<BranchSummary> GetBranchSummaryAsync(int branchId)
        {
            try
            {
                using (var connection = new MySqlConnection(_connectionString[1]))
                {
                    await connection.OpenAsync();
                    var parameters = new { branch_id = branchId };
                    // Using SQL to calculate the summary directly in the database for better performance
                    var summary = await connection.QueryFirstOrDefaultAsync<BranchSummary>(
                   "sp_GetBranchSalesAndProfit",
                   parameters,
                   commandType: CommandType.StoredProcedure
               );

                    //if (summary != null)
                    //{
                    //    Logger.LogInfo($"Retrieved summary for branch ID {branchId}");
                    //}
                    //else
                    //{
                    //    Logger.LogWarning($"Branch with ID {branchId} not found for summary");

                    //    // Create an empty summary if branch not found
                    //    summary = new BranchSummary
                    //    {
                    //        branch_id = branchId,
                    //        branch_name = "Unknown",
                    //        manager_name = "Unknown",
                    //        total_sales = 0,
                    //        total_purchases = 0,
                    //        total_profit = 0
                    //    };
                    //}

                    return summary;
                }
            }
            catch (Exception ex)
            {
                Logger.LogError($"Error retrieving summary for branch ID {branchId}", ex);
                throw;
            }
        }

        /// <summary>
        /// Get sales summary for all branches
        /// </summary>
        //public async Task<IEnumerable<BranchSummary>> GetAllBranchSummariesAsync()
        //{
        //    try
        //    {
        //        using (var connection = new MySqlConnection(_connectionString[0]))
        //        {
        //            await connection.OpenAsync();

        //            // Using SQL to calculate the summary directly in the database for better performance
        //            var query = @"
        //                SELECT 
        //                    b.id as branch_id,
        //                    b.shop_name as branch_name,
        //                    b.manager_name,
        //                    COALESCE(SUM(s.total_amount), 0) as total_sales,
        //                    COALESCE(SUM(p.total_amount), 0) as total_purchases,
        //                    COALESCE(SUM(s.total_amount), 0) - COALESCE(SUM(p.total_amount), 0) as total_profit
        //                FROM 
        //                    branches b
        //                LEFT JOIN 
        //                    sales s ON b.id = s.branch_id
        //                LEFT JOIN 
        //                    purchases p ON b.id = p.branch_id
        //                GROUP BY 
        //                    b.id, b.shop_name, b.manager_name
        //                ORDER BY 
        //                    total_sales DESC";

        //            var summaries = await connection.QueryAsync<BranchSummary>(query);
        //            Logger.LogInfo($"Retrieved summaries for {summaries.Count()} branches");

        //            return summaries;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        Logger.LogError("Error retrieving branch summaries", ex);
        //        throw;
        //    }
        //}
        /// <summary>
        /// Save a branch (insert or update based on id)
        /// </summary>
        public async Task<bool> SaveBranchAsync(Branch branch)
        {
            if (branch == null) throw new ArgumentNullException(nameof(branch));
            if (branch.id > 0)
            {
                // Update existing branch
                return await UpdateBranchAsync(branch);
            }
            else
            {
                // Insert new branch
                var newId = await CreateBranchAsync(branch);
                return newId > 0;
            }
        }
        /// <summary>
        /// Insert company details using stored procedure
        /// </summary>

        /// <summary>
        /// Get all company details from company_details table
        /// </summary>
        public async Task<IEnumerable<Branch>> GetAllCompanyDetailsAsync(int id)
        {
            try
            {
                using (var connection = new MySqlConnection(_connectionString[1]))
                {
                    await connection.OpenAsync();
                    var query = "SELECT * FROM Company_details Where user_id ="+ @id;
                    var branches = await connection.QueryAsync<Branch>(query);
                    Terret_Billing.Application.Logging.Logger.LogInfo($"Retrieved {branches.AsList().Count} company details from database");
                    return branches;
                }
            }
            catch (Exception ex)
            {
                Terret_Billing.Application.Logging.Logger.LogError("Error retrieving company details", ex);
                throw;
            }
        }
        public async Task<Branch> GetCompanyDetailsByFirmIdAsync(string firmId)
        {
            using (var connection = new MySql.Data.MySqlClient.MySqlConnection(_connectionString[0]))
            {
                var query = "SELECT * FROM company_details WHERE FirmId = @firmId LIMIT 1";
                return await connection.QueryFirstOrDefaultAsync<Branch>(query, new { firmId });
            }
        }
    }
}
