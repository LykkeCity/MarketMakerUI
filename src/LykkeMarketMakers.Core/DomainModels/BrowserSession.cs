using LykkeMarketMakers.Core.Interfaces.DomainModels;

namespace LykkeMarketMakers.Core.DomainModels
{
    public class BrowserSession : IBrowserSession
    {
        public string Id { get; set; }
        public string UserName { get; set; }
    }
}
