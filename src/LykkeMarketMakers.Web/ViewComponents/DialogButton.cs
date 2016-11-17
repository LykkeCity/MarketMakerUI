using LykkeMarketMakers.Web.Models.ViewComponentsViewModels;
using Microsoft.AspNetCore.Mvc;

namespace LykkeMarketMakers.Web.ViewComponents
{
    public class DialogButton : ViewComponent
    {
        public IViewComponentResult Invoke(DialogButtonModel model)
        {
            return View(model);
        }
    }
}
