using System.Threading.Tasks;
using Terret_Billing.Domain.Entities;
using Terret_Billing.Presentation.Models;

namespace Terret_Billing.Application.Interfaces
{
    public interface IPartyService
    {     



        Task CreatePartyAsync(Customer customer);

        Task<bool> IsGSTNumberDuplicateAsync(string gstNumber);

    }
} 