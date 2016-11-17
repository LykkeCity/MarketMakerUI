using System.Threading.Tasks;
using LykkeMarketMakers.Core.DomainModels;

namespace LykkeMarketMakers.Web.Services
{
    public interface IUserCacheService
    {
        Task UpdateUsersAndRoles();
        UserRolesPair GetUsersRolePair(string userId);
    }
}
