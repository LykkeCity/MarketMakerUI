using LykkeMarketMakers.Core.Interfaces.DomainModels;
using LykkeMarketMakers.Web.Models.CommonViewModels;

namespace LykkeMarketMakers.Web.Areas.Management.Models
{
    public class EditRoleDialogViewModel : IModalDialog
    {
        public IBackofficeUserRole UserRole { get; set; }
        public string Caption { get; set; }
        public string Width { get; set; }
    }
}
