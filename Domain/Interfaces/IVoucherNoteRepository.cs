using System.Threading.Tasks;
using Terret_Billing.Application.DTOs;
using Terret_Billing.Presentation.Models;

namespace Terret_Billing.Domain.Interfaces
{
    public interface IVoucherNoteRepository
    {
        Task<VoucherNoteItemDto> GetVoucherNoteItemByBarcodeAsync(string barcode);
        Task<VoucherCompanyInfo> GetCompanyDetailsByMobileAsync(string mobile);
        Task InsertVoucherNoteAsync(Terret_Billing.Application.DTOs.VoucherNoteInsertDto dto);
        Task<string> GetLastVoucherNoteNumberAsync(string firmId);
    }
} 