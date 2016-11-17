using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using LykkeMarketMakers.Web.Models.ViewComponentsViewModels;

namespace LykkeMarketMakers.Web.ViewComponents
{
    public class SelectFromEnum : ViewComponent
    {
        public IViewComponentResult Invoke(SelectEnumModel model)
        {
            List<KeyValuePair<string, string>> values = Enum.GetValues(model.EnumType).Cast<Enum>()
                .Select(item => new KeyValuePair<string, string>(item.ToString(), item.ToString())).ToList();

            var selectModel = new SelectModel
            {
                Caption = model.Caption,
                Name = model.Name,
                Values = values,
                CurrentValue = model.CurrentValue?.ToString(),
                HasOptional = model.HasOptional,
                OptionalText = model.OptionalText,
                SelectClasses = model.SelectClasses
            };

            return View(selectModel);
        }
    }
}
