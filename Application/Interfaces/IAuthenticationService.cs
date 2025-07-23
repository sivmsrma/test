using System.Threading.Tasks;
using Terret_Billing.Domain.Entities;

namespace Terret_Billing.Application.Interfaces
{
    public interface IAuthenticationService
    {
        Task<User> AuthenticateAsync(string username, string password,string logintype);
        bool ValidateCredentials(string username, string password);
    }
}
