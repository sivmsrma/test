using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using MySql.Data.MySqlClient;
using Terret_Billing.Domain.Entities;
using Terret_Billing.Domain.Interfaces;
using Terret_Billing.Infrastructure.Data;

namespace Terret_Billing.Infrastructure.Data.Repositories
{
    public class ItemRepository : IItemRepository
    {
        private readonly IDatabaseHelper _databaseHelper;
        private readonly List<string> _connectionStrings;

        public ItemRepository(IDatabaseHelper databaseHelper)
        {
            _databaseHelper = databaseHelper ?? throw new ArgumentNullException(nameof(databaseHelper));
            _connectionStrings = databaseHelper.GetConnectionString();
            if (_connectionStrings == null || _connectionStrings.Count == 0)
            {
                throw new InvalidOperationException("No connection strings available");
            }
        }

        private async Task ExecuteWithLocalConnectionAsync(Func<IDbConnection, Task> action)
        {
            using (var connection = new MySqlConnection(_connectionStrings[0]))
            {
                await connection.OpenAsync();
                await action(connection);
            }
        }

        private async Task<T> ExecuteWithLocalConnectionAsync<T>(Func<IDbConnection, Task<T>> action)
        {
            using (var connection = new MySqlConnection(_connectionStrings[0]))
            {
                await connection.OpenAsync();
                return await action(connection);
            }
        }

        private async Task ExecuteWithServerConnectionAsync(Func<IDbConnection, Task> action)
        {
            if (_connectionStrings.Count < 2) return;
            
            using (var connection = new MySqlConnection(_connectionStrings[1]))
            {
                await connection.OpenAsync();
                await action(connection);
            }
        }

        private async Task<T> ExecuteWithServerConnectionAsync<T>(Func<IDbConnection, Task<T>> action)
        {
            if (_connectionStrings.Count < 2) return default;
            
            using (var connection = new MySqlConnection(_connectionStrings[1]))
            {
                await connection.OpenAsync();
                return await action(connection);
            }
        }

        public async Task<IEnumerable<Item>> GetAllItemsAsync()
        {
            return await ExecuteWithLocalConnectionAsync(async connection =>
                await connection.QueryAsync<Item>("sp_GetAllItems",
                    commandType: CommandType.StoredProcedure));
        }

        public async Task<IEnumerable<Item>> GetAllItemsFromServerAsync()
        {
            return await ExecuteWithServerConnectionAsync(async connection =>
                await connection.QueryAsync<Item>("sp_GetAllItems",
                    commandType: CommandType.StoredProcedure)) ?? new List<Item>();
        }

        public async Task<Item> GetItemByIdAsync(int id)
        {
            using var connection = new MySqlConnection(_connectionStrings[0]);
            return await connection.QueryFirstOrDefaultAsync<Item>("sp_GetItemById",
                new { p_item_id = id },
                commandType: CommandType.StoredProcedure);
        }

        public async Task<long> AddItemAsync(Item item)
        {
            using var connection = new MySqlConnection(_connectionStrings[0]);

            var parameters = new DynamicParameters(item);
            parameters.Add("ItemId", dbType: DbType.Int64, direction: ParameterDirection.Output);

            await connection.ExecuteAsync("sp_InsertItemInfo", parameters, commandType: CommandType.StoredProcedure);

            long itemId = parameters.Get<long>("ItemId");
            item.ItemId = itemId;

            return itemId;
        }


        public async Task<bool> UpdateItemAsync(Item item)
        {
            var connection = new MySqlConnection(_connectionStrings[0]);
            var parameters = new DynamicParameters();

            parameters.Add("p_item_id", item.ItemId, DbType.Int32);
            parameters.Add("p_metal_type", item.MetalType, DbType.String);
            parameters.Add("p_category", item.Category, DbType.String);
            parameters.Add("p_sub_category", item.SubCategory, DbType.String);
            parameters.Add("p_design", item.Design, DbType.String);
            parameters.Add("p_hsn_id", item.HsnId, DbType.Int32);
            parameters.Add("p_hsn_code", item.HsnCode, DbType.String);
            parameters.Add("p_short_name", item.ShortName, DbType.String);
            parameters.Add("p_barcode", item.Barcode, DbType.String);
            
            parameters.Add("p_modified_date", DateTime.UtcNow, DbType.DateTime);

            var rowsAffected = await connection.ExecuteAsync("sp_UpdateItem",
                parameters,
                commandType: CommandType.StoredProcedure);

            return rowsAffected > 0;
        }

