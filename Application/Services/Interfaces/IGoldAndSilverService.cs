using System.Collections.Generic;
using System.Threading.Tasks;
using Terret_Billing.Domain.Entities;

namespace Terret_Billing.Application.Services.Interfaces
{
    public interface IGoldAndSilverService
    {
        Task AddEntryAsync(GoldAndSilverTaggingEntry entry);
        Task<(IEnumerable<GoldAndSilverTaggingEntry> Items, int TotalCount)> GetEntriesByTypePagedAsync(string type, string firmId, string invoiceNumber, int pageNumber, int pageSize);  
        Task<decimal?> GetAvailableWeightAsync(string metalType, string partyName, string invoiceNumber, string purity);
    }
}
