using LykkeMarketMakers.Core.Interfaces.DomainModels;

namespace LykkeMarketMakers.Core.DomainModels
{
    public class BackOfficeUser : IBackOfficeUser
    {
        public string Id { get; set; }
        public string FullName { get; set; }
        public string[] Roles { get; set; }
        public bool IsAdmin { get; set; }


        public static BackOfficeUser CreateDefaultAdminUser(string id)
        {
            return new BackOfficeUser
            {
                Id = id,
                FullName = "Admin",
                IsAdmin = true,
                Roles = new string[0]
            };
        }

        public static IBackOfficeUser CreateDefault()
        {
            return new BackOfficeUser
            {
                IsAdmin = false,
                Roles = new string[0]
            };
        }
    }
}