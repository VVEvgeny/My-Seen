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
        public ActionResult Films(string id)
        {
            LogSave.Save(User.Identity.IsAuthenticated ? User.Identity.GetUserId() : "", Request.UserHostAddress, Request.UserAgent, "Share/Films", id);
            var model = new ShareViewModelFilmsMin(id);
            return View(model);
        }

        [BrowserActionFilter]
        public ActionResult Serials(string id)
        {
            LogSave.Save(User.Identity.IsAuthenticated ? User.Identity.GetUserId() : "", Request.UserHostAddress, Request.UserAgent, "Share/Serials", id);
            return View();
        }

        [BrowserActionFilter]
        public ActionResult Books(string id)
        {
            LogSave.Save(User.Identity.IsAuthenticated ? User.Identity.GetUserId() : "", Request.UserHostAddress, Request.UserAgent, "Share/Books", id);
            return View();
        }

        [BrowserActionFilter]
        public ActionResult Events(string id)
        {
            LogSave.Save(User.Identity.IsAuthenticated ? User.Identity.GetUserId() : "", Request.UserHostAddress, Request.UserAgent, "Share/Events", id);
            return View();
        }

        [BrowserActionFilter]
        public ActionResult Tracks(string id)
        {
            LogSave.Save(User.Identity.IsAuthenticated ? User.Identity.GetUserId() : "", Request.UserHostAddress, Request.UserAgent, "Share/Tracks", id);
            var model = new ShareViewModelTracks(id, MarkersOnRoads, ReadUserSideStorage(UserSideStorageKeys.RoadsYear, 0));
            return View(model);
        }
    }
}