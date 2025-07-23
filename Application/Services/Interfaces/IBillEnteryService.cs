using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terret_Billing.Application.DTOs;

namespace Terret_Billing.Application.Services
{
    public interface IBillEntryService
    {
        Task<TaggingItemDto?> FindItemAsync(string barcode);
    }
}
