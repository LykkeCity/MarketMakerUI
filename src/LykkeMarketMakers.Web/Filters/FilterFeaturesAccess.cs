using LykkeMarketMakers.Core.Enums;
using LykkeMarketMakers.Core.Extensions;
using LykkeMarketMakers.Web.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;

namespace LykkeMarketMakers.Web.Filters
{
    public class FilterFeaturesAccessAttribute : ActionFilterAttribute
    {
        private readonly UserFeatureAccess[] _filters;

        public FilterFeaturesAccessAttribute(params UserFeatureAccess[] filters)
        {
            _filters = filters;
        }

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            IUserCacheService userCacheService = filterContext.HttpContext.RequestServices.GetService<IUserCacheService>();
            var userId = filterContext.HttpContext.User.Identity.Name;

            if (userId == null)
            {
                filterContext.Result = new UnauthorizedResult();
            }
            else
            {
                var userPair = userCacheService.GetUsersRolePair(userId);

                foreach (var userFeatureAccess in _filters)
                {
                    if (userPair.HasAccessToFeature(userFeatureAccess))
                    {
                        base.OnActionExecuting(filterContext);
                        return;
                    }
                }

                filterContext.Result = new UnauthorizedResult();
            }
        }
    }
}
