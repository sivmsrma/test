using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terret_Billing.Domain.Entities;
using Terret_Billing.Domain.Interfaces;
using Terret_Billing.Application.Services.Interfaces;

namespace Terret_Billing.Application.Services
{
     public class ItemInfoService : IItemInfoService
    {
        private readonly IItemInfoRepository _repo;
        public ItemInfoService(IItemInfoRepository repo)
        {
            _repo = repo;
        }

        public async Task InsertItemAsync(ItemInfo item)
        {
            await _repo.InsertItemAsync(item, false); // Save to local
            //await _repo.SyncLocalToServerAsync(item.firm_id); // Sync to server
        }

        //public async Task SyncLocalToServerAsync(string firmId)
        //{
        //    await _repo.SyncLocalToServerAsync(firmId);
        //}

        public async Task<List<ItemInfo>> GetItemInfoByFirmIdAsync(string firmId, bool useLocal = true)
        {
            var localData = await _repo.GetItemInfoByFirmIdAsync(firmId, useLocal: true);
            if (localData != null && localData.Any())
                return localData;
            // If not found locally, fetch from server
            var serverData = await _repo.GetItemInfoFromServerByFirmIdAsync(firmId);
            return serverData;
        }

        //public async Task<bool> CategoryExistsAsync(string metal, string category, string firmId)
        //{
        //    return await _repo.CategoryExistsAsync(metal, category, firmId);
        //}

        public async Task UpdateItemAsync(ItemInfo item)
        {
            await _repo.UpdateItemAsync(item);
            //await _repo.SyncLocalToServerAsync(item.firm_id);
        }
    }
}