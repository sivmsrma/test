using System.Collections.Generic;
using System.Threading.Tasks;
using Terret_Billing.Domain.Entities;

namespace Terret_Billing.Application.Services.Interfaces
{
    public interface IItemService
    {
        Task<IEnumerable<Item>> GetAllItemsAsync();
        Task<Item> GetItemByIdAsync(int id);
        Task<int> CreateItemAsync(Item item);
        Task<bool> UpdateItemAsync(Item item);
        Task<bool> DeleteItemAsync(int id);
        Task<bool> ItemExistsAsync(int id);

        // HSN Operations
        Task<IEnumerable<HsnItem>> GetHsnItemsAsync();
        Task<IEnumerable<HsnItem>> Get4DigitHsnCodesAsync();
        Task<IEnumerable<HsnItem>> Get6DigitHsnCodesAsync();
        Task<IEnumerable<HsnItem>> Get8DigitHsnCodesAsync();
    }
}
