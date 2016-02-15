using System;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using MySeenLib;
using MySeenWeb.ActionFilters;
using MySeenWeb.Add_Code.Services.Logging.NLog;
using MySeenWeb.Controllers._Base;
using MySeenWeb.Models;
using MySeenWeb.Models.TablesLogic;
using MySeenWeb.Models.Tools;

namespace MySeenWeb.Controllers.Home
{
    //[RequireHttps]
    public class HomeController : BaseController
    {
        [BrowserActionFilter]
        public ActionResult Index()
        {
            var logger = new NLogLogger();
            const string methodName = "public ActionResult Index()";
            try
            {
                return View(new HomeViewModel(ReadUserSideStorage(UserSideStorageKeys.MarkersOnRoads, 0)));
            }
            catch (Exception ex)
            {
                logger.Error(methodName, ex);
            }
            return null;
        }










        //Переписать
        [Authorize]
        [HttpPost]
        public JsonResult EndImprovement(string id, string desc, string version)
        {
            var logger = new NLogLogger();
            const string methodName = "public JsonResult EndImprovement(string id, string desc, string version)";
            try
            {
                if (Admin.IsAdmin(User.Identity.Name))
                {
                    var logic = new ImprovementLogic();
                    return !logic.Update(id, desc, version, User.Identity.GetUserId())
                        ? new JsonResult {Data = new {success = false, error = logic.ErrorMessage}}
                        : Json(new {success = true});
                }
            }
            catch (Exception ex)
            {
                logger.Error(methodName, ex);
            }
            return new JsonResult { Data = new { success = false, error = methodName } };
        }
        [Authorize]
        [HttpPost]
        public JsonResult DeleteImprovement(string id)
        {
            var logger = new NLogLogger();
            const string methodName = "public JsonResult DeleteImprovement(string id)";
            try
            {
                var logic = new ImprovementLogic();
                return !logic.Delete(id, User.Identity.GetUserId()) ? new JsonResult { Data = new { success = false, error = logic.ErrorMessage } } : Json(new { success = true });
            }
            catch (Exception ex)
            {
                logger.Error(methodName, ex);
            }
            return new JsonResult { Data = new { success = false, error = methodName } };
        }
        [Authorize]
        [HttpPost]
        public ActionResult DeleteNLog(string id)
        {
            var logger = new NLogLogger();
            const string methodName = "public ActionResult DeleteNLog(string id)";
            try
            {
                if (Admin.IsAdmin(User.Identity.Name))
                {
                    var logic = new NLogErrorsLogic();
                    return !logic.Delete(id, User.Identity.GetUserId())
                        ? new JsonResult {Data = new {success = false, error = logic.ErrorMessage}}
                        : Json(new {success = true});
                }
            }
            catch (Exception ex)
            {
                logger.Error(methodName, ex);
            }
            return new JsonResult { Data = new { success = false, error = methodName } };
        }
        [BrowserActionFilter]
        [Authorize]
        public ActionResult TrackEditor(string id)
        {
            var logger = new NLogLogger();
            const string methodName = "public ActionResult TrackEditor(string id)";
            try
            {
                return View(new HomeViewModelTrackEditor(id, User.Identity.GetUserId()));
            }
            catch (Exception ex)
            {
                logger.Error(methodName, ex);
            }
            return new JsonResult { Data = new { success = false, error = methodName } };
        }
    }
}