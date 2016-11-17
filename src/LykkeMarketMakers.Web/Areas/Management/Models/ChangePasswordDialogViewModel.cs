using LykkeMarketMakers.Web.Models.CommonViewModels;

namespace LykkeMarketMakers.Web.Areas.Management.Models
{
    public class ChangePasswordDialogViewModel : IModalDialog
    {
        public string UserId { get; set; }
        public string NewPassword { get; set; }
        public string PasswordConfirmation { get; set; }
        public string Caption { get; set; }
        public string Width { get; set; }
    }
}
