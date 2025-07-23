using Dapper;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Terret_Billing.Domain.Interfaces;
using Terret_Billing.Presentation.Models;
using Terret_Billing.Presentation.Models.Request;
using Terret_Billing.Domain.Entities;

namespace Terret_Billing.Infrastructure.Data.Repositories
{
    public class BillRepository : IBillRepository
    {
        private readonly List<string> _connectionStrings;
        private readonly IDatabaseHelper _databaseHelper;

        public BillRepository(IDatabaseHelper databaseHelper)
        {
            _databaseHelper = databaseHelper;
            _connectionStrings = databaseHelper.GetConnectionString();
        }

        public async Task AddAsync(BillRequest billRequest)
        {
            billRequest.Bill.LocalId = await PostAddAsyncToLocal(billRequest, _connectionStrings);

            foreach (var item in billRequest.BillItem)
                item.LocalId = item.Id;

            await PostAddAsyncToServer(billRequest, _connectionStrings);
            await UpdateOnLocal(billRequest, _connectionStrings);
        }

        private DynamicParameters MapToParameters<T>(T obj, params (string name, DbType dbType, ParameterDirection direction, int? size)[] extraParams)
        {
            var parameters = new DynamicParameters();
            foreach (var prop in typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance))
                parameters.Add(prop.Name, prop.GetValue(obj));

            foreach (var param in extraParams)
                parameters.Add(param.name, dbType: param.dbType, direction: param.direction, size: param.size);

            return parameters;
        }

        //public async Task<int> PostAddAsyncToLocal(BillRequest billRequest, List<string> _connectionStrings)
        //{
        //    using var connection = new MySqlConnection(_connectionStrings[0]);
        //        await connection.OpenAsync();

        //    var billParameters = MapToParameters(billRequest.Bill,
        //        ("NewBillId", DbType.Int32, ParameterDirection.Output, null),
        //        ("BillNo", DbType.String, ParameterDirection.Output, 50));

        //    await connection.ExecuteAsync("sp_AddBillHeader_local", billParameters, commandType: CommandType.StoredProcedure);

        //    billRequest.Bill.Id = billParameters.Get<int>("NewBillId");
        //    billRequest.Bill.BillNo = billParameters.Get<string>("BillNo");

        //        foreach (var item in billRequest.BillItem)
        //        {
        //            item.BillId = billRequest.Bill.Id;
        //        var itemParams = MapToParameters(item, ("NewItemId", DbType.Int32, ParameterDirection.Output, null));
        //            await connection.ExecuteAsync("sp_AddBillItem_local", itemParams, commandType: CommandType.StoredProcedure);
        //        item.Id = item.LocalId = itemParams.Get<int>("NewItemId");
        //        }

        //        if (billRequest.BillPayment != null)
        //        {
        //            foreach (var payment in billRequest.BillPayment)
        //            {
        //                payment.BillId = billRequest.Bill.Id;
        //            var paymentParams = MapToParameters(payment, ("NewPaymentId", DbType.Int32, ParameterDirection.Output, null));
        //                await connection.ExecuteAsync("sp_AddBillPayment_local", paymentParams, commandType: CommandType.StoredProcedure);
        //            payment.Id = payment.LocalId = paymentParams.Get<int>("NewPaymentId");
        //        }
        //    }

