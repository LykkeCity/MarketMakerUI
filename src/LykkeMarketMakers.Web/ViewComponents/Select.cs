using LykkeMarketMakers.Web.Models.ViewComponentsViewModels;
using Microsoft.AspNetCore.Mvc;

namespace LykkeMarketMakers.Web.ViewComponents
{
    public class Select : ViewComponent
    {
        public IViewComponentResult Invoke(SelectModel model)
        {
            return View(model);
        }
    }
}
