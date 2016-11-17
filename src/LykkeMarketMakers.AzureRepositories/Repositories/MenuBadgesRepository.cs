using System.Collections.Generic;
using System.Threading.Tasks;
using Lykke.AzureStorage;
using LykkeMarketMakers.AzureRepositories.Entities;
using LykkeMarketMakers.Core.Interfaces.DomainModels;
using LykkeMarketMakers.Core.Interfaces.Repositories;

namespace LykkeMarketMakers.AzureRepositories.Repositories
{
    public class MenuBadgesRepository : IMenuBadgesRepository
    {
        private readonly INoSQLTableStorage<MenuBadgeEntity> _tableStorage;

        public MenuBadgesRepository(INoSQLTableStorage<MenuBadgeEntity> tableStorage)
        {
            _tableStorage = tableStorage;
        }

        public Task SaveBadgeAsync(string id, string value)
        {
            MenuBadgeEntity entity = MenuBadgeEntity.Create(id, value);
            return _tableStorage.InsertOrReplaceAsync(entity);
        }

        public Task RemoveBadgeAsync(string id)
        {
            string partitionKey = MenuBadgeEntity.GeneratePartitionKey();
            string rowKey = MenuBadgeEntity.GenerateRowKey(id);
            return _tableStorage.DeleteAsync(partitionKey, rowKey);
        }

        public async Task<IEnumerable<IMenuBadge>> GetBadesAsync()
        {
            string partitionKey = MenuBadgeEntity.GeneratePartitionKey();
            return await _tableStorage.GetDataAsync(partitionKey);
        }
    }
}
