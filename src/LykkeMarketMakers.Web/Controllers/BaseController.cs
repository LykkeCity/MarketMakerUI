using System;
using System.Linq;
using System.Threading.Tasks;
using LykkeMarketMakers.Core.Interfaces.DomainModels;
using LykkeMarketMakers.Core.Interfaces.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Lykke.Common;
using LykkeMarketMakers.Core.DomainModels;
using LykkeMarketMakers.Web.Enums;

namespace LykkeMarketMakers.Web.Controllers
{
    public class BaseController : Controller
    {
        protected readonly IBackOfficeUsersRepository UsersRepository;
        protected readonly IBackofficeUserRolesRepository UserRolesRepository;
        private const string OwnershipCookie = "DataOwnership";
        public const string SessionCoocke = "Session";

        public BaseController(IBackOfficeUsersRepository usersRepository, IBackofficeUserRolesRepository userRolesRepository)
        {
            UsersRepository = usersRepository;
            UserRolesRepository = userRolesRepository;
        }

        protected JsonResult JsonFailResult(string message, string div, ErrorPlaceholder placeholder = ErrorPlaceholder.Top)
        {
            return placeholder == ErrorPlaceholder.Top
                ? new JsonResult(new { status = "Fail", msg = message, divError = div })
                : new JsonResult(new { status = "Fail", msg = message, divError = div, placement = placeholder.ToString().ToLower() });
        }

        public JsonResult JsonRequestResult(string div, string url, object model = null, bool putToHistory = false)
        {
            if (model == null)
            {
                return new JsonResult(new { div, refreshUrl = url, showLoading = true });
            }

            string modelAsString = model as string ?? model.ToUrlParamString();
            return new JsonResult(new { div, refreshUrl = url, prms = modelAsString, showLoading = true });
        }

        public JsonResult JsonResultShowDialog(string url, object model = null)
        {
            return new JsonResult(new { status = "ShowDialog", url, prms = model });
        }

        public JsonResult JsonResultReloadData()
        {
            return new JsonResult(new { status = "Reload" });
        }

        public JsonResult JsonRefreshLast()
        {
            return new JsonResult(new { status = "refreshLast" });
        }

        public static JsonResult JsonResultCloseDialog()
        {
            return new JsonResult(new { status = "CloseDialog" });
        }

        public string GetSession()
        {
            string sessionCooke = GetCookie(SessionCoocke);

            if (!string.IsNullOrEmpty(sessionCooke))
            {
                return sessionCooke;
            }

            var sessionId = Guid.NewGuid().ToString();
            SetCookie(SessionCoocke, sessionId, new CookieOptions { Expires = DateTime.UtcNow.AddYears(5) });
            return sessionId;
        }

        public string GetIp()
        {
            return Request.HttpContext.Connection.RemoteIpAddress.ToString();
        }

        public async Task<IBackOfficeUser> GetUserAsync()
        {
            return await UsersRepository.GetAsync(GetUserId());
        }

        public string GetUserId()
        {
            return User.Identity.Name;
        }

        public async Task<UserRolesPair> GetUserRolesPair()
        {
            string userId = GetUserId();

            var result = new UserRolesPair
            {
                User = await UsersRepository.GetAsync(userId),
                Roles = (await UserRolesRepository.GetAllRolesAsync()).ToArray()
            };

            return result;
        }

        public string GetCookie(string key)
        {
            string result;
            return Request.Cookies.TryGetValue(OwnershipCookie, out result)
                ? result
                : null;
        }

        public void SetCookie(string key, string value, CookieOptions options)
        {
            if (string.IsNullOrEmpty(key))
            {
                return;
            }

            Response.Cookies.Append(key, value, options);
        }
    }
}
