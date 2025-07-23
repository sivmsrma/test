using System.Collections.Generic;
using System.Threading.Tasks;
using Terret_Billing.Domain.Entities;

namespace Terret_Billing.Domain.Interfaces
{
    public interface IGoldAndSilverRepository
    {
        Task AddAsync(GoldAndSilverTaggingEntry entry);
        Task<(IEnumerable<GoldAndSilverTaggingEntry> Items, int TotalCount)> GetByTypePagedAsync(string type, string firmId, string invoiceNumber, int pageNumber, int pageSize);
        Task<decimal?> GetPendingWeightByFiltersAsync(string metalType, string partyName, string invoiceNumber, string purity);
    }
}
