namespace LykkeMarketMakers.Web.Models.ViewComponentsViewModels
{
    public class InputWithLabelModel
    {
        public string Name { get; set; }
        public string Caption { get; set; }
        public string Value { get; set; }
        public string Placeholder { get; set; }
        public string Type { get; set; }
        public bool Lg { get; set; } = true;
        public bool ReadOnly { get; set; }
        public bool HasFocus { get; set; }
    }
}