        public async Task<bool> DeleteItemAsync(int id)
        {

            var connection = new MySqlConnection(_connectionStrings[0]); 
            var rowsAffected = await connection.ExecuteAsync("sp_DeleteItem",
                new { p_item_id = id, p_modified_date = DateTime.UtcNow },
                commandType: CommandType.StoredProcedure);

            return rowsAffected > 0;
        }

        public async Task<bool> ItemExistsAsync(long id)
        {
            var connection = new MySqlConnection(_connectionStrings[0]);
            return await connection.ExecuteScalarAsync<bool>("sp_ItemExists",
                new { p_item_id = id },
                commandType: CommandType.StoredProcedure);
        }

        public async Task<IEnumerable<HsnItem>> GetHsnItemsAsync()
        {

            var connection = new MySqlConnection(_connectionStrings[0]);
            return await connection.QueryAsync<HsnItem>("sp_GetHsnItems",
                commandType: CommandType.StoredProcedure);
        }


        public async Task<string> GetHSNCodeByItemIdAsync(int itemId)
        {

            var connection = new MySqlConnection(_connectionStrings[0]);
            return await connection.QueryFirstOrDefaultAsync<string>("sp_GetHSNCodeByItemId",
                new { p_item_id = itemId },
                commandType: CommandType.StoredProcedure);
        }



        public async Task<IEnumerable<HsnItem>> Get4DigitHsnCodesAsync()
        {

            var connection = new MySqlConnection(_connectionStrings[0]);
            return await connection.QueryAsync<HsnItem>(
                "GetAll4DigitPrefixes",
                commandType: CommandType.StoredProcedure);
        }

        public async Task<IEnumerable<HsnItem>> Get6DigitHsnCodesAsync()
        {
            var connection = new MySqlConnection(_connectionStrings[0]);
            return await connection.QueryAsync<HsnItem>(
                "GetAll6DigitPrefixes",
                commandType: CommandType.StoredProcedure);
        }

        public async Task<IEnumerable<HsnItem>> Get8DigitHsnCodesAsync()
        {
            var connection = new MySqlConnection(_connectionStrings[0]);
            return await connection.QueryAsync<HsnItem>(
                "GetAll8DigitHSNCodes",
                commandType: CommandType.StoredProcedure);
        }

        public async Task<IEnumerable<string>> GetCategoriesByMetalTypeAsync(string metalType, string firmId )
        {
            var connection = new MySqlConnection(_connectionStrings[0]);
            return await connection.QueryAsync<string>(
                "sp_GetCategoriesByMetalTypeAndFirm",
                new { p_metal_type = metalType, p_firm_id = firmId },
                commandType: CommandType.StoredProcedure);
        }

        public async Task<IEnumerable<string>> GetSubCategoriesAsync(string metalType, string category, string firmId )
        {
            var connection = new MySqlConnection(_connectionStrings[0]);
            return await connection.QueryAsync<string>(
                "sp_GetSubCategoriesByMetalTypeAndCategoryAndFirm",
                new { p_metal_type = metalType, p_category = category, p_firm_id = firmId },
                commandType: CommandType.StoredProcedure);
        }

        public async Task<IEnumerable<string>> GetDesignsAsync(string metalType, string category, string subCategory, string firmId)
        {
            var connection = new MySqlConnection(_connectionStrings[0]);
            return await connection.QueryAsync<string>(
                "sp_GetDesignsByMetalTypeAndCategoryAndSubCategoryAndFirm",
                new { p_metal_type = metalType, p_category = category, p_sub_category = subCategory, p_firm_id = firmId },
                commandType: CommandType.StoredProcedure);
        }

