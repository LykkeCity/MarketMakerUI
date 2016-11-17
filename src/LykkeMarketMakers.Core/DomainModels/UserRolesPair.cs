using LykkeMarketMakers.Core.Interfaces.DomainModels;

namespace LykkeMarketMakers.Core.DomainModels
{
    public class UserRolesPair
    {
        public IBackOfficeUser User { get; set; }
        public IBackofficeUserRole[] Roles { get; set; }
    }
}
