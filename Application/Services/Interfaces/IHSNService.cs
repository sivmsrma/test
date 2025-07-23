using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terret_Billing.Domain.Entities;

namespace Terret_Billing.Application.Services.Interfaces
{
    public interface IHSNService
    {
        Task<IEnumerable<HSNListItem>> GetHSNListWithSyncAsync(string firmId, int codeLength);
    }
}
