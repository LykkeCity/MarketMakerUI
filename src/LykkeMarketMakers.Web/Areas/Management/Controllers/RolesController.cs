using System.Linq;
using System.Threading.Tasks;
using LykkeMarketMakers.Core.DomainModels;
using LykkeMarketMakers.Core.Enums;
using LykkeMarketMakers.Core.Interfaces.Repositories;
using LykkeMarketMakers.Web.Areas.Management.Models;
using LykkeMarketMakers.Web.Filters;
using LykkeMarketMakers.Web.Translates;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using BaseController = LykkeMarketMakers.Web.Controllers.BaseController;

namespace LykkeMarketMakers.Web.Areas.Management.Controllers
{
    [Authorize]
    [Area("Management")]
    [FilterFeaturesAccess(UserFeatureAccess.MenuUsers)]
    public class RolesController : BaseController
    {
        public RolesController(
            IBackOfficeUsersRepository usersRepository, 
            IBackofficeUserRolesRepository userRolesRepository) : base(usersRepository, userRolesRepository)
        {

        }

        [HttpPost]
        public async Task<IActionResult> List()
        {
            var model = new RolesListViewModel
            {
                UserRoles = (await UserRolesRepository.GetAllRolesAsync()).ToList()
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> EditDialog(string id)
        {
            var model = new EditRoleDialogViewModel
            {
                UserRole = string.IsNullOrEmpty(id) ? BackofficeUserRole.Default : await UserRolesRepository.GetAsync(id),
                Caption = string.IsNullOrEmpty(id) ? Phrases.AddRole : Phrases.EditRole
            };

            return View(model);
        }

        [HttpPost]
        public async Task<ActionResult> Edit(EditRoleViewModel model)
        {
            if (string.IsNullOrEmpty(model.Name))
            {
                return JsonFailResult(Phrases.FieldShouldNotBeEmpty, "#name");
            }

            if (model.Features == null)
            {
                return JsonFailResult(Phrases.PleaseSelectAtLeastOneItem, "#features");
            }

            await UserRolesRepository.SaveAsync(model);

            return JsonResultReloadData();
        }
    }
}
