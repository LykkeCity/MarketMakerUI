using LykkeMarketMakers.Core.Interfaces.DomainModels;
using Microsoft.WindowsAzure.Storage.Table;

namespace LykkeMarketMakers.AzureRepositories.Entities
{
    public class BrowserSessionEntity : TableEntity, IBrowserSession
    {
        public static string GeneratePartitionKey()
        {
            return "BrowSess";
        }

        public static string GenerateRowKey(string id)
        {
            return id;
        }

        public string Id => RowKey;
        public string UserName { get; set; }

        public static BrowserSessionEntity Create(string id, string userName)
        {
            return new BrowserSessionEntity
            {
                PartitionKey = GeneratePartitionKey(),
                RowKey = GenerateRowKey(id),
                UserName = userName
            };
        }
    }
}
