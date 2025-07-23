using System.Collections.Generic;
using System.Threading.Tasks;
using Terret_Billing.Domain.Entities;
using Terret_Billing.Presentation.Models;
using Terret_Billing.Presentation.Models.Request;

namespace Terret_Billing.Domain.Interfaces
{
    public interface IPurchaseRepository
    {

       // Task<PurchaseRequest> GetByGetByIdAsync(int bill_no);
       
        Task AddAsync(PurchaseRequest purchaseRequest);

        //Task UpdateAsync(PurchaseRequest purchaseRequest);
    }
}
