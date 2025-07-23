using System.Collections.Generic;
using System.Threading.Tasks;
using Terret_Billing.Domain.Entities;
using Terret_Billing.Presentation.Models;

namespace Terret_Billing.Application.Services.Interfaces
{
    public interface IDiamondService
    {
        Task CreateDiamondEntryAsync(DiamondTaggingEntry entry);
        Task<IEnumerable<DiamondTaggingEntry>> GetDiamondEntriesByTypeAsync(string type, string firmId, string invoiceNumber = null);
        Task<IEnumerable<DiamondTaggingEntry>> GetAllDiamondEntriesAsync(string MetalType, string Firmid);
        Task<DiamondTaggingEntry> GetDiamondEntryByIdAsync(long id);
        Task UpdateDiamondEntryAsync(DiamondTaggingEntry entry);
        Task DeleteDiamondEntryAsync(long id);
        Task<bool> IsInvoiceNumberDuplicateAsync(string invoiceNumber);
        Task<DiamondTaggingEntry> GetDiamondEntryByInvoiceNoAsync(string invoiceNumber);
    }
}