using LykkeMarketMakers.Web.Models.ViewComponentsViewModels;
using Microsoft.AspNetCore.Mvc;

namespace LykkeMarketMakers.Web.ViewComponents
{
    public class SaveCancelButtonsPair : ViewComponent
    {
        public IViewComponentResult Invoke(SaveCancelButtonsPairModel model)
        {
            return View(model);
        }
    }
}
