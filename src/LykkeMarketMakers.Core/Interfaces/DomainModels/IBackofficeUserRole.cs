using LykkeMarketMakers.Core.Enums;

namespace LykkeMarketMakers.Core.Interfaces.DomainModels
{
    public interface IBackofficeUserRole
    {
        string Id { get; }
        string Name { get;}
        UserFeatureAccess[] Features { get; }
    }
}
