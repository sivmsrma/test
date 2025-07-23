using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using Terret_Billing.Domain.Entities;
using Terret_Billing.Domain.Interfaces;
using Terret_Billing.Application.Services.Interfaces;

namespace Terret_Billing.Infrastructure.Data.Repositories
{
    public class ItemInfoRepository : IItemInfoRepository
    {
        private const int LOCAL_DB_INDEX = 0;
        private const int REMOTE_DB_INDEX = 1;
        private readonly List<string> _connectionStrings;

        public ItemInfoRepository(IDatabaseHelper databaseHelper)
        {
            _connectionStrings = databaseHelper?.GetConnectionString();
        }

        private MySqlConnection CreateConnection(bool useLocal)
        {
            var connStr = useLocal ? _connectionStrings[LOCAL_DB_INDEX] : _connectionStrings[REMOTE_DB_INDEX];
            return new MySqlConnection(connStr);
        }

        public async Task InsertItemAsync(ItemInfo item, bool isServer = false)
        {
            using var conn = CreateConnection(!isServer); // false = server → !false = local
            string sql = @"
                INSERT INTO item_info 
                (metal_type, category, sub_category, design, hsn_id, hsn_code, short_name, firmid, IsDataPostOnServer)
                VALUES (@metal, @category, @sub_category, @design, @hsn_id, @hsn_code, @short_name, @firmid, @IsDataPostOnServer);
                SELECT LAST_INSERT_ID();";
                
            // Execute the query and get the inserted ID
            var insertedId = await conn.ExecuteScalarAsync<long>(sql, new 
            {
                metal = item.metal,
                category = item.category,
                sub_category = item.sub_category,
                design = item.design,
                hsn_id = item.hsn_id,
                hsn_code = item.hsn_code,
                short_name = item.short_name,
                firmid = item.firm_id,  // Changed from firm_id to firmid to match the database column
                IsDataPostOnServer = item.IsDataPostOnServer
            });
            
            item.item_id = insertedId; // Update the item with the new ID
        }


        public async Task<List<ItemInfo>> GetItemInfoByFirmIdAsync(string firmId, bool useLocal = true)
        {
            using var conn = CreateConnection(useLocal);
            var result = await conn.QueryAsync<ItemInfo>(
                "GetItemInfoByFirmId",
                new { firmid = firmId },
                commandType: CommandType.StoredProcedure);
            return result.ToList();
        }

        public async Task<List<ItemInfo>> GetItemInfoFromServerByFirmIdAsync(string firmId)
        {
            using var conn = CreateConnection(false); // use remote/server
            var result = await conn.QueryAsync<ItemInfo>(
                "GetItemInfoByFirmId",
                new { firmid = firmId },
                commandType: CommandType.StoredProcedure);
            return result.ToList();
        }

        //public async Task<bool> CategoryExistsAsync(string metal, string category, string firmId)
        //{
        //    using var conn = CreateConnection(true); // always local
        //    var result = await conn.QueryFirstOrDefaultAsync<int>(
        //        "SELECT COUNT(*) FROM item_info WHERE metal = @metal AND category = @category AND firmid = @firmId",
        //        new { metal, category, firmId });
        //    return result > 0;
        //}

        public async Task UpdateItemAsync(ItemInfo item)
        {
            using var conn = CreateConnection(true); // always local
            string sql = "CALL UpdateItemInfo(@metal, @category, @sub_category, @design, @hsn_id, @hsn_code, @short_name, @firm_id, @IsDataPostOnServer)";
            await conn.ExecuteAsync(sql, item);
        }

        //public async Task SyncLocalToServerAsync(string firmId)
        //{
        //    using var localConn = CreateConnection(true);
        //    var unsynced = (await localConn.QueryAsync<ItemInfo>(
        //        "SELECT * FROM item_info WHERE firm_id = @firmId AND IsDataPostOnServer = 0", new { firmId })).ToList();

        //    if (!unsynced.Any()) return;

        //    using var serverConn = CreateConnection(false);
        //    foreach (var item in unsynced)
        //    {
        //        await serverConn.ExecuteAsync(
        //            "INSERT INTO item_info (metal, category, sub_category, design, hsn_id, hsn_code, short_name, firm_id, IsDataPostOnServer) " +
        //            "VALUES (@metal, @category, @sub_category, @design, @hsn_id, @hsn_code, @short_name, @firm_id, 1)",
        //            item);

        //        await localConn.ExecuteAsync(
        //            "UPDATE item_info SET IsDataPostOnServer = 1 WHERE item_id = @item_id",
        //            new { item.item_id });
        //    }
        //}
    }
}
