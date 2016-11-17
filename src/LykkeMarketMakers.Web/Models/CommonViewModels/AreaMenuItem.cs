namespace LykkeMarketMakers.Web.Models.CommonViewModels
{
    public class AreaMenuItem
    {
        public string Caption { get; set; }
        public string Url { get; set; }

        public static AreaMenuItem Create(string caption, string url)
        {
            return new AreaMenuItem
            {
                Caption = caption,
                Url = url
            };
        }
    }
}
