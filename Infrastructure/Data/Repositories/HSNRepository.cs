using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using Dapper;
using MySql.Data.MySqlClient;
using Terret_Billing.Domain.Entities;
using Terret_Billing.Domain.Interfaces;

namespace Terret_Billing.Infrastructure.Data.Repositories
{
    public class HSNRepository : IHSNRepository
    {
        private const int LOCAL_DB_INDEX = 0;
        private const int REMOTE_DB_INDEX = 1;
        private readonly List<string> _connectionStrings;

        public HSNRepository(IDatabaseHelper databaseHelper)
        {
            _connectionStrings = databaseHelper?.GetConnectionString();
        }

        private MySqlConnection CreateConnection(bool useLocal)
        {
            var connStr = useLocal ? _connectionStrings[LOCAL_DB_INDEX] : _connectionStrings[REMOTE_DB_INDEX];
            return new MySqlConnection(connStr);
        }

        public async Task<IEnumerable<HSNListItem>> GetHSNListByFirmIdAndLengthAsync(string firmId, int codeLength, bool useLocal)
        {
            using var connection = CreateConnection(useLocal);
            return await connection.QueryAsync<HSNListItem>(
                "GetHSNListByFirmIdAndLength",
                new { firmId = firmId, length = codeLength },
                commandType: CommandType.StoredProcedure);
        }

        public async Task InsertHSNListToLocalAsync(IEnumerable<HSNListItem> items)
        {
            using var connection = CreateConnection(true); // always local
            foreach (var item in items)
            {
                await connection.ExecuteAsync(
                    "INSERT INTO hsn_list (firm_id, metal, hsn_code, IsDataPostOnServer) VALUES (@firm_id, @metal, @hsn_code, @IsDataPostOnServer)",
                    new
                    {
                        firm_id = item.firm_id,
                        metal = item.metal,
                        hsn_code = item.hsn_code,
                        IsDataPostOnServer = item.IsDataPostOnServer
                    });
            }
        }
    }
}
