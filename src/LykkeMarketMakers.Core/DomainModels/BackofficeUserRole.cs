using LykkeMarketMakers.Core.Enums;
using LykkeMarketMakers.Core.Interfaces.DomainModels;

namespace LykkeMarketMakers.Core.DomainModels
{
    public class BackofficeUserRole : IBackofficeUserRole
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public UserFeatureAccess[] Features { get; set; }


        public static BackofficeUserRole Create(IBackofficeUserRole src)
        {
            return new BackofficeUserRole
            {
                Id = src.Id,
                Features = src.Features,
                Name = src.Name
            };
        }

        public static readonly BackofficeUserRole Default = new BackofficeUserRole
        {
            Features = new UserFeatureAccess[0]
        };
    }
}
