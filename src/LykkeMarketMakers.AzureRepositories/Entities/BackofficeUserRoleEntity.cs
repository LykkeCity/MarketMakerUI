using System;
using System.Collections.Generic;
using System.Linq;
using Lykke.Common;
using LykkeMarketMakers.Core.Enums;
using LykkeMarketMakers.Core.Interfaces.DomainModels;
using Microsoft.WindowsAzure.Storage.Table;

namespace LykkeMarketMakers.AzureRepositories.Entities
{
    public class BackOfficeUserRoleEntity : TableEntity, IBackofficeUserRole
    {
        public string Id => RowKey;
        public string Name { get; set; }
        public string Features { get; set; }

        public static string GeneratePartitionKey()
        {
            return "role";
        }

        public static string GenegrateRowKey(string id)
        {
            return id;
        }

        UserFeatureAccess[] IBackofficeUserRole.Features
            => Features.FromStringViaSeparator('|').Select(itm => itm.ParseEnum(UserFeatureAccess.Nothing)).ToArray();

        private void SetFeatures(IEnumerable<UserFeatureAccess> features)
        {
            Features = features.Select(itm => itm.ToString()).ToStringViaSeparator("|");
        }

        public static BackOfficeUserRoleEntity Create(IBackofficeUserRole src)
        {
            var result = new BackOfficeUserRoleEntity
            {
                PartitionKey = GeneratePartitionKey(),
                RowKey = src.Id == null ? Guid.NewGuid().ToString("N") : GenegrateRowKey(src.Id),
                Name = src.Name
            };

            result.SetFeatures(src.Features);

            return result;
        }
    }
}
