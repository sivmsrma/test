using System.Collections.Generic;
using System.Threading.Tasks;
using Terret_Billing.Application.Services;
using Terret_Billing.Domain.Entities;
using Terret_Billing.Presentation.Models;

namespace Terret_Billing.Domain.Interfaces
{
    public interface IPartyRepository 
    {
        Task<Customer> GetByIdAsync(int id);

        Task<IEnumerable<Party>> GetAllAsync();

        Task AddAsync(Customer customer);

        Task UpdateAsync(Party party);

        Task DeleteAsync(int id);

        Task<bool> ExistsByGSTNumberAsync(string gstNumber);

        Task<Party> GetByGSTNumberAsync(string gstNumber);

        Task VerifyCreatePartyAsync(Customer customer);

        Task<bool> IsGSTNumberDuplicateAsyncverify(string gstNumber);


        Task<IEnumerable<Party>> GetBySearchTextAsync(string searchText);

    }
} 