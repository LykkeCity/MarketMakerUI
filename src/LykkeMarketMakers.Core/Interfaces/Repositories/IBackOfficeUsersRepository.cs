using System.Collections.Generic;
using System.Threading.Tasks;
using LykkeMarketMakers.Core.Interfaces.DomainModels;

namespace LykkeMarketMakers.Core.Interfaces.Repositories
{
    public interface IBackOfficeUsersRepository
    {
        Task CreateAsync(IBackOfficeUser backOfficeUser, string password);
        Task UpdateAsync(IBackOfficeUser backOfficeUser);

        Task<IBackOfficeUser> AuthenticateAsync(string username, string password);
        Task<IBackOfficeUser> GetAsync(string id);

        Task<bool> UserExists(string id);
        Task ChangePasswordAsync(string id, string newPassword);
        Task<IEnumerable<IBackOfficeUser>> GetAllAsync();
    }
}
