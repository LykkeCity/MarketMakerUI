using System;
using System.Security.Cryptography;
using Lykke.Common;
using LykkeMarketMakers.AzureRepositories.Entities;

namespace LykkeMarketMakers.AzureRepositories.Extensions
{
    public static class BackOfficeUserExtensions
    {
        public static void SetPassword(this BackOfficeUserEntity entity, string password)
        {
            entity.Salt = Guid.NewGuid().ToString();
            entity.Hash = CalcHash(password, entity.Salt);
        }

        public static bool CheckPassword(this BackOfficeUserEntity entity, string password)
        {
            var hash = CalcHash(password, entity.Salt);
            return entity.Hash == hash;
        }

        private static string CalcHash(string password, string salt)
        {
            var sha1 = SHA1.Create();
            var hash = sha1.ComputeHash((password + salt).ToUtf8Bytes());

            return Convert.ToBase64String(hash);
        }
    }
}
