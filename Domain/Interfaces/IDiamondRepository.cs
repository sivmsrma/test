using System.Collections.Generic;
using System.Threading.Tasks;
using Terret_Billing.Domain.Entities;

namespace Terret_Billing.Domain.Interfaces
{
    public interface IDiamondRepository
    {
        Task<DiamondTaggingEntry> GetByIdAsync(long id);

        Task<IEnumerable<DiamondTaggingEntry>> GetAllAsync();

        Task AddAsync(DiamondTaggingEntry entry);

        Task UpdateAsync(DiamondTaggingEntry entry);

        Task DeleteAsync(long id);

        Task<bool> ExistsByInvoiceNoAsync(string invoiceNo);

        Task<DiamondTaggingEntry> GetByInvoiceNoAsync(string invoiceNo);
    }
}

