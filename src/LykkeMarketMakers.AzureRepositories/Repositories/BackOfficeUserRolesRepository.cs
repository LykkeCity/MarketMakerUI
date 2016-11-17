using System.Collections.Generic;
using System.Threading.Tasks;
using Lykke.AzureStorage;
using LykkeMarketMakers.AzureRepositories.Entities;
using LykkeMarketMakers.Core.Interfaces.DomainModels;
using LykkeMarketMakers.Core.Interfaces.Repositories;

namespace LykkeMarketMakers.AzureRepositories.Repositories
{
    public class BackOfficeUserRolesRepository : IBackofficeUserRolesRepository
    {
        private readonly INoSQLTableStorage<BackOfficeUserRoleEntity> _tableStorage;

        public BackOfficeUserRolesRepository(INoSQLTableStorage<BackOfficeUserRoleEntity> tableStorage)
        {
            _tableStorage = tableStorage;
        }

        public async Task<IEnumerable<IBackofficeUserRole>> GetAllRolesAsync()
        {
            string partitionKey = BackOfficeUserRoleEntity.GeneratePartitionKey();
            return await _tableStorage.GetDataAsync(partitionKey);
        }

        public async Task<IBackofficeUserRole> GetAsync(string id)
        {
            string partitionKey = BackOfficeUserRoleEntity.GeneratePartitionKey();
            string rowKey = BackOfficeUserRoleEntity.GenegrateRowKey(id);
            return await _tableStorage.GetDataAsync(partitionKey, rowKey);
        }

        public async Task SaveAsync(IBackofficeUserRole data)
        {
            BackOfficeUserRoleEntity newEntity = BackOfficeUserRoleEntity.Create(data);
            await _tableStorage.InsertOrReplaceAsync(newEntity);
        }
    }
}
