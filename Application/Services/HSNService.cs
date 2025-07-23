using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terret_Billing.Application.Services.Interfaces;
using Terret_Billing.Domain.Interfaces;
using Terret_Billing.Domain.Entities;

namespace Terret_Billing.Application.Services
{
    public class HSNService : IHSNService
    {
        private readonly IHSNRepository _repo;
        public HSNService(IHSNRepository repo)
        {
            _repo = repo;
        }

        public async Task<IEnumerable<HSNListItem>> GetHSNListWithSyncAsync(string firmId, int codeLength)
        {
            var localData = await _repo.GetHSNListByFirmIdAndLengthAsync(firmId, codeLength, true);
            if (localData == null || !localData.Any())
            {
                var serverData = await _repo.GetHSNListByFirmIdAndLengthAsync(firmId, codeLength, false);
                if (serverData != null && serverData.Any())
                {
                    await _repo.InsertHSNListToLocalAsync(serverData);
                    localData = await _repo.GetHSNListByFirmIdAndLengthAsync(firmId, codeLength, true);
                }
            }
            return localData;
        }
    }
}