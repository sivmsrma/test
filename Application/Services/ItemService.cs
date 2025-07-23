using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using MySql.Data.MySqlClient;
using Terret_Billing.Application.Services.Interfaces;
using Terret_Billing.Domain.Entities;
using Terret_Billing.Domain.Interfaces;

namespace Terret_Billing.Application.Services
{
    public class ItemService : IItemService
    {
        private readonly IItemRepository _itemRepository;
        private readonly string _connectionString;

        public ItemService(IItemRepository itemRepository, string connectionString)
        {
            _itemRepository = itemRepository ?? throw new ArgumentNullException(nameof(itemRepository));
            _connectionString = connectionString ?? throw new ArgumentNullException(nameof(connectionString));
        }

        public async Task<IEnumerable<Item>> GetAllItemsAsync()
        {
            try
            {
                return await _itemRepository.GetAllItemsAsync();
            }
            catch (Exception ex)
            {

                throw new ApplicationException("An error occurred while retrieving items.", ex);
            }
        }

        public async Task<Item> GetItemByIdAsync(int id)
        {
            try
            {
                return await _itemRepository.GetItemByIdAsync(id);
            }
            catch (Exception ex)
            {
                // Log error
                throw new ApplicationException($"An error occurred while retrieving item with ID {id}.", ex);
            }
        }

        public async Task<int> CreateItemAsync(Item item)
        {
            try
            {
                if (item == null)
                    throw new ArgumentNullException(nameof(item));

                // Add any business validation here
                if (string.IsNullOrWhiteSpace(item.MetalType))
                    throw new ArgumentException("Metal type is required.");

                if (string.IsNullOrWhiteSpace(item.Category))
                    throw new ArgumentException("Category is required.");

                // Get the   barcode for the same metal type and short name
                var lastBarcode = await _itemRepository.GetLastBarcodeByMetalTypeAsync(item.MetalType, item.ShortName);

                // Determine the next incremental number
                var nextNumber = 1;
                if (!string.IsNullOrEmpty(lastBarcode))
                {
                    var parts = lastBarcode.Split('-');
                    if (parts.Length > 1 && int.TryParse(parts[parts.Length - 1], out int lastNumber))
                    {
                        nextNumber = lastNumber + 1;
                    }
                }

                // Generate the new barcode
                item.Barcode = $"{item.ShortName}-{nextNumber:D2}"; // Use :D2 to pad with leading zero if needed

                return (int)await _itemRepository.AddItemAsync(item);
            }
            catch (Exception ex)
            {
                // Log error
                throw new ApplicationException("An error occurred while creating the item.", ex);
            }
        }

        public async Task<bool> UpdateItemAsync(Item item)
        {
            try
            {
                if (item == null)
                    throw new ArgumentNullException(nameof(item));

                // Check if item exists
                var exists = await _itemRepository.ItemExistsAsync(item.ItemId);
                if (!exists)
                    throw new KeyNotFoundException($"Item with ID {item.ItemId} not found.");

                // Add any business validation here
                if (string.IsNullOrWhiteSpace(item.MetalType))
                    throw new ArgumentException("Metal type is required.");

                if (string.IsNullOrWhiteSpace(item.Category))
                    throw new ArgumentException("Category is required.");

                return await _itemRepository.UpdateItemAsync(item);
            }
            catch (KeyNotFoundException)
            {
                throw;
            }
            catch (Exception ex)
            {
                // Log error
                throw new ApplicationException($"An error occurred while updating item with ID {item?.ItemId}.", ex);
            }
        }

        public async Task<bool> DeleteItemAsync(int id)
        {
            try
            {
                // Check if item exists
                var exists = await _itemRepository.ItemExistsAsync(id);
                if (!exists)
                    return false;

                return await _itemRepository.DeleteItemAsync(id);
            }
            catch (Exception ex)
            {
                // Log error
                throw new ApplicationException($"An error occurred while deleting item with ID {id}.", ex);
            }
        }

        public async Task<bool> ItemExistsAsync(int id)
        {
            try
            {
                return await _itemRepository.ItemExistsAsync(id);
            }
            catch (Exception ex)
            {
                // Log error
                throw new ApplicationException($"An error occurred while checking if item with ID {id} exists.", ex);
            }
        }

        public async Task<IEnumerable<HsnItem>> GetHsnItemsAsync()
        {
            try
            {
                return await _itemRepository.GetHsnItemsAsync();
            }
            catch (Exception ex)
            {
                // Log error
                throw new ApplicationException("An error occurred while retrieving HSN items.", ex);
            }
        }

        public async Task<List<HsnItem>> GetHsnListByMetalTypeAsync(string metalType)
        {
            try
            {
                using (var connection = new MySqlConnection(_connectionString))
                {
                    await connection.OpenAsync();
                    const string sql = "SELECT hsn_id AS HsnId, metal AS Metal, hsn_code AS Hsn FROM hsn_list WHERE metal LIKE @Metal";
                    var items = await connection.QueryAsync<HsnItem>(sql, new { Metal = $"%{metalType}%" });
                    return items.AsList();
                }
            }
            catch (Exception ex)
            {
                Terret_Billing.Application.Logging.Logger.LogError("Error getting HSN list", ex);
                return new List<HsnItem>();
            }
        }

        public async Task<IEnumerable<HsnItem>> Get4DigitHsnCodesAsync()
        {
            try
            {
                return await _itemRepository.Get4DigitHsnCodesAsync();
            }
            catch (Exception ex)
            {
                // Log error
                throw new ApplicationException("An error occurred while retrieving 4-digit HSN codes.", ex);
            }
        }

        public async Task<IEnumerable<HsnItem>> Get6DigitHsnCodesAsync()
        {
            try
            {
                return await _itemRepository.Get6DigitHsnCodesAsync();
            }
            catch (Exception ex)
            {
                // Log error
                throw new ApplicationException("An error occurred while retrieving 6-digit HSN codes.", ex);
            }
        }

        public async Task<IEnumerable<HsnItem>> Get8DigitHsnCodesAsync()
        {
            try
            {
                return await _itemRepository.Get8DigitHsnCodesAsync();
            }
            catch (Exception ex)
            {
                // Log error
                throw new ApplicationException("An error occurred while retrieving 8-digit HSN codes.", ex);
            }
        }
    }
}
