using Lykke.Common.Log;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;

namespace LykkeMarketMakers.Web.Filters
{
    public class HandleAllExceptionsAttribute : ExceptionFilterAttribute
    {
        public override void OnException(ExceptionContext filterContext)
        {
            IModelMetadataProvider modelMetadataProvider = filterContext.HttpContext.RequestServices.GetService<IModelMetadataProvider>();
            ILog log = filterContext.HttpContext.RequestServices.GetService<ILog>();

            var controllerName = (string)filterContext.RouteData.Values["controller"];
            var actionName = (string)filterContext.RouteData.Values["action"];

            var context = filterContext.HttpContext.Connection.RemoteIpAddress + ": " +
                          filterContext.HttpContext.User.Identity.Name;

            string parameters = filterContext.HttpContext.Request.QueryString.HasValue
                ? filterContext.HttpContext.Request.QueryString.ToString()
                : JsonConvert.SerializeObject(filterContext.HttpContext.Request.Form);

            log.WriteErrorAsync(controllerName + '/' + actionName, parameters, context, filterContext.Exception);

            var result = new ViewResult
            {
                ViewName = "CustomError",
                ViewData = new ViewDataDictionary(modelMetadataProvider, filterContext.ModelState)
                {
                    {"Exception", filterContext.Exception}
                }
            };
            // TODO: Pass additional detailed data via ViewData
            filterContext.ExceptionHandled = true; // mark exception as handled
            filterContext.Result = result;
        }

    }
}