using System.Web.Mvc;
using System.Web.Routing;
using MySeenWeb.Add_Code;

namespace MySeenWeb.ActionFilters
{
    public class BrowserActionFilterAttribute : ActionFilterAttribute, IActionFilter
    {
        void IActionFilter.OnActionExecuting(ActionExecutingContext filterContext)
        {
            var browserName = filterContext.RequestContext.HttpContext.Request.Browser.Browser.ToLowerInvariant();
            var majorVersion = filterContext.RequestContext.HttpContext.Request.Browser.MajorVersion;

            if (browserName != BrowserTypes.Chrome.ToString().ToLowerInvariant()
                && browserName != BrowserTypes.Firefox.ToString().ToLowerInvariant()
                && browserName != BrowserTypes.Opera.ToString().ToLowerInvariant()
                && browserName != BrowserTypes.Safari.ToString().ToLowerInvariant()
                && ((browserName != BrowserTypes.Ie.ToString().ToLowerInvariant() && browserName != BrowserTypes.InternetExplorer.ToString().ToLowerInvariant()) || majorVersion < 9))
                filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary(new { controller = "BrowserFilter" }));

            OnActionExecuting(filterContext);
        }
    }
}