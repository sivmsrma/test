using System.Collections.Generic;
using System.Threading.Tasks;
using Terret_Billing.Domain.Entities;

namespace Terret_Billing.Domain.Interfaces
{
    public interface ITaggingRepository
    {
        Task<TaggingItem> GetByIdAsync(long id);

        Task<IEnumerable<TaggingItem>> GetAllAsync(string firmId = null);

        Task<(IEnumerable<TaggingItem> Items, int TotalCount)> GetAllPagedAsync(
            string firmId,
            string typeOfStock,
            string metalType,
            string partyName,
            string invoiceNumber,
            int pageNumber,
            int pageSize);

        Task AddAsync(TaggingItem item);

        Task UpdateAsync(TaggingItem item);

        Task DeleteAsync(long id);

        Task<bool> ExistsByInvoiceNumberAsync(string invoiceNumber);

        Task<TaggingItem> GetByInvoiceNumberAsync(string invoiceNumber);
        Task<TaggingItem?> GetByBarcodeAsync(string barcode);
    }
}
