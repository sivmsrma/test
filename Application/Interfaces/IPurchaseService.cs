using System.Threading.Tasks;
using Terret_Billing.Domain.Entities;
using Terret_Billing.Presentation.Models;
using Terret_Billing.Presentation.Models.Request;

namespace Terret_Billing.Application.Interfaces
{
    public interface IPurchaseService
    {
        
        Task CreatePurchaseAsync(PurchaseRequest purchaseRequest);
        
    }
}




