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
    public class PartyRepository : IPartyRepository
    {
        private readonly List<string> _connectionString;
        private DatabaseHelper databaseHelper;
        private readonly IPartyRepository _partyRepository;
        public PartyRepository(IDatabaseHelper databaseHelper)
        {
            _connectionString = databaseHelper.GetConnectionString();
        }


        public PartyRepository(DatabaseHelper databaseHelper)
        {
            this.databaseHelper = databaseHelper;
        }

        public async Task AddAsync(Customer customer)
        {
            await PostAddAsyncToLocal(customer, _connectionString);
            await PostAddAsyncToServer(customer, _connectionString);           

        }
              
        public async Task PostAddAsyncToLocal(Customer customer, List<string> _connectionString)
        {
            try
            {
                using (var connection = new MySqlConnection(_connectionString[0]))
                {
                    await connection.OpenAsync();

                    var parameters = new DynamicParameters(customer); // assuming props match

                    // Add OUT parameters
                    parameters.Add("NewPartyId", dbType: DbType.Int64, direction: ParameterDirection.Output);
                    parameters.Add("OutFirmId", dbType: DbType.String, size: 50, direction: ParameterDirection.Output);

                    // Execute procedure
                    await connection.ExecuteAsync(
                        "sp_AddParty_local",
                        parameters,
                        commandType: CommandType.StoredProcedure
                    );

                    // Retrieve output values
                    customer.LocalId = parameters.Get<long>("NewPartyId");
                    customer.FirmId = parameters.Get<string>("OutFirmId");
                }
            }
            catch (Exception ex)
            {
                // log or handle exception
                Console.WriteLine("Error: " + ex.Message);
            }

            
        }

        public async Task PostAddAsyncToServer(Customer customer, List<string> _connectionString)
        {
            try
            {
                using (var connection = new MySqlConnection(_connectionString[1]))
                {
                    await connection.OpenAsync();

                    await connection.ExecuteAsync(
                       "sp_AddParty_server",
                       customer,
                       commandType: CommandType.StoredProcedure
                   );
                    using (var localConn = new MySqlConnection(_connectionString[0]))
                    {
                        await localConn.ExecuteAsync("sp_MarkPartyAsSynced", new { p_LocalId = customer.LocalId }, commandType: CommandType.StoredProcedure);
                    }
                }
            }
            catch (Exception ex)
            {

               
            }
        }

      

        public async Task<Customer> GetByIdAsync(int id)
        {
            using (var connection = new MySqlConnection(_connectionString[1]))
            {
                await connection.OpenAsync();
                var parameters = new { p_Id = id };
                return await connection.QueryFirstOrDefaultAsync<Customer>(
                    "GetPartyById",
                    parameters,
                    commandType: CommandType.StoredProcedure
                );
            }
        }

        public async Task<IEnumerable<Party>> GetAllAsync()
        {
            using (var connection = new MySqlConnection(_connectionString[0]))
            {
                await connection.OpenAsync();
                return await connection.QueryAsync<Party>(
                    "sp_GetAllParties",
                    commandType: CommandType.StoredProcedure
                );
            }
        }

        public async Task UpdateAsync(Party party)
        {
            using (var connection = new MySqlConnection(_connectionString[0]))
            {
                await connection.OpenAsync();

                var parameters = new DynamicParameters();
                parameters.Add("p_UpdatedAt", DateTime.UtcNow);

                // Map all Party properties to parameters dynamically
                var properties = typeof(Party).GetProperties(BindingFlags.Public | BindingFlags.Instance)
                    .Where(p => p.CanRead && p.CanWrite);

                foreach (var prop in properties)
                {
                    var paramName = "p_" + prop.Name;
                    var value = prop.GetValue(party) ?? DBNull.Value;
                    parameters.Add(paramName, value);
                }

                await connection.ExecuteAsync(
                    "UpdateParty",
                    parameters,
                    commandType: CommandType.StoredProcedure
                );
            }
        }

        public async Task DeleteAsync(int id)
        {
            using (var connection = new MySqlConnection(_connectionString[0]))
            {
                await connection.OpenAsync();
                var parameters = new { p_Id = id };
                await connection.ExecuteAsync(
                    "DeleteParty",
                    parameters,
                    commandType: CommandType.StoredProcedure
                );
            }
        }

        public async Task<bool> ExistsByGSTNumberAsync(string gstNumber)
        {
            using (var connection = new MySqlConnection(_connectionString[0]))
            {
                await connection.OpenAsync();
                var parameters = new { p_GSTNumber = gstNumber };
                var count = await connection.ExecuteScalarAsync<int>(
                     "SELECT COUNT(*) FROM Party WHERE GSTNumber = @p_GSTNumber  AND IsActive = 1",
                    parameters
                );
                return count > 0;
            }
        }

        public async Task<Party> GetByGSTNumberAsync(string gstNumber)
        {
            using (var connection = new MySqlConnection(_connectionString[0]))
            {
                await connection.OpenAsync();
                var parameters = new { p_GSTNumber = gstNumber };
                return await connection.QueryFirstOrDefaultAsync<Party>(
                    "GetPartyByGSTNumber",
                    parameters,
                    commandType: CommandType.StoredProcedure
                );
            }
        }


        public async Task<IEnumerable<Party>> GetBySearchTextAsync(string SerchText)
        {
            using (var connection = new MySqlConnection(_connectionString[0]))
            {
                await connection.OpenAsync();
                var parameters = new { p_SearchText = SerchText };
                return await connection.QueryAsync<Party>(
                    "sp_SearchPartyByText",
                    parameters,
                    commandType: CommandType.StoredProcedure
                );
            }
        }

        public async Task VerifyCreatePartyAsync(Customer customer)
        {

            if (await IsGSTNumberDuplicateAsyncverify(customer.GSTNumber))
                throw new ArgumentException("GST number already exists.", nameof(customer.GSTNumber));

            await _partyRepository.AddAsync(customer);
        }

        public async Task<bool> IsGSTNumberDuplicateAsyncverify(string gstNumber)
        {
            if (string.IsNullOrWhiteSpace(gstNumber))
                return false;

            return await _partyRepository.ExistsByGSTNumberAsync(gstNumber);
        }
    }
}

