using System;
using System.Linq;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using MySeenLib;
using MySeenWeb.Add_Code.Services.Logging.NLog;
using MySeenWeb.Controllers._Base;
using MySeenWeb.Models.OtherViewModels;
using MySeenWeb.Models.Tools;

namespace MySeenWeb.Controllers.Home
{
    public class SettingsController : BaseController
    {
        [Authorize]
        [HttpPost]
        public JsonResult SetLanguage(int val)
        {
            var logger = new NLogLogger();
            const string methodName = "public JsonResult SetLanguage(int language)";
            try
            {
                var ac = new ApplicationDbContext();
                var userId = User.Identity.GetUserId();
                ac.Users.First(u => u.Id == userId).Culture = Defaults.Languages.GetValDb(val);
                ac.SaveChanges();
                CultureInfoTool.SetCulture(Defaults.Languages.GetValDb(val));
                WriteUserSideStorage(UserSideStorageKeys.Language, val);
                Defaults.ReloadResources();
                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                logger.Error(methodName, ex);
            }
            return new JsonResult { Data = new { success = false, error = methodName } };
        }
        [Authorize]
        [HttpPost]
        public JsonResult SetRpp(int val)
        {
            var logger = new NLogLogger();
            const string methodName = "public JsonResult SetRpp(int rpp)";
            try
            {
                var ac = new ApplicationDbContext();
                var userId = User.Identity.GetUserId();
                ac.Users.First(u => u.Id == userId).RecordPerPage = val;
                ac.SaveChanges();
                Rpp = val;
                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                logger.Error(methodName, ex);
            }
            return new JsonResult { Data = new { success = false, error = methodName } };
        }
        [Authorize]
        [HttpPost]
        public JsonResult SetMor(int val)
        {
            var logger = new NLogLogger();
            const string methodName = "public JsonResult SetMor(int mor)";
            try
            {
                var ac = new ApplicationDbContext();
                var userId = User.Identity.GetUserId();
                ac.Users.First(u => u.Id == userId).MarkersOnRoads = val;
                ac.SaveChanges();
                MarkersOnRoads = val;
                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                logger.Error(methodName, ex);
            }
            return new JsonResult { Data = new { success = false, error = methodName } };
        }
        [Authorize]
        [HttpPost]
        public JsonResult SetVkService(int val)
        {
            var logger = new NLogLogger();
            const string methodName = "public JsonResult SetVkService(int val)";
            try
            {
                var ac = new ApplicationDbContext();
                var userId = User.Identity.GetUserId();
                ac.Users.First(u => u.Id == userId).VkServiceEnabled = val == (int)Defaults.EnabledDisabledBase.Indexes.Enabled;
                ac.SaveChanges();
                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                logger.Error(methodName, ex);
            }
            return new JsonResult { Data = new { success = false, error = methodName } };
        }
        [Authorize]
        [HttpPost]
        public JsonResult SetGoogleService(int val)
        {
            var logger = new NLogLogger();
            const string methodName = "public JsonResult SetGoogleService(int val)";
            try
            {
                var ac = new ApplicationDbContext();
                var userId = User.Identity.GetUserId();
                ac.Users.First(u => u.Id == userId).GoogleServiceEnabled = val == (int)Defaults.EnabledDisabledBase.Indexes.Enabled;
                ac.SaveChanges();
                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                logger.Error(methodName, ex);
            }
            return new JsonResult { Data = new { success = false, error = methodName } };
        }
        [Authorize]
        [HttpPost]
        public JsonResult SetFacebookService(int val)
        {
            var logger = new NLogLogger();
            const string methodName = "public JsonResult SetFacebookService(int val)";
            try
            {
                var ac = new ApplicationDbContext();
                var userId = User.Identity.GetUserId();
                ac.Users.First(u => u.Id == userId).FacebookServiceEnabled = val == (int)Defaults.EnabledDisabledBase.Indexes.Enabled;
                ac.SaveChanges();
                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                logger.Error(methodName, ex);
            }
            return new JsonResult { Data = new { success = false, error = methodName } };
        }
    }
}