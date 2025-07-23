using System.Collections.Generic;
using System.Threading.Tasks;
using Terret_Billing.Domain.Entities;

namespace Terret_Billing.Application.Services.Interfaces
{
    public interface ITaggingService
    {
        Task<IEnumerable<TaggingItem>> GetAllTaggingsAsync(string firmId = null);

        Task<TaggingItem> GetTaggingByIdAsync(long id);

        Task CreateTaggingAsync(TaggingItem item);

        Task UpdateTaggingAsync(TaggingItem item);

        Task DeleteTaggingAsync(long id);

        Task<bool> IsInvoiceNumberDuplicateAsync(string invoiceNumber);

        Task<TaggingItem> GetTaggingByInvoiceNumberAsync(string invoiceNumber);

        Task<(IEnumerable<TaggingItem> Items, int TotalCount)> GetAllTaggingsPagedAsync( string firmId, string typeOfStock, string metalType, string partyName, string invoiceNumber, int pageNumber, int pageSize);
    }
}
