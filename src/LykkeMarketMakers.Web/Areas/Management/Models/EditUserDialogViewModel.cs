using System.Collections.Generic;
using LykkeMarketMakers.Core.Interfaces.DomainModels;
using LykkeMarketMakers.Web.Models.CommonViewModels;

namespace LykkeMarketMakers.Web.Areas.Management.Models
{
    public class EditUserDialogViewModel : IModalDialog
    {
        public IBackOfficeUser User { get; set; }
        public Dictionary<string, IBackofficeUserRole> Roles { get; set; }
        public string Caption { get; set; }
        public string Width { get; set; }
    }
}
