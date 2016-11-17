using LykkeMarketMakers.Core.Interfaces.DomainModels;

namespace LykkeMarketMakers.Web.Areas.Management.Models
{
    public class EditUserViewModel : IBackOfficeUser
    {
        public string Create { get; set; }

        public string Id { get; set; }
        public string FullName { get; set; }
        public string[] Roles { get; set; }

        public string IsAdmin { get; set; }
        public string Password { get; set; }
        bool IBackOfficeUser.IsAdmin => !string.IsNullOrEmpty(IsAdmin);
    }
}
