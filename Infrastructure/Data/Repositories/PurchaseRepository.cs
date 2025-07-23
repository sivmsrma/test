using Dapper;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using Terret_Billing.Domain.Entities;
using Terret_Billing.Domain.Interfaces;
using Terret_Billing.Presentation.Models;
using Terret_Billing.Presentation.Models.Request;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace Terret_Billing.Infrastructure.Data.Repositories

{
        public class PurchaseRepository : IPurchaseRepository
        {
            private readonly List<string> _connectionString;
            private readonly User _currentUser;

        public PurchaseRepository(IDatabaseHelper databaseHelper)
            {
                _connectionString = databaseHelper.GetConnectionString();
            }
        public async Task AddAsync(PurchaseRequest purchaseRequest)
        {
            //purchaseRequest.Purchase.FirmId = _currentUser.firm_id;
            
            // 1. Save to local DB and get local purchase ID
            int localPurchaseId = await PostAddAsyncToLocal(purchaseRequest, _connectionString);

            // 2. Save local_id in Purchase object for server sync
            purchaseRequest.Purchase.LocalId = localPurchaseId;

            // 3. Save local_id for each item for server sync
            foreach (var item in purchaseRequest.PurchaseItems)
            {
                item.LocalId = item.Id; // Id set during local insert
            }

            // 4. Save to server
            await PostAddAsyncToServer(purchaseRequest, _connectionString);
            await updateonlocal(purchaseRequest, _connectionString);
            
        }
        public async Task<int> PostAddAsyncToLocal(PurchaseRequest purchaseRequest, List<string> _connectionString)
        {
            try
            {
                using (var connection = new MySqlConnection(_connectionString[0]))
                {
                    await connection.OpenAsync();

                    // Insert into Purchases
                    var parameters = new DynamicParameters(purchaseRequest.Purchase);
                    parameters.Add("PurchaseId", dbType: DbType.Int32, direction: ParameterDirection.Output);

                    await connection.ExecuteAsync("sp_InsertPurchase", parameters, commandType: CommandType.StoredProcedure);

                    int purchaseId = parameters.Get<int>("PurchaseId");
                    purchaseRequest.Purchase.Id = purchaseId; // Optional: if needed

                    // Insert into PurchaseItems
                    foreach (var item in purchaseRequest.PurchaseItems)
                    {
                        item.PurchaseId = purchaseId;
                        item.firm_id = purchaseRequest.Purchase.firm_id; // Ensure FirmId is set

                        var itemParams = new DynamicParameters(item);
                        itemParams.Add("Id", dbType: DbType.Int32, direction: ParameterDirection.Output);

                        await connection.ExecuteAsync("sp_InsertPurchaseItem", itemParams, commandType: CommandType.StoredProcedure);

                        int itemId = itemParams.Get<int>("Id");

                        item.Id = itemId;           
                        item.LocalId = itemId;    


                        await connection.ExecuteAsync(
                            "sp_InsertStockTaggingFromPurchase_Local",
                            new { purchaseId = purchaseId },
                            commandType: CommandType.StoredProcedure
                        );

                    }

                    return purchaseId;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Local Insert Error: " + ex.Message);
                return 0;
            }
        }
        public async Task PostAddAsyncToServer(PurchaseRequest purchaseRequest, List<string> _connectionString)
        {
            try
            {
                using (var connection = new MySqlConnection(_connectionString[1]))
                {
                    await connection.OpenAsync();

                    //using (var transaction = await connection.BeginTransactionAsync())
                    {
                        // Insert into Purchases (server generates new PurchaseId)
                        var parameters = new DynamicParameters(purchaseRequest.Purchase);
                       

                        await connection.ExecuteAsync("sp_InsertPurchase", parameters, commandType: CommandType.StoredProcedure);

                     

                        // Insert into PurchaseItems with correct serverPurchaseId
                        foreach (var item in purchaseRequest.PurchaseItems)
                        {
                           

                            var itemParams = new DynamicParameters(item);
                            itemParams.Add("PurchaseId",purchaseRequest.Purchase.Id );
                            itemParams.Add("firm_id", purchaseRequest.Purchase.firm_id);
                            itemParams.Add("local_id", item.LocalId);
                            await connection.ExecuteAsync("sp_InsertPurchaseItems", itemParams, commandType: CommandType.StoredProcedure);
                            

                        }

                       

                        //await transaction.CommitAsync();
                    }
                }
            }
            catch (MySqlException ex)
            {
                Console.WriteLine("Server Insert Error: " + ex.Message);
                throw;
            }
        }
        public async Task updateonlocal(PurchaseRequest purchaseRequest, List<string> _connectionString)
        {
            using (var connection = new MySqlConnection(_connectionString[0]))
            {
                await connection.ExecuteAsync("sp_MarkPurchaseAndItemsPosted",
                    new { purchaseId = purchaseRequest.Purchase.LocalId },
                    commandType: CommandType.StoredProcedure);
            }
        }



    }
}
