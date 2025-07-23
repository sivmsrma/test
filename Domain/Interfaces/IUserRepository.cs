using System.Collections.Generic;
using System.Threading.Tasks;
using Terret_Billing.Domain.Entities;

namespace Terret_Billing.Domain.Interfaces
{
    public interface IUserRepository
    {
        Task<User> GetByIdAsync(int id);

        Task<IEnumerable<User>> GetAllAsync(int id);

        Task<int> AddAsync(User user);

        Task UpdateAsync(User user);

        Task DeleteAsync(int id);

        Task<bool> ExistsByUsernameAsync(string username);

        Task<bool> ExistsByEmailAsync(string email);

        Task<User> GetByUsernameAsync(string username);

        Task<User> GetByEmailAsync(string email);

        Task<UserPermission> GetUserPermissionsAsync(int userId);

        Task<bool> SaveUserPermissionsAsync(UserPermission permissions);

        Task<(IEnumerable<User> Users, int TotalCount)> GetAllUsersPagedAsync(int pageNumber, int pageSize, int creatorId);

      }
} 
