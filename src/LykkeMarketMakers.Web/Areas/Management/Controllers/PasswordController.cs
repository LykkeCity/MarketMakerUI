using System.Threading.Tasks;
using LykkeMarketMakers.Core.Interfaces.Repositories;
using LykkeMarketMakers.Web.Areas.Management.Models;
using LykkeMarketMakers.Web.Controllers;
using LykkeMarketMakers.Web.Translates;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LykkeMarketMakers.Web.Areas.Management.Controllers
{
    [Authorize]
    [Area("Management")]
    public class PasswordController : BaseController
    {
        public PasswordController(
            IBackOfficeUsersRepository usersRepository, 
            IBackofficeUserRolesRepository userRolesRepository) : base(usersRepository, userRolesRepository)
        {
        }

        public IActionResult ChangeMyPasswordForm()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> ChangeMyPassword(ChangePasswordModel model)
        {
            if (string.IsNullOrEmpty(model.NewPassword))
            {
                return JsonFailResult(Phrases.FieldShouldNotBeEmpty, "#newPassword");
            }

            if (string.IsNullOrEmpty(model.PasswordConfirmation))
            {
                return JsonFailResult(Phrases.FieldShouldNotBeEmpty, "#passwordConfirmation");
            }

            if (model.NewPassword != model.PasswordConfirmation)
            {
                return JsonFailResult(Phrases.PasswordsDoNotMatch, "#passwordConfirmation");
            }

            await UsersRepository.ChangePasswordAsync(GetUserId(), model.NewPassword);

            return JsonFailResult(Phrases.PasswordIsNowChanged, "#btnChangePassword");
        }
    }
}
