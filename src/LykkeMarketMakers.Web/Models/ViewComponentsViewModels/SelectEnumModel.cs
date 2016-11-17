using System;

namespace LykkeMarketMakers.Web.Models.ViewComponentsViewModels
{
    public class SelectEnumModel
    {
        public string Caption { get; set; }
        public string Name { get; set; }
        public Type EnumType { get; set; }
        public Enum CurrentValue { get; set; }
        public string OptionalText { get; set; }
        public bool HasOptional { get; set; }
        public string SelectClasses { get; set; } = "input-lg";
    }
}
