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
using Terret_Billing.Presentation.Models;

namespace Terret_Billing.Infrastructure.Data.Repositories
{
    public class DiamondRepository : IDiamondRepository
    {
        private const int REMOTE_DB_INDEX = 1;
        private const int LOCAL_DB_INDEX = 0;
        private readonly List<string> _connectionStrings;
        private DatabaseHelper databaseHelper;

        public DiamondRepository(IDatabaseHelper databaseHelper)
        {
            _connectionStrings = databaseHelper?.GetConnectionString();
        }

        public DiamondRepository(DatabaseHelper databaseHelper)
        {
            this.databaseHelper = databaseHelper;
        }

        private string RemoteConnectionString => _connectionStrings[REMOTE_DB_INDEX];
        private string LocalConnectionString => _connectionStrings[LOCAL_DB_INDEX];

        public async Task AddAsync(DiamondTaggingEntry entry)
        {
            long localId = await PostAddAsyncToLocal(entry, _connectionStrings);
            entry.Local_Id = localId;
            await PostAddAsyncToServer(entry, _connectionStrings);
        }

        public async Task<long> PostAddAsyncToLocal(DiamondTaggingEntry entry, List<string> _connectionString)
        {
            using (var connection = new MySqlConnection(_connectionString[LOCAL_DB_INDEX]))
            {
                await connection.OpenAsync();

                var parameters = new DynamicParameters(entry);
                parameters.Add("Diamond_id", dbType: DbType.Int64, direction: ParameterDirection.Output);
                parameters.Add("Barcode", dbType: DbType.String, direction: ParameterDirection.Output);

                await connection.ExecuteAsync(
                    "sp_InsertIntoDiamondTaggingEntry_local",
                    parameters,
                    commandType: CommandType.StoredProcedure
                ).ConfigureAwait(false);
                long? insertedId = parameters.Get<long?>("Diamond_id");
                entry.Barcode = parameters.Get<string>("Barcode");
                return insertedId.Value;
            }
        }

        public async Task PostAddAsyncToServer(DiamondTaggingEntry entry, List<string> _connectionString)
        {
            try
            {
                using (var connection = new MySqlConnection(_connectionString[REMOTE_DB_INDEX]))
                {
                    await connection.OpenAsync();

                    await connection.ExecuteAsync(
                        "sp_InsertIntoDiamondTaggingEntry_server",
                        entry,
                        commandType: CommandType.StoredProcedure
                    ).ConfigureAwait(false);

                    await UpdateLocalEntryAfterServerPost(entry, _connectionString);
                }
            }
            catch (Exception)
            {
               
                throw;
            }
        }

        private async Task UpdateLocalEntryAfterServerPost(DiamondTaggingEntry entry, List<string> _connectionString)
        {
            using (var connection = new MySqlConnection(_connectionString[LOCAL_DB_INDEX]))
            {
                await connection.OpenAsync();

                 await connection.ExecuteAsync(
                "sp_UpdateDiamondTaggingEntryPostStatus_local",
                new { Dia_E_D_id = entry.Local_Id },
                commandType: CommandType.StoredProcedure
            );
            }
        }

       

        public async Task<DiamondTaggingEntry> GetByIdAsync(long id)
        {
            using (var connection = new MySqlConnection(LocalConnectionString))
            {
                await connection.OpenAsync();
                var parameters = new { p_Id = id };
                return await connection.QueryFirstOrDefaultAsync<DiamondTaggingEntry>(
                    "GetDiamondById",
                    parameters,
                    commandType: CommandType.StoredProcedure
                );
            }
        }

        public async Task<IEnumerable<DiamondTaggingEntry>> GetAllAsync()
        {
            using (var connection = new MySqlConnection(LocalConnectionString))
            {
                await connection.OpenAsync();
                return await connection.QueryAsync<DiamondTaggingEntry>(
                    "GetAllDiamonds",
                    commandType: CommandType.StoredProcedure
                );
            }
        }

        public async Task UpdateAsync(DiamondTaggingEntry entry)
        {
            using (var connection = new MySqlConnection(LocalConnectionString))
            {
                await connection.OpenAsync();

                var parameters = new DynamicParameters();
                parameters.Add("p_UpdatedAt", DateTime.UtcNow);

                // Map all DiamondTaggingEntry properties to parameters dynamically
                var properties = typeof(DiamondTaggingEntry).GetProperties(BindingFlags.Public | BindingFlags.Instance)
                    .Where(p => p.CanRead && p.CanWrite);

                foreach (var prop in properties)
                {
                    var paramName = "p_" + prop.Name;
                    var value = prop.GetValue(entry) ?? DBNull.Value;
                    parameters.Add(paramName, value);
                }

                await connection.ExecuteAsync(
                    "UpdateDiamond",
                    parameters,
                    commandType: CommandType.StoredProcedure
                );
            }
        }

        public async Task DeleteAsync(long id)
        {
            using (var connection = new MySqlConnection(RemoteConnectionString))
            {
                await connection.OpenAsync();
                var parameters = new { p_Id = id };
                await connection.ExecuteAsync(
                    "DeleteDiamond",
                    parameters,
                    commandType: CommandType.StoredProcedure
                );
            }
        }

        public async Task<bool> ExistsByInvoiceNoAsync(string invoiceNo)
        {
            using (var connection = new MySqlConnection(LocalConnectionString))
            {
                await connection.OpenAsync();
                var parameters = new { p_InvoiceNo = invoiceNo };
                var count = await connection.ExecuteScalarAsync<int>(
                    "SELECT COUNT(*) FROM Diamond_tagging_entry WHERE Dia_E_Invoice_No = @p_InvoiceNo",
                    parameters
                );
                return count > 0;
            }
        }

        public async Task<DiamondTaggingEntry> GetByInvoiceNoAsync(string invoiceNo)
        {
            using (var connection = new MySqlConnection(LocalConnectionString))
            {
                await connection.OpenAsync();
                var parameters = new { p_InvoiceNo = invoiceNo };
                return await connection.QueryFirstOrDefaultAsync<DiamondTaggingEntry>(
                    "GetDiamondByInvoiceNo",
                    parameters,
                    commandType: CommandType.StoredProcedure
                );
            }
        }
    }
}
