using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terret_Billing.Domain.Entities;


namespace Terret_Billing.Domain.Interfaces
{
    public interface IPurchaseReportRepository
    {
        Task<IEnumerable<PurchaseViewReport>> GetAllPurchaseReportLocalAsync(string firmId);
        Task<IEnumerable<string>> GetPartyNamesWithPurchasesAsync(string firmId);
        Task<IEnumerable<string>> GetBillNoWithPurchasesAsync(string firmId);
        Task<IEnumerable<string>> GetMetalsWithPurchasesAsync(string firmId);
        Task<IEnumerable<string>> GetPurityTypesWithPurchasesAsync(string firmId);
        // Add sync method signature if needed
    }
}


