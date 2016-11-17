using Microsoft.AspNetCore.Mvc;

namespace LykkeMarketMakers.Web.Controllers
{
    public class ErrorsController : Controller
    {
        [HttpPost]
        public IActionResult AccessDenied()
        {
            return View();
        }
    }
}