        //    return (int)billRequest.Bill.Id;
        //}
        public async Task<int> PostAddAsyncToLocal(BillRequest billRequest, List<string> _connectionStrings)
        {
            using var connection = new MySqlConnection(_connectionStrings[0]);
            await connection.OpenAsync();
            using var transaction = await connection.BeginTransactionAsync();

            try
            {
                var billParameters = MapToParameters(billRequest.Bill,
                    ("NewBillId", DbType.Int32, ParameterDirection.Output, null),
                    ("BillNo", DbType.String, ParameterDirection.Output, 50));

                await connection.ExecuteAsync("sp_AddBillHeader_local", billParameters, transaction: transaction, commandType: CommandType.StoredProcedure);

                billRequest.Bill.Id = billParameters.Get<int>("NewBillId");
                billRequest.Bill.BillNo = billParameters.Get<string>("BillNo");

                foreach (var item in billRequest.BillItem)
                {
                    item.BillId = billRequest.Bill.Id;
                    var itemParams = MapToParameters(item, ("NewItemId", DbType.Int32, ParameterDirection.Output, null));
                    await connection.ExecuteAsync("sp_AddBillItem_local", itemParams, transaction: transaction, commandType: CommandType.StoredProcedure);
                    item.Id = item.LocalId = itemParams.Get<int>("NewItemId");
                }

                if (billRequest.BillPayment != null)
                {
                    foreach (var payment in billRequest.BillPayment)
                    {
                        payment.BillId = billRequest.Bill.Id;
                        var paymentParams = MapToParameters(payment, ("NewPaymentId", DbType.Int32, ParameterDirection.Output, null));
                        await connection.ExecuteAsync("sp_AddBillPayment_local", paymentParams, transaction: transaction, commandType: CommandType.StoredProcedure);
                        payment.Id = payment.LocalId = paymentParams.Get<int>("NewPaymentId");
                    }
                }

                await transaction.CommitAsync();
                return (int)billRequest.Bill.Id;
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }

        //public async Task PostAddAsyncToServer(BillRequest billRequest, List<string> _connectionStrings)
        //{
        //    using var connection = new MySqlConnection(_connectionStrings[1]);
        //        await connection.OpenAsync();

        //    var billParams = MapToParameters(billRequest.Bill);
        //        await connection.ExecuteAsync("sp_AddBillHeader_server", billParams, commandType: CommandType.StoredProcedure);

        //        foreach (var item in billRequest.BillItem)
        //        {
        //        var itemParams = MapToParameters(item);
        //        itemParams.Add("BillLocalId", billRequest.Bill.LocalId);
        //        itemParams.Add("BillFirmId", billRequest.Bill.FirmId);
        //        itemParams.Add("LocalId", item.LocalId);

        //            await connection.ExecuteAsync("sp_AddBillItem_server", itemParams, commandType: CommandType.StoredProcedure);
        //        }

        //        if (billRequest.BillPayment != null)
        //        {
        //            foreach (var payment in billRequest.BillPayment)
        //            {
        //            var paymentParams = MapToParameters(payment);
        //            paymentParams.Add("BillLocalId", billRequest.Bill.LocalId);
        //            paymentParams.Add("BillFirmId", billRequest.Bill.FirmId);
        //                await connection.ExecuteAsync("sp_AddBillPayment_server", paymentParams, commandType: CommandType.StoredProcedure);
        //        }
        //    }
        //}
        public async Task PostAddAsyncToServer(BillRequest billRequest, List<string> _connectionStrings)
        {
            using var connection = new MySqlConnection(_connectionStrings[1]);
            await connection.OpenAsync();
            using var transaction = await connection.BeginTransactionAsync();

            try
            {
                var billParams = MapToParameters(billRequest.Bill);
                await connection.ExecuteAsync("sp_AddBillHeader_server", billParams, transaction: transaction, commandType: CommandType.StoredProcedure);

                foreach (var item in billRequest.BillItem)
                {
                    var itemParams = MapToParameters(item);
                    itemParams.Add("BillLocalId", billRequest.Bill.LocalId);
                    itemParams.Add("BillFirmId", billRequest.Bill.FirmId);
                    itemParams.Add("LocalId", item.LocalId);

                    await connection.ExecuteAsync("sp_AddBillItem_server", itemParams, transaction: transaction, commandType: CommandType.StoredProcedure);
                }

                if (billRequest.BillPayment != null)
                {
                    foreach (var payment in billRequest.BillPayment)
                    {
                        var paymentParams = MapToParameters(payment);
                        paymentParams.Add("BillLocalId", billRequest.Bill.LocalId);
                        paymentParams.Add("BillFirmId", billRequest.Bill.FirmId);

                        await connection.ExecuteAsync("sp_AddBillPayment_server", paymentParams, transaction: transaction, commandType: CommandType.StoredProcedure);
                    }
                }

                await transaction.CommitAsync();
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }


        //public async Task UpdateOnLocal(BillRequest billRequest, List<string> _connectionStrings)
        //{
        //    using var connection = new MySqlConnection(_connectionStrings[0]);
        //    await connection.ExecuteAsync("sp_MarkBillAndItemsPosted", new { billId = billRequest.Bill.LocalId }, commandType: CommandType.StoredProcedure);
        //    }
        public async Task UpdateOnLocal(BillRequest billRequest, List<string> _connectionStrings)
        {
            using var connection = new MySqlConnection(_connectionStrings[0]);
            await connection.OpenAsync();
            using var transaction = await connection.BeginTransactionAsync();

            try
            {
                await connection.ExecuteAsync("sp_MarkBillAndItemsPosted", new { billId = billRequest.Bill.LocalId }, transaction: transaction, commandType: CommandType.StoredProcedure);
                await transaction.CommitAsync();
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }


        public async Task<(Bill Bill, List<BillItem> Items, List<BillPayment> Payments, Branch Company)> GetFullBillForPrintAsync(int billId, string firmId)
        {
            using var connection = new MySqlConnection(_connectionStrings[0]);
            await connection.OpenAsync();

              using var multi = await connection.QueryMultipleAsync(
                  "sp_GetFullBillForPrint",
                  new { in_BillId = billId, in_FirmId = firmId },
                  commandType: CommandType.StoredProcedure
              );


            var bill = await multi.ReadFirstOrDefaultAsync<Bill>();
            var items = (await multi.ReadAsync<BillItem>()).ToList();
            var payments = (await multi.ReadAsync<BillPayment>()).ToList();
            var company = await multi.ReadFirstOrDefaultAsync<Branch>();
            var party = await multi.ReadFirstOrDefaultAsync<Customer>();
            return (bill, items, payments, company);
        } 
    }
}
