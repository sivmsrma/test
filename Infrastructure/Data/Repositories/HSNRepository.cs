using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terret_Billing.Domain.Entities;
using Terret_Billing.Domain.Interfaces;

namespace Terret_Billing.Infrastructure.Data.Repositories
{
    public class HSNRepository : IHSNRepository
    {
        private readonly string _localConnectionString;
        private readonly string _serverConnectionString;

        public HSNRepository(IConfiguration configuration)
        {
            _localConnectionString = configuration.GetConnectionString("BillingDbLocal");
            _serverConnectionString = configuration.GetConnectionString("BillingDbServer");
        }

        private MySqlConnection CreateConnection(bool useLocal)
        {
            return useLocal ? new MySqlConnection(_localConnectionString) : new MySqlConnection(_serverConnectionString);
        }

        public async Task<IEnumerable<HSNListItem>> GetHSNListByFirmIdAndLengthAsync(string firmId, int codeLength, bool useLocal)
        {
            using var connection = CreateConnection(useLocal);
            return await connection.QueryAsync<HSNListItem>(
                "GetHSNListByFirmIdAndLength",
                new { firmid = firmId, length = codeLength },
                commandType: CommandType.StoredProcedure);
        }

        public async Task InsertHSNListToLocalAsync(IEnumerable<HSNListItem> items)
        {
            using var connection = CreateConnection(true);
            foreach (var item in items)
            {
                await connection.ExecuteAsync(
                    "INSERT INTO hsn_list (firm_id, metal, hsn_code, IsDataPostOnServer) VALUES (@firm_id, @metal, @hsn_code, @IsDataPostOnServer)",
                    new { firm_id = item.firm_id, metal = item.metal, hsn_code = item.hsn_code, IsDataPostOnServer = item.IsDataPostOnServer });
            }
        }
    }
}