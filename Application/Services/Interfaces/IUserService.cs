using System.Collections.Generic;
using System.Threading.Tasks;
using Terret_Billing.Domain.Entities;

namespace Terret_Billing.Application.Services.Interfaces
{
    public interface IUserService
    {
        Task<IEnumerable<User>> GetAllUsersAsync();
        Task<User> GetUserByIdAsync(int userId);
        Task<int> CreateUserAsync(User user);
        Task<bool> UpdateUserAsync(User user);
        Task<bool> DeleteUserAsync(int userId);
        Task<bool> SaveUserPermissionsAsync(UserPermission permissions);       
        Task<UserPermission> GetUserPermissionsAsync(int userId);
        Task<(IEnumerable<User> Users, int TotalCount)> GetAllUsersPagedAsync(int pageNumber, int pageSize, int creatorId);
    }
}
