using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terret_Billing.Domain.Entities;

namespace Terret_Billing.Domain.Interfaces
{
    public interface IHSNRepository
    {
        Task<IEnumerable<HSNListItem>> GetHSNListByFirmIdAndLengthAsync(string firmId, int codeLength, bool useLocal);
        Task InsertHSNListToLocalAsync(IEnumerable<HSNListItem> items);
    }
}
