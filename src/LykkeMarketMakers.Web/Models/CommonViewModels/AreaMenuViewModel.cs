using System.Collections.Generic;

namespace LykkeMarketMakers.Web.Models.CommonViewModels
{
    public class AreaMenuViewModel
    {
        public List<AreaMenuItem> MenuItems { get; set; } = new List<AreaMenuItem>();

        public static AreaMenuViewModel Create(params AreaMenuItem[] menuItems)
        {
            var result = new AreaMenuViewModel();
            result.MenuItems.AddRange(menuItems);
            return result;
        }
    }
}
