using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using MySql.Data.MySqlClient;
using Terret_Billing.Domain.Entities;
using Terret_Billing.Domain.Interfaces;

namespace Terret_Billing.Infrastructure.Data.Repositories
{
    public class GoldAndSilverRepository : IGoldAndSilverRepository
    {
        private const int REMOTE_DB_INDEX = 1; // for remote
        private const int LOCAL_DB_INDEX = 0;  // for local
        private readonly List<string> _connectionStrings;
        private DatabaseHelper databaseHelper;

        public GoldAndSilverRepository(IDatabaseHelper databaseHelper)
        {
            _connectionStrings = databaseHelper?.GetConnectionString();
        }

        public GoldAndSilverRepository(DatabaseHelper databaseHelper)
        {
            this.databaseHelper = databaseHelper;
        }


        public async Task AddAsync(GoldAndSilverTaggingEntry entry)
        {
            long localId = await PostAddAsyncToLocal(entry, _connectionStrings);
            entry.Local_Id = localId;

            await PostAddAsyncToServer(entry, _connectionStrings);
        }

        private string RemoteConnectionString => _connectionStrings[REMOTE_DB_INDEX];
        private string LocalConnectionString => _connectionStrings[LOCAL_DB_INDEX];

        public async Task<long> PostAddAsyncToLocal(GoldAndSilverTaggingEntry entry, List<string> _connectionString)
        {
            using (var connection = new MySqlConnection(_connectionString[LOCAL_DB_INDEX]))
            {
                await connection.OpenAsync();

                var parameters = new DynamicParameters(entry);
                parameters.Add("GoldSilver_id", dbType: DbType.Int64, direction: ParameterDirection.Output);
                parameters.Add("Barcode", dbType: DbType.String, direction: ParameterDirection.Output);

                await connection.ExecuteAsync(
                    "sp_InsertIntoGoldAndSilverTaggingEntry_local",
                    parameters,
                    commandType: CommandType.StoredProcedure
                ).ConfigureAwait(false);
                // Use nullable type to avoid DBNull cast error
                long? insertedId = parameters.Get<long?>("GoldSilver_id");
                entry.Barcode = parameters.Get<string>("Barcode");

                // Handle case where ID was not returned
                if (insertedId == null || insertedId == 0)
                {
                    throw new ApplicationException("Insert failed: GoldSilver_id was not returned.");
                }

                return insertedId.Value;
            }
        }

        public async Task PostAddAsyncToServer(GoldAndSilverTaggingEntry entry, List<string> _connectionString)
        {
            try
            {
                using (var connection = new MySqlConnection(_connectionString[REMOTE_DB_INDEX]))
                {
                    await connection.OpenAsync();

                    // Server insert
                    await connection.ExecuteAsync(
                        "sp_InsertIntoGoldAndSilverTaggingEntry_server",
                        entry,
                        commandType: CommandType.StoredProcedure
                    ).ConfigureAwait(false);
                }

                using (var localConnection = new MySqlConnection(_connectionString[LOCAL_DB_INDEX]))
                {
                    await localConnection.OpenAsync();

                    var updateParams = new
                    {
                        local_id = entry.Local_Id,
                        firm_id = entry.Firm_Id,
                    };

                    await localConnection.ExecuteAsync(
                        "sp_UpdateIsDataPostOnServer",
                        updateParams,
                        commandType: CommandType.StoredProcedure
                    );
                }
            }
            catch (Exception ex)
            {

            }
        }

        public async Task<List<GoldAndSilverTaggingEntry>> GetAllAsync(User user)
        {
            using (var connection = new MySqlConnection(RemoteConnectionString))
            {
                await connection.OpenAsync();

                var parameters = new { p_firm_id = user.firm_id };

                var results = await connection.QueryAsync<GoldAndSilverTaggingEntry>(
                    "sp_GetGoldAndSilverByInvoice",
                    parameters,
                    commandType: CommandType.StoredProcedure
                );

                return results.ToList();
            }
        }

        public async Task<(IEnumerable<GoldAndSilverTaggingEntry> Items, int TotalCount)> GetByTypePagedAsync(string type, string firmId, string invoiceNumber, int pageNumber, int pageSize)
        {
            using (var connection = new MySqlConnection(LocalConnectionString))
            {
                await connection.OpenAsync();

                var parameters = new
                {
                    firmId = firmId,
                    invoiceNo = string.IsNullOrWhiteSpace(invoiceNumber) ? "" : invoiceNumber.Trim(),
                    metalType = type,
                    pageNumber,
                    pageSize
                };

                using (var multi = await connection.QueryMultipleAsync(
                    "PopulateGoldAndSilverTaggingGrid",
                    parameters,
                    commandType: CommandType.StoredProcedure))
                {
                    var totalCountObj = multi.ReadFirst<dynamic>();
                    int totalCount = Convert.ToInt32(totalCountObj.TotalCount);
                    var items = multi.Read<GoldAndSilverTaggingEntry>().ToList();
                    return (items, totalCount);
                }
            }
        }

        public async Task<decimal?> GetPendingWeightByFiltersAsync(
            string metalType, string partyName, string invoiceNumber, string purity)
        {
            using (var connection = new MySqlConnection(_connectionStrings[0]))
            {
                await connection.OpenAsync();

                var parameters = new
                {
                    in_metal_type = metalType,
                    in_party_name = partyName,
                    in_invoice_number = invoiceNumber,
                    in_purity = purity
                };

                var result = await connection.QueryFirstOrDefaultAsync<dynamic>(
                    "sp_GetPendingWeightByFilters",
                    parameters,
                    commandType: CommandType.StoredProcedure
                );

                if (result != null && result.pending_weight != null)
                    return (decimal?)result.pending_weight;

                return null;
            }
        }


    }
}
