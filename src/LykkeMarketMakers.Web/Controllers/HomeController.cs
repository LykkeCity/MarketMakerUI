using System;
using System.Collections.Generic;
using System.Reflection;
using System.Security.Claims;
using System.Threading.Tasks;
using LykkeMarketMakers.Core.Interfaces.DomainModels;
using LykkeMarketMakers.Core.Interfaces.Repositories;
using LykkeMarketMakers.Web.Extensions;
using LykkeMarketMakers.Web.Helpers;
using LykkeMarketMakers.Web.Models.HomeViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Authentication;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using LykkeMarketMakers.Web.Translates;

namespace LykkeMarketMakers.Web.Controllers
{
    public class HomeController : BaseController
    {
        private readonly IBrowserSessionsRepository _browserSessionsRepository;
        private readonly IMenuBadgesRepository _menuBadgesRepository;

        public HomeController(
            IBackOfficeUsersRepository usersRepository,
            IBackofficeUserRolesRepository rolesRepository,
            IBrowserSessionsRepository browserSessionsRepository,
            IMenuBadgesRepository menuBadgesRepository) : base(usersRepository, rolesRepository)
        {
            _browserSessionsRepository = browserSessionsRepository;
            _menuBadgesRepository = menuBadgesRepository;
        }

        public async Task<IActionResult> Index(string culture)
        {
            if (!string.IsNullOrEmpty(culture))
            {
                Response.Cookies.Append(
                    CookieRequestCultureProvider.DefaultCookieName,
                    CookieRequestCultureProvider.MakeCookieValue(new RequestCulture(culture)),
                    new CookieOptions { Expires = DateTimeOffset.UtcNow.AddYears(1) }
                );
            }

            if (User.Identity.IsAuthenticated)
            {
                return View("IndexAuthenticated");
            }

            var sessionId = GetSession();

            var viewModel = new IndexPageModel
            {
                BrowserSession = await _browserSessionsRepository.GetSessionAsync(sessionId)
            };

            return View(viewModel);
        }

        [HttpPost]
        public ActionResult Layout()
        {
            return View();
        }

        public async Task<IActionResult> Menu()
        {
            var viewModel = new MainMenuViewModel
            {
                Ver = Assembly.GetEntryAssembly().GetName().Version.ToString(),
                UserRolesPair = await GetUserRolesPair()
            };

            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Authenticate(AuthenticateModel data)
        {
            if (string.IsNullOrEmpty(data.Username))
            {
                return JsonFailResult(Phrases.FieldShouldNotBeEmpty, "#username");
            }

            if (string.IsNullOrEmpty(data.Password))
            {
                return JsonFailResult(Phrases.FieldShouldNotBeEmpty, "#password");
            }

            var user = await UsersRepository.AuthenticateAsync(data.Username, data.Password);

            if (user == null)
            {
                return JsonFailResult(Phrases.InvalidUsernameOrPassword, "#password");
            }

            var sessionId = GetSession();

            await _browserSessionsRepository.SaveSessionAsync(sessionId, user.Id);

            await SignIn(user);

            var divResult = Request.IsMobileBrowser() ? "#pamain" : "body";

            return JsonRequestResult(divResult, Url.Action("Layout"));
        }

        private async Task SignIn(IBackOfficeUser user)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.Id),
                new Claim(ClaimTypes.Email, user.Id)
            };

            var userIdentity = new ClaimsIdentity(claims, "Passport");

            var userPrincipal = new ClaimsPrincipal(userIdentity);

            await HttpContext.Authentication.SignInAsync(LykkeAuthHelper.AuthenticationScheme, userPrincipal,
                new AuthenticationProperties
                {
                    IsPersistent = false,
                    AllowRefresh = false
                });
        }

        [HttpPost]
        public async Task<IActionResult> GetBadges()
        {
            var badges = await _menuBadgesRepository.GetBadesAsync();
            return Json(badges.Select(itm => new { id = itm.Id, value = itm.Value }));
        }

        public async Task<IActionResult> LogOut()
        {
            await HttpContext.Authentication.SignOutAsync(LykkeAuthHelper.AuthenticationScheme);
            return RedirectToAction("Index");
        }

        public IActionResult Error()
        {
            return View();
        }

        public IActionResult Version()
        {
            return Content(Assembly.GetEntryAssembly().GetName().Version.ToString());
        }
    }
}
