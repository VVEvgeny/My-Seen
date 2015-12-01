using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using MySeenWeb.ActionFilters;
using MySeenWeb.Models;
using MySeenWeb.Models.Tools;

namespace MySeenWeb.Controllers
{
    //[RequireHttps]
    public class ShareController : BaseController
    {
        [BrowserActionFilter]
        public ActionResult Tracks(string id)
        {
            LogSave.Save(User.Identity.IsAuthenticated ? User.Identity.GetUserId() : "", Request.UserHostAddress, Request.UserAgent, "Share/Tracks", id);
            var model = new ShareViewModelTracks(id,MarkersOnRoads);
            return View(model);
        }
    }
}