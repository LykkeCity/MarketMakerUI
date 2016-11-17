using System.Collections.Generic;
using System.Threading.Tasks;
using Lykke.AzureStorage;
using LykkeMarketMakers.AzureRepositories.Entities;
using LykkeMarketMakers.AzureRepositories.Extensions;
using LykkeMarketMakers.Core.Interfaces.DomainModels;
using LykkeMarketMakers.Core.Interfaces.Repositories;

namespace LykkeMarketMakers.AzureRepositories.Repositories
{
    public class BackOfficeUsersRepository : IBackOfficeUsersRepository
    {
        private readonly INoSQLTableStorage<BackOfficeUserEntity> _tableStorage;

        public BackOfficeUsersRepository(INoSQLTableStorage<BackOfficeUserEntity> tableStorage)
        {
            _tableStorage = tableStorage;
        }

        public Task CreateAsync(IBackOfficeUser backOfficeUser, string password)
        {
            var newUser = BackOfficeUserEntity.Create(backOfficeUser, password);
            return _tableStorage.InsertOrReplaceAsync(newUser);
        }

        public Task UpdateAsync(IBackOfficeUser backOfficeUser)
        {
            string partitionKey = BackOfficeUserEntity.GeneratePartitionKey();
            string rowKey = BackOfficeUserEntity.GenerateRowKey(backOfficeUser.Id);

            return _tableStorage.MergeAsync(partitionKey, rowKey, entity =>
            {
                entity.Edit(backOfficeUser);
                return entity;
            });
        }

        public Task SaveAsync(IBackOfficeUser backOfficeUser, string password)
        {
            return string.IsNullOrEmpty(backOfficeUser.Id) 
                ? CreateAsync(backOfficeUser, password) 
                : UpdateAsync(backOfficeUser);
        }

        public async Task<IBackOfficeUser> AuthenticateAsync(string username, string password)
        {
            if (username == null || password == null)
            {
                return null;
            }

            string partitionKey = BackOfficeUserEntity.GeneratePartitionKey();
            string rowKey = BackOfficeUserEntity.GenerateRowKey(username);


            BackOfficeUserEntity entity = await _tableStorage.GetDataAsync(partitionKey, rowKey);

            return entity == null 
                ? null 
                : (entity.CheckPassword(password) 
                    ? entity 
                    : null);
        }

        public async Task<IBackOfficeUser> GetAsync(string id)
        {
            if (id == null)
            {
                return null;
            }

            string partitionKey = BackOfficeUserEntity.GeneratePartitionKey();
            string rowKey = BackOfficeUserEntity.GenerateRowKey(id);

            return await _tableStorage.GetDataAsync(partitionKey, rowKey);
        }

        public async Task<bool> UserExists(string id)
        {
            string partitionKey = BackOfficeUserEntity.GeneratePartitionKey();
            string rowKey = BackOfficeUserEntity.GenerateRowKey(id);

            return await _tableStorage.GetDataAsync(partitionKey, rowKey) != null;
        }

        public Task ChangePasswordAsync(string id, string newPassword)
        {
            string partitionKey = BackOfficeUserEntity.GeneratePartitionKey();
            string rowKey = BackOfficeUserEntity.GenerateRowKey(id);

            return _tableStorage.ReplaceAsync(partitionKey, rowKey, itm =>
            {
                itm.SetPassword(newPassword);
                return itm;
            });
        }

        public async Task<IEnumerable<IBackOfficeUser>> GetAllAsync()
        {
            string partitionKey = BackOfficeUserEntity.GeneratePartitionKey();

            return await _tableStorage.GetDataAsync(partitionKey);
        }
    }
}
