using System.Threading.Tasks;
using LykkeMarketMakers.Core.Interfaces.Repositories;
using LykkeMarketMakers.Web.Areas.Management.Models;
using LykkeMarketMakers.Web.Models.CommonViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using LykkeMarketMakers.Core.DomainModels;
using LykkeMarketMakers.Core.Enums;
using LykkeMarketMakers.Web.Controllers;
using LykkeMarketMakers.Web.Filters;
using LykkeMarketMakers.Web.Translates;

namespace LykkeMarketMakers.Web.Areas.Management.Controllers
{
    [Authorize]
    [Area("Management")]
    [FilterFeaturesAccess(UserFeatureAccess.MenuUsers)]
    public class UsersController : BaseController
    {
        public UsersController(
            IBackOfficeUsersRepository usersRepository, 
            IBackofficeUserRolesRepository userRolesRepository) : base(usersRepository, userRolesRepository)
        {
        }

        [HttpPost]
        public IActionResult Index()
        {
            var model = AreaMenuViewModel.Create(
                AreaMenuItem.Create(Phrases.Users, Url.Action("List")),
                AreaMenuItem.Create(Phrases.Roles, Url.Action("List", "Roles"))
                );

            return View("_ViewAreaMenu", model);
        }

        [HttpPost]
        public async Task<IActionResult> List()
        {
            var model = new UsersListViewModel
            {
                Users = (await UsersRepository.GetAllAsync()).ToList(),
                Roles = (await UserRolesRepository.GetAllRolesAsync()).ToDictionary(itm => itm.Id)
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> EditDialog(string id)
        {
            var model = new EditUserDialogViewModel
            {
                User = string.IsNullOrEmpty(id) ? BackOfficeUser.CreateDefault() : await UsersRepository.GetAsync(id),
                Caption = string.IsNullOrEmpty(id) ? Phrases.AddUser : Phrases.EditUser,
                Roles = (await UserRolesRepository.GetAllRolesAsync()).ToDictionary(itm => itm.Id),
                Width = "900px"
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(EditUserViewModel model)
        {
            if (string.IsNullOrEmpty(model.Id))
            {
                return JsonFailResult(Phrases.FieldShouldNotBeEmpty, "#id");
            }

            if (string.IsNullOrEmpty(model.FullName))
            {
                return JsonFailResult(Phrases.FieldShouldNotBeEmpty, "#fullName");
            }

            // IF we create new account we have to check Password and whether user exists
            if (!string.IsNullOrEmpty(model.Create))
            {
                if (await UsersRepository.UserExists(model.Id))
                {
                    return JsonFailResult(Phrases.UserExists, "#id");
                }

                if (string.IsNullOrEmpty(model.Password))
                {
                    return JsonFailResult(Phrases.FieldShouldNotBeEmpty, "#password");
                }

                await UsersRepository.CreateAsync(model, model.Password);

            }
            else
            {
                await UsersRepository.UpdateAsync(model);
            }

            return JsonRequestResult("#pamain", Url.Action("Index"));
        }

        [HttpPost]
        public ActionResult ChangePasswordDialog(string id)
        {
            var model = new ChangePasswordDialogViewModel
            {
                UserId = id,
                Caption = Phrases.ChangePassword,
                Width = "555px"
            };

            return View(model);
        }

        [HttpPost]
        public async Task<ActionResult> ChangeUserPassword(ChangePasswordModel model)
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

            bool result;

            try
            {
                await UsersRepository.ChangePasswordAsync(model.UserId, model.NewPassword);
                result = true;
            }
            catch
            {
                result = false;
            }

            return result
                ? JsonResultCloseDialog()
                : JsonFailResult(Phrases.ErrorChangePassword, "#btnChangePassword"); ;
        }

    }
}
