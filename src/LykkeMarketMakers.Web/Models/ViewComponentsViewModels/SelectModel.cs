using System.Collections.Generic;

namespace LykkeMarketMakers.Web.Models.ViewComponentsViewModels
{
    public class SelectModel
    {
        public string Caption { get; set; }
        public string Name { get; set; }
        public List<KeyValuePair<string, string>> Values { get; set; } = new List<KeyValuePair<string, string>>();
        public string CurrentValue { get; set; }
        public string[] CurrentValues { get; set; }
        public string OptionalText { get; set; }
        public bool IsMultiple { get; set; }
        public string Height { get; set; } = "200px";
        public bool HasOptional { get; set; }
        public string SelectClasses { get; set; } = "input-lg";
    }
}
