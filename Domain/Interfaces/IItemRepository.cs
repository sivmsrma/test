using System.Collections.Generic;
using System.Threading.Tasks;
using Terret_Billing.Domain.Entities;

namespace Terret_Billing.Domain.Interfaces
{
    public interface IItemRepository
    {
        // CRUD Operations
        Task<IEnumerable<Item>> GetAllItemsAsync();
        Task<Item> GetItemByIdAsync(int id);
        Task<long> AddItemAsync(Item item);
        Task<bool> UpdateItemAsync(Item item);
        Task<bool> DeleteItemAsync(int id);
        Task<bool> ItemExistsAsync(long id);


        Task<IEnumerable<HsnItem>> GetHsnItemsAsync();
        Task<IEnumerable<HsnItem>> Get4DigitHsnCodesAsync();
        Task<IEnumerable<HsnItem>> Get6DigitHsnCodesAsync();
        Task<IEnumerable<HsnItem>> Get8DigitHsnCodesAsync();

        // Legacy methods from Application.Interfaces.IItemRepository
        Task<IEnumerable<string>> GetCategoriesByMetalTypeAsync(string metalType, string firmId );
        Task<IEnumerable<string>> GetSubCategoriesAsync(string metalType, string category, string firmId );
        Task<IEnumerable<string>> GetDesignsAsync(string metalType, string category, string subCategory, string firmId );
        Task<string> GetHSNCodeAsync(string metalType, string category, string subCategory, string design);
        Task<string> GetShortNameAsync(string metalType, string category, string subCategory, string design);
        Task<int> GetCompanyIdByFirmIdAsync(string firmId);
        Task<string> GetLastBarcodeByCompanyAsync(string shortName, string firmId);
        Task<string> GetLastBarcodeByMetalTypeAsync(string metalType, string shortName);
        Task<string> GetLastDiamondBarcodeAsync(string category, string subCategory, string design);
        Task<string> GetHSNCodeByItemIdAsync(int itemId);
        Task<bool> InsertGoldAndSilverTaggingEntryAsync(GoldAndSilverTaggingEntry entry);
        Task<int?> GetItemIdAsync(string metalType, string category, string subCategory, string design);
        Task<int?> GetItemIdAsync(string metalType, string category, string subCategory, string design, string firmId);
        Task<IEnumerable<string>> GetPuritiesByMetalTypeAsync(string metalType);
    }
}
