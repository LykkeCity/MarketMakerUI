using System.Linq;
using LykkeMarketMakers.Core.DomainModels;
using LykkeMarketMakers.Core.Enums;
using LykkeMarketMakers.Core.Interfaces.DomainModels;

namespace LykkeMarketMakers.Core.Extensions
{
    public static class UserExtensions
    {
        public static bool HasAccessToFeature(this IBackOfficeUser src, IBackofficeUserRole[] roles, UserFeatureAccess feature)
        {
            if (src.IsAdmin)
            {
                return true;
            }

            foreach (var roleId in src.Roles)
            {
                var foundRole = roles.FirstOrDefault(role => role.Id == roleId);

                if (foundRole == null)
                {
                    continue;
                }

                if (foundRole.Features.Any(f => f == feature))
                {
                    return true;
                }
            }

            return false;
        }

        public static bool HasAccessToFeature(this UserRolesPair src, UserFeatureAccess feature)
        {
            return HasAccessToFeature(src.User, src.Roles, feature);
        }
    }
}
