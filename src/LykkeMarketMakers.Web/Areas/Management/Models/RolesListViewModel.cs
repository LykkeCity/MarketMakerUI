using System.Collections.Generic;
using LykkeMarketMakers.Core.Interfaces.DomainModels;

namespace LykkeMarketMakers.Web.Areas.Management.Models
{
    public class RolesListViewModel
    {
        public List<IBackofficeUserRole> UserRoles { get; set; }
    }
}
