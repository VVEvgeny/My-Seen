using System.Web;
using System.Web.Mvc;
using MySeenLib;
using Microsoft.AspNet.Identity;
using MySeenWeb.Add_Code.Services.Logging.NLog;

namespace MySeenWeb.ActionFilters
{
    public class IsAdmin : ActionFilterAttribute, IActionFilter
    {
        void IActionFilter.OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (!Admin.IsAdmin(HttpContext.Current.User.Identity.GetUserName()))
            {
                var logger = new NLogLogger();
                logger.Warn("CALL NOT ALLOWED UserName=" + HttpContext.Current.User.Identity.GetUserName());

                filterContext.Result = new JsonResult { Data = new { success = false, error = Resource.NoRights } };
            }
            OnActionExecuting(filterContext);
        }
    }
}