using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using Dapper;
using MySql.Data.MySqlClient;
using Terret_Billing.Domain.Entities;
using Terret_Billing.Domain.Interfaces;
using Terret_Billing.Infrastructure.Data;

namespace Terret_Billing.Infrastructure.Data.Repositories
{
    public class TaggingRepository : ITaggingRepository
    {
        private readonly List<string> _connectionString;
        private readonly IDatabaseHelper _databaseHelper;

        public TaggingRepository(IDatabaseHelper databaseHelper)
        {
            _databaseHelper = databaseHelper;
            _connectionString = databaseHelper.GetConnectionString();
        }

        public async Task<TaggingItem> GetByBarcodeAsync(string barcode)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(barcode))
                    return null;
                    
                using (var connection = new MySqlConnection(_connectionString[0]))
                {
                    await connection.OpenAsync();
                    
                    var parameters = new { in_barcode = barcode };
                    
                    var result = await connection.QueryFirstOrDefaultAsync<TaggingItem>(
                        "sp_GetTaggingItemByBarcode",
                        parameters,
                        commandType: CommandType.StoredProcedure);
                        
                    return result;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in GetByBarcodeAsync: {ex.Message}");
                throw new ApplicationException($"Error retrieving item with barcode {barcode}", ex);
            }
        }

        public async Task<IEnumerable<TaggingItem>> GetAllAsync(string firmId = null)
        {
            try
            {
                using (var connection = new MySqlConnection(_connectionString[0])) // Using local connection
                {
                    await connection.OpenAsync();

                    if (!string.IsNullOrEmpty(firmId))
                    {
                        return await connection.QueryAsync<TaggingItem>(
                            "sp_GetAllTaggingItems",
                            new { firmId },
                            commandType: CommandType.StoredProcedure);
                    }

                    return await connection.QueryAsync<TaggingItem>(
                        "sp_GetAllTaggingItems",
                        commandType: CommandType.StoredProcedure);
                }
            }
            catch (Exception ex)
            {
                // Log error
                Console.WriteLine($"Error in GetAllAsync: {ex.Message}");
                throw;
            }
        }

        public async Task<(IEnumerable<TaggingItem> Items, int TotalCount)> GetAllPagedAsync(
            string firmId,
            string typeOfStock,
            string metalType,
            string partyName,
            string invoiceNumber,
            int pageNumber,
            int pageSize)
        {
            using (var connection = new MySqlConnection(_connectionString[0]))
            {
                await connection.OpenAsync();
                var parameters = new
                {
                    firmId = firmId ?? string.Empty,
                    filterTypeOfStock = typeOfStock ?? string.Empty,
                    filterMetalType = metalType ?? string.Empty,
                    filterPartyName = partyName ?? string.Empty,
                    filterInvoiceNumber = invoiceNumber ?? string.Empty,
                    pageNumber,
                    pageSize
                };
                using (var multi = await connection.QueryMultipleAsync(
                    "sp_GetAllTaggingItemsPaged", parameters, commandType: CommandType.StoredProcedure))
                {
                    var totalCountObj = await multi.ReadFirstAsync<dynamic>();
                    int totalCount = Convert.ToInt32(totalCountObj.TotalCount);
                    var items = (await multi.ReadAsync<TaggingItem>()).AsList();
                    return (items, totalCount);
                }
            }
        }

        public async Task<TaggingItem> GetByIdAsync(long id)
        {
            try
            {
                using (var connection = new MySqlConnection(_connectionString[0]))
                {
                    await connection.OpenAsync();
                    return await connection.QueryFirstOrDefaultAsync<TaggingItem>(
                        "sp_GetTaggingItemById",
                        new { id },
                        commandType: CommandType.StoredProcedure);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in GetByIdAsync: {ex.Message}");
                throw;
            }
        }

        public async Task AddAsync(TaggingItem item)
        {
            await PostAddAsyncToLocal(item, _connectionString);
            await PostAddAsyncToServer(item, _connectionString);
        }

        private async Task PostAddAsyncToLocal(TaggingItem item, List<string> connectionStrings)
        {
            try
            {
                using (var connection = new MySqlConnection(connectionStrings[0]))
                {
                    await connection.OpenAsync();
                    var parameters = new DynamicParameters(item);

                    // Add OUT parameters if needed
                    parameters.Add("NewTaggingId", dbType: DbType.Int64, direction: ParameterDirection.Output);

                    await connection.ExecuteAsync(
                        "sp_AddTaggingItem_local",
                        parameters,
                        commandType: CommandType.StoredProcedure
                    );

                    // Get the generated ID
                    item.stock_id = parameters.Get<long>("NewTaggingId");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in PostAddAsyncToLocal: {ex.Message}");
                throw;
            }
        }

        private async Task PostAddAsyncToServer(TaggingItem item, List<string> connectionStrings)
        {
            try
            {
                using (var connection = new MySqlConnection(connectionStrings[1]))
                {
                    await connection.OpenAsync();

                    await connection.ExecuteAsync(
                        "sp_AddTaggingItem_server",
                        item,
                        commandType: CommandType.StoredProcedure
                    );

                    // Mark as synced in local DB
                    using (var localConn = new MySqlConnection(connectionStrings[0]))
                    {
                        await localConn.ExecuteAsync(
                            "sp_MarkTaggingAsSynced",
                            new { p_LocalId = item.stock_id },
                            commandType: CommandType.StoredProcedure
                        );
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in PostAddAsyncToServer: {ex.Message}");
                // Handle server sync failure (maybe queue for later sync)
            }
        }

        public async Task UpdateAsync(TaggingItem item)
        {
            try
            {
                using (var connection = new MySqlConnection(_connectionString[0]))
                {
                    await connection.OpenAsync();

                    var parameters = new DynamicParameters(item);
                    parameters.Add("p_UpdatedAt", DateTime.UtcNow);

                    await connection.ExecuteAsync(
                        "sp_UpdateTaggingItem",
                        parameters,
                        commandType: CommandType.StoredProcedure
                    );
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in UpdateAsync: {ex.Message}");
                throw;
            }
        }

        public async Task DeleteAsync(long id)
        {
            try
            {
                using (var connection = new MySqlConnection(_connectionString[0]))
                {
                    await connection.OpenAsync();
                    var parameters = new { id };
                    await connection.ExecuteAsync(
                        "sp_DeleteTaggingItem",
                        parameters,
                        commandType: CommandType.StoredProcedure
                    );
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in DeleteAsync: {ex.Message}");
                throw;
            }
        }

        public async Task<bool> ExistsByInvoiceNumberAsync(string invoiceNumber)
        {
            try
            {
                using (var connection = new MySqlConnection(_connectionString[0]))
                {
                    await connection.OpenAsync();
                    var parameters = new { invoiceNumber };
                    var count = await connection.ExecuteScalarAsync<int>(
                        "sp_CheckInvoiceNumberExists",
                        parameters,
                        commandType: CommandType.StoredProcedure
                    );
                    return count > 0;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in ExistsByInvoiceNumberAsync: {ex.Message}");
                throw;
            }
        }

        public async Task<TaggingItem> GetByInvoiceNumberAsync(string invoiceNumber)
        {
            try
            {
                using (var connection = new MySqlConnection(_connectionString[0]))
                {
                    await connection.OpenAsync();
                    var parameters = new { invoiceNumber };
                    return await connection.QueryFirstOrDefaultAsync<TaggingItem>(
                        "sp_GetTaggingItemByInvoiceNumber",
                        parameters,
                        commandType: CommandType.StoredProcedure
                    );
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in GetByInvoiceNumberAsync: {ex.Message}");
                throw;
            }
        }
    }
}
