using System.Threading.Tasks;
using Dapper;
using MySql.Data.MySqlClient;
using Terret_Billing.Application.DTOs;
using Terret_Billing.Domain.Interfaces;
using System.Linq;
using Terret_Billing.Presentation.Models;
using System.Collections.Generic;

namespace Terret_Billing.Infrastructure.Data.Repositories
{
    public class VoucherNoteRepository : IVoucherNoteRepository
    {
        private readonly List<string> _connectionStrings;

        public VoucherNoteRepository()
        {
            // Use MySqlDatabaseHelper to get both local and server connection strings
            var helper = new MySqlDatabaseHelper();
            _connectionStrings = helper.GetConnectionString();
        }

        public async Task<VoucherNoteItemDto> GetVoucherNoteItemByBarcodeAsync(string barcode)
        {
            using (var connection = new MySqlConnection(_connectionStrings[0]))
            {
                var result = await connection.QueryFirstOrDefaultAsync<VoucherNoteItemDto>(
                    "sp_GetTaggingItemByBarcodeVoucher",
                    new { in_barcode = barcode },
                    commandType: System.Data.CommandType.StoredProcedure
                );
                return result;
            }
        }

        public async Task<VoucherCompanyInfo> GetCompanyDetailsByMobileAsync(string mobile)
        {
            using (var connection = new MySqlConnection(_connectionStrings[0]))
            {
                var result = await connection.QueryFirstOrDefaultAsync<VoucherCompanyInfo>(
                    "sp_GetCompanyDetailsByPhone",
                    new { inputPhoneNumber = mobile },
                    commandType: System.Data.CommandType.StoredProcedure
                );
                return result;
            }
        }

        public async Task InsertVoucherNoteAsync(Terret_Billing.Application.DTOs.VoucherNoteInsertDto dto)
        {
            long localId = 0;
            // Save to local DB with IsSynced=0 by default
            using (var localConnection = new MySql.Data.MySqlClient.MySqlConnection(_connectionStrings[0]))
            {
                var sql = @"INSERT INTO vouchernote
                    (VoucherNoteNumber, user_name, firm_id, assigned_branch, PhoneNumber, ShopName, State, GSTIN, Address, user_name_r, firm_id_r, assigned_branch_r, PhoneNumber_r, ShopName_r, State_r, GSTIN_r, Address_r, date, Barcode, IsSynced)
                    VALUES
                    (@VoucherNoteNumber, @UserName, @FirmId, @AssignedBranch, @PhoneNumber, @ShopName, @State, @GSTIN, @Address, @UserNameR, @FirmIdR, @AssignedBranchR, @PhoneNumberR, @ShopNameR, @StateR, @GSTINR, @AddressR, @Date, @Barcode, 0); SELECT LAST_INSERT_ID();";
                localId = await localConnection.ExecuteScalarAsync<long>(sql, dto);
            }
            try
            {
                // Save to server DB (without IsSynced column)
                using (var serverConnection = new MySql.Data.MySqlClient.MySqlConnection(_connectionStrings[1]))
                {
                    var sql = @"INSERT INTO vouchernote
                        (VoucherNoteNumber, user_name, firm_id, assigned_branch, PhoneNumber, ShopName, State, GSTIN, Address, user_name_r, firm_id_r, assigned_branch_r, PhoneNumber_r, ShopName_r, State_r, GSTIN_r, Address_r, date, Barcode)
                        VALUES
                        (@VoucherNoteNumber, @UserName, @FirmId, @AssignedBranch, @PhoneNumber, @ShopName, @State, @GSTIN, @Address, @UserNameR, @FirmIdR, @AssignedBranchR, @PhoneNumberR, @ShopNameR, @StateR, @GSTINR, @AddressR, @Date, @Barcode)";
                    await serverConnection.ExecuteAsync(sql, dto);
                }
                // Update local row to IsSynced=1
                using (var localConnection = new MySql.Data.MySqlClient.MySqlConnection(_connectionStrings[0]))
                {
                    var updateSql = "UPDATE vouchernote SET IsSynced=1 WHERE id=@Id";
                    await localConnection.ExecuteAsync(updateSql, new { Id = localId });
                }
            }
            catch
            {
                // If server fails, local IsSynced remains 0
            }
        }

        public async Task<string> GetLastVoucherNoteNumberAsync(string firmId)
        {
            using (var connection = new MySqlConnection(_connectionStrings[0]))
            {
                var sql = "SELECT VoucherNoteNumber FROM vouchernote WHERE firm_id = @firmId ORDER BY id DESC LIMIT 1";
                return await connection.QueryFirstOrDefaultAsync<string>(sql, new { firmId });
            }
        }
    }
}
