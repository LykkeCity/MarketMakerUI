using LykkeMarketMakers.Core.Enums;
using LykkeMarketMakers.Web.Translates;

namespace LykkeMarketMakers.Web.Extensions
{
    public static class EnumExtensions
    {
        public static string GetCaption(this UserFeatureAccess userFeatureAccess)
        {
            switch (userFeatureAccess)
            {
                case UserFeatureAccess.Nothing:
                    return Phrases.Access_None;
                case UserFeatureAccess.MenuUsers:
                    return Phrases.Access_UserMenu;
            }

            return userFeatureAccess.ToString();
        }
    }
}
