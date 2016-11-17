using LykkeMarketMakers.Core.Enums;
using LykkeMarketMakers.Core.Interfaces.DomainModels;

namespace LykkeMarketMakers.Web.Areas.Management.Models
{
    public class EditRoleViewModel : IBackofficeUserRole
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public UserFeatureAccess[] Features { get; set; }
    }
}
