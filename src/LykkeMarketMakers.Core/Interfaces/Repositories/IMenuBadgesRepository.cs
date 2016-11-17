using System.Collections.Generic;
using System.Threading.Tasks;
using LykkeMarketMakers.Core.Interfaces.DomainModels;

namespace LykkeMarketMakers.Core.Interfaces.Repositories
{
    public interface IMenuBadgesRepository
    {
        Task SaveBadgeAsync(string id, string value);
        Task RemoveBadgeAsync(string id);
        Task<IEnumerable<IMenuBadge>> GetBadesAsync();
    }
}
