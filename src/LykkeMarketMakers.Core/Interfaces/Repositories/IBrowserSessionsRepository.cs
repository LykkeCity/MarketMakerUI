using System.Threading.Tasks;
using LykkeMarketMakers.Core.Interfaces.DomainModels;

namespace LykkeMarketMakers.Core.Interfaces.Repositories
{
    public interface IBrowserSessionsRepository
    {
        Task<IBrowserSession> GetSessionAsync(string sessionId);
        Task SaveSessionAsync(string sessionId, string userId);
    }
}
