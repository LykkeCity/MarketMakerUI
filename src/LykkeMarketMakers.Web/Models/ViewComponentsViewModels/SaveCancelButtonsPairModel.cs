using LykkeMarketMakers.Web.Translates;

namespace LykkeMarketMakers.Web.Models.ViewComponentsViewModels
{
    public class SaveCancelButtonsPairModel
    {
        public string Url { get; set; }
        public string FormId { get; set; }
        public string SaveText { get; set; } = Phrases.Save;
        public bool IsMobile { get; set; }
    }
}
