namespace LykkeMarketMakers.Web.Areas.Management.Models
{
    public class ChangePasswordModel
    {
        public string UserId { get; set; }
        public string NewPassword { get; set; }
        public string PasswordConfirmation { get; set; }
    }
}
