using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terret_Billing.Domain.Entities;

namespace Terret_Billing.Domain.Interfaces
{
    public interface IItemInfoRepository
    {
        Task InsertItemAsync(ItemInfo item, bool isServer = false);
        //Task SyncLocalToServerAsync(string firmId);
        Task<List<ItemInfo>> GetItemInfoByFirmIdAsync(string firmId, bool useLocal = true);
        Task<List<ItemInfo>> GetItemInfoFromServerByFirmIdAsync(string firmId);
        //Task<bool> CategoryExistsAsync(string metal, string category, string firmId);
        Task UpdateItemAsync(ItemInfo item);
    }
}