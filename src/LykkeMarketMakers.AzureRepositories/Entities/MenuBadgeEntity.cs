using LykkeMarketMakers.Core.Interfaces.DomainModels;
using Microsoft.WindowsAzure.Storage.Table;

namespace LykkeMarketMakers.AzureRepositories.Entities
{
    public class MenuBadgeEntity : TableEntity, IMenuBadge
    {
        public static string GeneratePartitionKey()
        {
            return "Badge";
        }

        public static string GenerateRowKey(string badgeId)
        {
            return badgeId;
        }

        public string Id => RowKey;
        public string Value { get; set; }

        public static MenuBadgeEntity Create(string id, string value)
        {
            return new MenuBadgeEntity
            {
                PartitionKey = GeneratePartitionKey(),
                RowKey = GenerateRowKey(id),
                Value = value
            };
        }
    }
}
