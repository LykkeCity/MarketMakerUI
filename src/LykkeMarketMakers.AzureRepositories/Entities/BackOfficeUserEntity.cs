using System.Collections.Generic;
using System.Linq;
using Lykke.Common;
using LykkeMarketMakers.AzureRepositories.Extensions;
using LykkeMarketMakers.Core.Interfaces.DomainModels;
using Microsoft.WindowsAzure.Storage.Table;

namespace LykkeMarketMakers.AzureRepositories.Entities
{
    public class BackOfficeUserEntity : TableEntity, IBackOfficeUser
    {
        string[] IBackOfficeUser.Roles => Roles.FromStringViaSeparator('|').ToArray();

        public string Id => RowKey;
        public string FullName { get; set; }
        public string Roles { get; set; }
        
        public bool IsAdmin { get; set; }
        public string Hash { get; set; }
        public string Salt { get; set; }

        public static string GeneratePartitionKey()
        {
            return "BackOfficeUser";
        }

        public static string GenerateRowKey(string id)
        {
            return id.ToLower();
        }

        private void SetRoles(IEnumerable<string> roles)
        {
            Roles = roles.ToStringViaSeparator("|");
        }

        internal void Edit(IBackOfficeUser src)
        {
            IsAdmin = src.IsAdmin;
            FullName = src.FullName;
            SetRoles(src.Roles);
        }

        public static BackOfficeUserEntity Create(IBackOfficeUser src, string password)
        {
            var result = new BackOfficeUserEntity
            {
                PartitionKey = GeneratePartitionKey(),
                RowKey = GenerateRowKey(src.Id.Trim()),
            };

            result.Edit(src);
            result.SetPassword(password);

            return result;
        }
    }
}
