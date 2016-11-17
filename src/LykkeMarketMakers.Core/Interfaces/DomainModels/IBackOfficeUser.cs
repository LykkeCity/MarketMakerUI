namespace LykkeMarketMakers.Core.Interfaces.DomainModels
{
    public interface IBackOfficeUser
    {
        string Id { get; }
        string FullName { get; }
        string[] Roles { get; }
        bool IsAdmin { get; }
    }
}
