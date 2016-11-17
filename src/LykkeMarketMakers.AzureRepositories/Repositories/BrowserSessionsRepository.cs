using System.Threading.Tasks;
using Lykke.AzureStorage;
using LykkeMarketMakers.AzureRepositories.Entities;
using LykkeMarketMakers.Core.Interfaces.DomainModels;
using LykkeMarketMakers.Core.Interfaces.Repositories;

namespace LykkeMarketMakers.AzureRepositories.Repositories
{
    public class BrowserSessionsRepository : IBrowserSessionsRepository
    {
        private readonly INoSQLTableStorage<BrowserSessionEntity> _tableStorage;

        public BrowserSessionsRepository(INoSQLTableStorage<BrowserSessionEntity> tableStorage)
        {
            _tableStorage = tableStorage;
        }

        public async Task<IBrowserSession> GetSessionAsync(string sessionId)
        {
            string partitionKey = BrowserSessionEntity.GeneratePartitionKey();
            string rowKey = BrowserSessionEntity.GenerateRowKey(sessionId);
            return await _tableStorage.GetDataAsync(partitionKey, rowKey);
        }

        public Task SaveSessionAsync(string sessionId, string userId)
        {
            BrowserSessionEntity newEntity = BrowserSessionEntity.Create(sessionId, userId);
            return _tableStorage.InsertOrReplaceAsync(newEntity);
        }
    }
}
