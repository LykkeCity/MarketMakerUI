using System.Collections.Generic;
using LykkeMarketMakers.Core.Interfaces.DomainModels;

namespace LykkeMarketMakers.Web.Areas.Management.Models
{
    public class UsersListViewModel
    {
        public List<IBackOfficeUser> Users { get; set; }
        public Dictionary<string, IBackofficeUserRole> Roles { get; set; }
    }
}
