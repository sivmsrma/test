using System.Threading.Tasks;
using Terret_Billing.Domain.Entities;
using System.Collections.Generic;

namespace Terret_Billing.Application.Services.Interfaces
{
    public interface IItemInfoService
    {
        Task InsertItemAsync(ItemInfo item);
        //Task SyncLocalToServerAsync(string firmId);
        Task<List<ItemInfo>> GetItemInfoByFirmIdAsync(string firmId, bool useLocal = true);
        //Task<bool> CategoryExistsAsync(string metal, string category, string firmId);
        Task UpdateItemAsync(ItemInfo item);
    }
} 