        public async Task<string> GetHSNCodeAsync(string metalType, string category, string subCategory, string design)
        {
            var connection = new MySqlConnection(_connectionStrings[0]);
            return await connection.QueryFirstOrDefaultAsync<string>(
                "sp_GetHSNCode",
                new { p_metal_type = metalType, p_category = category, p_sub_category = subCategory, p_design = design },
                commandType: CommandType.StoredProcedure);
        }
        public async Task<string> GetShortNameAsync(string metalType, string category, string subCategory, string design)
        {
            try
            {   using var connection = new MySqlConnection(_connectionStrings[0]);
                await connection.OpenAsync();

                var parameters = new DynamicParameters();
                parameters.Add("p_metal_type", metalType?.Trim());
                parameters.Add("p_category", category?.Trim());
                parameters.Add("p_sub_category", subCategory?.Trim());
                parameters.Add("p_design", design?.Trim());



                var result = await connection.QueryAsync<string>(
                    "sp_GetShortName",
                    parameters,
                    commandType: CommandType.StoredProcedure);

                var shortName = result.FirstOrDefault();
                Console.WriteLine($"Stored procedure returned: {shortName ?? "NULL"}");

                return shortName ?? "UNKNOWN";
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in GetShortNameAsync: {ex.Message}\n{ex.StackTrace}");
                throw;
            }
        }





        public async Task<int> GetCompanyIdByFirmIdAsync(string firmId)
        {
            var connection = new MySqlConnection(_connectionStrings[0]);
            return await connection.QueryFirstOrDefaultAsync<int>(
                "sp_GetCompanyIdByFirmId",
                new { p_firm_id = firmId },
                commandType: CommandType.StoredProcedure);
        }

        public async Task<string> GetLastBarcodeByCompanyAsync(string shortName, string firmId)
        {
            var connection = new MySqlConnection(_connectionStrings[0]);
            return await connection.QueryFirstOrDefaultAsync<string>(
                "sp_GetLastBarcodeByCompany",
                new { p_short_name = shortName, p_firm_id = firmId },
                commandType: CommandType.StoredProcedure);
        }

        public async Task<string> GetLastBarcodeByMetalTypeAsync(string metalType, string shortName)
        {
            var connection = new MySqlConnection(_connectionStrings[0]);
            return await connection.QueryFirstOrDefaultAsync<string>(
                "sp_GetLastBarcodeByMetalType",
                new { metal_type = metalType, short_name = shortName },
                commandType: CommandType.StoredProcedure);
        }

        public async Task<string> GetLastDiamondBarcodeAsync(string category, string subCategory, string design)
        {
            var connection = new MySqlConnection(_connectionStrings[0]);
            return await connection.QueryFirstOrDefaultAsync<string>(
                "sp_GetLastDiamondBarcode",
                new { p_category = category, p_sub_category = subCategory, p_design = design },
                commandType: CommandType.StoredProcedure);
        }

        public async Task<int?> GetItemIdAsync(string metalType, string category, string subCategory, string design)
        {
            var connection = new MySqlConnection(_connectionStrings[0]);
            return await connection.QueryFirstOrDefaultAsync<int?>(
                "sp_GetItemIdByDetails",
                new { p_metal_type = metalType, p_category = category, p_sub_category = subCategory, p_design = design },
                commandType: CommandType.StoredProcedure);
        }

        public async Task<int?> GetItemIdAsync(string metalType, string category, string subCategory, string design, string firmId)
        {
            var connection = new MySqlConnection(_connectionStrings[0]);
            return await connection.QueryFirstOrDefaultAsync<int?>(
                "sp_GetItemIdByDetailsWithFirm",
                new { p_metal_type = metalType, p_category = category, p_sub_category = subCategory, p_design = design, p_firm_id = firmId },
                commandType: CommandType.StoredProcedure);
        }

        public async Task<IEnumerable<string>> GetPuritiesByMetalTypeAsync(string metalType)
        {
            var connection = new MySqlConnection(_connectionStrings[0]);
            return await connection.QueryAsync<string>(
                "sp_GetPuritiesByMetalType",
                new { p_metal_type = metalType },
                commandType: CommandType.StoredProcedure);
        }

        public async Task<bool> InsertGoldAndSilverTaggingEntryAsync(GoldAndSilverTaggingEntry entry)
        {
            var connection = new MySqlConnection(_connectionStrings[0]);
            var parameters = new DynamicParameters(entry);
            parameters.Add("p_id", dbType: DbType.Int32, direction: ParameterDirection.Output);
            
            await connection.ExecuteAsync(
                "sp_InsertGoldAndSilverTaggingEntry",
                parameters,
                commandType: CommandType.StoredProcedure);
                
            return parameters.Get<int>("p_id") > 0;
        }
    }
}