using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using MySeenResources;
using MySeenWeb.Models.TablesLogic;

namespace MySeenWeb.ActionFilters
{
    public class IsAdmin : ActionFilterAttribute, IActionFilter
    {
        void IActionFilter.OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (!UserRolesLogic.IsAdmin(HttpContext.Current.User.Identity.GetUserId()))
            {
                filterContext.Result = new JsonResult { Data = new { success = false, error = Resource.NoRights } };
            }
            OnActionExecuting(filterContext);
        }
    }
}