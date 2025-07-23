using System.Collections.Generic;
using System.Threading.Tasks;
using Terret_Billing.Domain.Entities;

namespace Terret_Billing.Domain.Interfaces
{
    public interface IBranchRepository
    {
        Task<IEnumerable<Branch>> GetAllBranchesAsync();
        Task<Branch> GetBranchByIdAsync(int branchId);
        Task<int> CreateBranchAsync(Branch branch);
        Task<bool> UpdateBranchAsync(Branch branch);
        Task<bool> DeleteBranchAsync(int branchId);
        Task<BranchSummary> GetBranchSummaryAsync(int branchId);
        //Task<IEnumerable<BranchSummary>> GetAllBranchSummariesAsync();
        Task<bool> SaveBranchAsync(Branch branch);
        Task<long> InsertCompanyDetailsAsync(Branch branch, int userId);
        Task<IEnumerable<Branch>> GetAllCompanyDetailsAsync(int id);
        Task<Branch> GetCompanyDetailsByFirmIdAsync(string firmId);

    }
} 