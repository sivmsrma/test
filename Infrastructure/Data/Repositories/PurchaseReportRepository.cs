using System;
using Dapper;
using Terret_Billing.Domain.Entities;
using Terret_Billing.Domain.Interfaces;
using Microsoft.Extensions.Configuration;
using MySql.Data.MySqlClient;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using Terret_Billing.Infrastructure.Data;

namespace Terret_Billing.Infrastructure.Repositories
{
    public class PurchaseReportRepository : IPurchaseReportRepository
    {
        private readonly List<string> _connectionStrings;
        private readonly IDatabaseHelper _databaseHelper;

        public PurchaseReportRepository(IDatabaseHelper databaseHelper)
        {
            _databaseHelper = databaseHelper;
            _connectionStrings = databaseHelper.GetConnectionString();
        }

        private MySqlConnection CreateLocalConnection() => new MySqlConnection(_connectionStrings[0]);

        private MySqlConnection CreateServerConnection() => new MySqlConnection(_connectionStrings.Count > 1 ? _connectionStrings[1] : _connectionStrings[0]);

        public async Task<IEnumerable<PurchaseViewReport>> GetAllPurchaseReportLocalAsync(string firmId)
        {
            using var connection = CreateLocalConnection();
            return await connection.QueryAsync<PurchaseViewReport>(
                "GetPurchaseItemDetailsForGridByFirm",
                new { firmId = firmId },
                commandType: CommandType.StoredProcedure);
        }

        public async Task<IEnumerable<string>> GetPartyNamesWithPurchasesAsync(string firmId)
        {
            using var connection = CreateLocalConnection();
            var result = await connection.QueryAsync<string>(
                "GetPartyNamesWithPurchases",
                new { inputFirmId = firmId },
                commandType: CommandType.StoredProcedure);
            return result;
        }

        public async Task<IEnumerable<string>> GetBillNoWithPurchasesAsync(string firmId)
        {
            using var connection = CreateLocalConnection();
            var result = await connection.QueryAsync<string>(
                "GetBillNosByFirmId",
                new { input_firm_id
                
                = firmId },
                commandType: CommandType.StoredProcedure);
            return result;
        }

        public async Task<IEnumerable<string>> GetMetalsWithPurchasesAsync(string firmId)
        {
            using var connection = CreateLocalConnection();
            var result = await connection.QueryAsync<string>(
                "GetMetalListByFirmId",
                new { input_firm_id = firmId },
                commandType: CommandType.StoredProcedure);
            return result;
        }

        public async Task<IEnumerable<string>> GetPurityTypesWithPurchasesAsync(string firmId)
        {
            using var connection = CreateLocalConnection();
            var result = await connection.QueryAsync<string>(
                "GetPurityListByFirmId",
                new { input_firm_id = firmId },
                commandType: CommandType.StoredProcedure);
            return result;
        }


    }
}

