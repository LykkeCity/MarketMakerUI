using Microsoft.AspNetCore.Mvc;

namespace LykkeMarketMakers.Web.ViewComponents
{
    public class CaptionValue : ViewComponent
    {
        public IViewComponentResult Invoke(string caption, string value)
        {
            ViewBag.Caption = caption;
            ViewBag.Value = value;

            return View();
        }
    }
}
