using LykkeMarketMakers.Web.Models.ViewComponentsViewModels;
using Microsoft.AspNetCore.Mvc;

namespace LykkeMarketMakers.Web.ViewComponents
{
    public class InputWithLabelOnTop : ViewComponent
    {
        public IViewComponentResult Invoke(InputWithLabelModel model)
        {
            return View(model);
        }
    }
}
