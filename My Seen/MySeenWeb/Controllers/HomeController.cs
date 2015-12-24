using System;
using System.Web.Mvc;
using MySeenWeb.Models;
using Microsoft.AspNet.Identity;
using MySeenLib;
using MySeenWeb.ActionFilters;
using MySeenWeb.Add_Code.Services.Logging.NLog;
using MySeenWeb.Models.TablesLogic;
using MySeenWeb.Models.Tools;

namespace MySeenWeb.Controllers
{
    //[RequireHttps]
    public class HomeController : BaseController
    {
        [BrowserActionFilter]
        public ActionResult Index()
        {
            var logger = new NLogLogger();
            var methodName = "public ActionResult Index(string search, int? page)";
            try
            {
                return View(new HomeViewModel(
                    ReadUserSideStorage(UserSideStorageKeys.HomeCategory, Defaults.CategoryBase.Indexes.Films)
                        .ToString(),
                    User.Identity.IsAuthenticated ? User.Identity.GetUserId() : string.Empty,
                    ReadUserSideStorage(UserSideStorageKeys.ImprovementsCategory, Defaults.ComplexBase.Indexes.All),
                    MarkersOnRoads,
                    ReadUserSideStorage(UserSideStorageKeys.RoadsYear, 0),
                    ReadUserSideStorage(UserSideStorageKeys.EndedEvents, 0) == 1
                    ));
            }
            catch (Exception ex)
            {
                logger.Error(methodName, ex);
            }
            return null;
        }

        [Authorize]
        [HttpPost]
        public JsonResult ChangeCookies(string selected)
        {
            var logger = new NLogLogger();
            var methodName = "public JsonResult ChangeCookies(string selected)";
            try
            {
                WriteUserSideStorage(UserSideStorageKeys.HomeCategory, selected);
                WriteUserSideStorage(UserSideStorageKeys.HomeCategoryPrev, selected);
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
        public JsonResult ChangeCookiesImprovement(string selected)
        {
            var logger = new NLogLogger();
            var methodName = "public JsonResult ChangeCookiesImprovement(string selected)";
            try
            {
                WriteUserSideStorage(UserSideStorageKeys.ImprovementsCategory, selected);
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
        public JsonResult ChangeCookiesRoads(string selected)
        {
            var logger = new NLogLogger();
            var methodName = "public JsonResult ChangeCookiesRoads(string selected)";
            try
            {
                WriteUserSideStorage(UserSideStorageKeys.RoadsYear, selected);
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
        public JsonResult AddFilm(string name, string year, string datetime, string genre, string rating)
        {
            var logger = new NLogLogger();
            var methodName = "public JsonResult AddFilm(string name, string year, string datetime, string genre, string rating)";
            try
            {
                var logic = new FilmsLogic();
                return !logic.Add(name, year, datetime, genre, rating, User.Identity.GetUserId()) ? new JsonResult { Data = new { success = false, error = logic.ErrorMessage } } : Json(new { success = true });
            }
            catch (Exception ex)
            {
                logger.Error(methodName, ex);
            }
            return new JsonResult { Data = new { success = false, error = methodName } };
        }
        [Authorize]
        [HttpPost]
        public JsonResult EditFilm(string id, string name, string year, string datetime, string genre, string rating)
        {
            var logger = new NLogLogger();
            var methodName = "public JsonResult EditFilm(string id, string name, string year, string datetime, string genre, string rating)";
            try
            {
                var logic = new FilmsLogic();
                return !logic.Update(id, name, year, datetime, genre, rating, User.Identity.GetUserId()) ? new JsonResult { Data = new { success = false, error = logic.ErrorMessage } } : Json(new { success = true });
            }
            catch (Exception ex)
            {
                logger.Error(methodName, ex);
            }
            return new JsonResult { Data = new { success = false, error = methodName } };
        }
        [Authorize]
        [HttpPost]
        public JsonResult AddSerial(string name, string year, string season, string series, string datetime, string genre, string rating)
        {
            var logger = new NLogLogger();
            var methodName = "public JsonResult AddSerial(string name, string year, string season, string series, string datetime, string genre, string rating)";
            try
            {
                var logic = new SerialsLogic();
                return !logic.Add(name, year, season, series, datetime, genre, rating, User.Identity.GetUserId()) ? new JsonResult { Data = new { success = false, error = logic.ErrorMessage } } : Json(new { success = true });
            }
            catch (Exception ex)
            {
                logger.Error(methodName, ex);
            }
            return new JsonResult { Data = new { success = false, error = methodName } };
        }
        [Authorize]
        [HttpPost]
        public JsonResult EditSerial(string id, string name, string year, string season, string series, string datetime, string genre, string rating)
        {
            var logger = new NLogLogger();
            var methodName = "public JsonResult EditSerial(string id, string name, string year, string season, string series, string datetime, string genre, string rating)";
            try
            {
                var logic = new SerialsLogic();
                return !logic.Update(id, name, year, season, series, datetime, genre, rating, User.Identity.GetUserId()) ? new JsonResult { Data = new { success = false, error = logic.ErrorMessage } } : Json(new { success = true });
            }
            catch (Exception ex)
            {
                logger.Error(methodName, ex);
            }
            return new JsonResult { Data = new { success = false, error = methodName } };
        }
        [Authorize]
        [HttpPost]
        public JsonResult AddBook(string name, string year, string authors, string datetime, string genre, string rating)
        {
            var logger = new NLogLogger();
            const string methodName = "public JsonResult AddBook(string name, string year, string authors, string datetime, string genre, string rating)";
            try
            {
                var logic = new BooksLogic();
                return !logic.Add(name, year, authors, datetime, genre, rating, User.Identity.GetUserId()) ? new JsonResult { Data = new { success = false, error = logic.ErrorMessage } } : Json(new { success = true });
            }
            catch (Exception ex)
            {
                logger.Error(methodName, ex);
            }
            return new JsonResult { Data = new { success = false, error = methodName } };
        }
        [Authorize]
        [HttpPost]
        public JsonResult AddEvent(string name, string datetime, string type)
        {
            var logger = new NLogLogger();
            const string methodName = "public JsonResult AddEvent(string name, string datetime, string type)";
            try
            {
                var logic = new EventsLogic();
                return !logic.Add(name, datetime, type, User.Identity.GetUserId()) ? new JsonResult { Data = new { success = false, error = logic.ErrorMessage } } : Json(new { success = true });
            }
            catch (Exception ex)
            {
                logger.Error(methodName, ex);
            }
            return new JsonResult { Data = new { success = false, error = methodName } };
        }
        [Authorize]
        [HttpPost]
        public JsonResult EditEvent(string id, string name, string datetime, string type)
        {
            var logger = new NLogLogger();
            const string methodName = "public JsonResult EditEvent(string id, string name, string datetime, string type)";
            try
            {
                var logic = new EventsLogic();
                return !logic.Update(id, name, datetime, type, User.Identity.GetUserId())
                    ? new JsonResult { Data = new { success = false, error = logic.ErrorMessage } } : Json(new { success = true });
            }
            catch (Exception ex)
            {
                logger.Error(methodName, ex);
            }
            return new JsonResult { Data = new { success = false, error = methodName } };
        }
        [Authorize]
        [HttpPost]
        public JsonResult DeleteEvent(string id)
        {
            var logger = new NLogLogger();
            const string methodName = "public JsonResult DeleteEvent(string id)";
            try
            {
                var logic = new EventsLogic();
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
        public JsonResult EditBook(string id, string name, string year, string authors, string datetime, string genre, string rating)
        {
            var logger = new NLogLogger();
            const string methodName = "public JsonResult EditBook(string id, string name, string year, string authors, string datetime, string genre, string rating)";
            try
            {
                var logic = new BooksLogic();
                return !logic.Update(id, name, year, authors, datetime, genre, rating, User.Identity.GetUserId())
                    ? new JsonResult { Data = new { success = false, error = logic.ErrorMessage } } : Json(new { success = true });
            }
            catch (Exception ex)
            {
                logger.Error(methodName, ex);
            }
            return new JsonResult { Data = new { success = false, error = methodName } };
        }
        [Authorize]
        [HttpPost]
        public JsonResult DeleteFilm(string id)
        {
            var logger = new NLogLogger();
            const string methodName = "public JsonResult DeleteFilm(string id)";
            try
            {
                var logic = new FilmsLogic();
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
        public JsonResult DeleteSerial(string id)
        {
            var logger = new NLogLogger();
            const string methodName = "public JsonResult DeleteSerial(string id)";
            try
            {
                var logic = new SerialsLogic();
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
        public JsonResult DeleteBook(string id)
        {
            var logger = new NLogLogger();
            const string methodName = "public JsonResult DeleteBook(string id)";
            try
            {
                var logic = new BooksLogic();
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
        public JsonResult AddImprovement(string desc, string complex)
        {
            var logger = new NLogLogger();
            const string methodName = "public JsonResult AddImprovement(string desc, string complex)";
            try
            {
                var logic = new ImprovementLogic();
                return !logic.Add(desc, complex, User.Identity.GetUserId()) ? new JsonResult { Data = new { success = false, error = logic.ErrorMessage } } : Json(new { success = true });
            }
            catch (Exception ex)
            {
                logger.Error(methodName, ex);
            }
            return new JsonResult { Data = new { success = false, error = methodName } };
        }
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
        public JsonResult AddTrack(string name, string datetime, string type, string coordinates, string distance)
        {
            var logger = new NLogLogger();
            const string methodName = "public JsonResult AddTrack(string name, string datetime, string type, string coordinates, string distance)";
            try
            {
                var logic = new TracksLogic();
                return !logic.Add(name, datetime, type, coordinates, distance, User.Identity.GetUserId()) ? new JsonResult { Data = new { success = false, error = logic.ErrorMessage } } : Json(new { success = true });
            }
            catch (Exception ex)
            {
                logger.Error(methodName, ex);
            }
            return new JsonResult { Data = new { success = false, error = methodName } };
        }
        [Authorize]
        [HttpPost]
        public JsonResult EditTrack(string id, string name, string datetime, string type, string coordinates, string distance)
        {
            var logger = new NLogLogger();
            const string methodName = "public JsonResult EditTrack(string id, string name, string datetime, string type, string coordinates, string distance)";
            try
            {
                var logic = new TracksLogic();
                return !logic.Update(id, name, datetime, type, coordinates, distance, User.Identity.GetUserId()) ? new JsonResult { Data = new { success = false, error = logic.ErrorMessage } } : Json(new { success = true });
            }
            catch (Exception ex)
            {
                logger.Error(methodName, ex);
            }
            return new JsonResult { Data = new { success = false, error = methodName } };
        }
        [Authorize]
        public ActionResult GetTrack(int id)
        {
            var logger = new NLogLogger();
            const string methodName = "public ActionResult GetTrack(int id)";
            try
            {
                return Json(HomeViewModelTracks.GetTrack(id, User.Identity.GetUserId()), JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                logger.Error(methodName, ex);
            }
            return new JsonResult { Data = new { success = false, error = methodName } };
        }
        //[Authorize]
        public ActionResult GetTrackByKey(string id)
        {
            var logger = new NLogLogger();
            const string methodName = "public ActionResult GetTrackByKey(string id)";
            try
            {
                return Json(HomeViewModelTracks.GetTrackByKey(id, ReadUserSideStorage(UserSideStorageKeys.RoadsYear, 0)), JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                logger.Error(methodName, ex);
            }
            return new JsonResult { Data = new { success = false, error = methodName } };
        }
        [Authorize]
        public ActionResult GetTrackByType(string id)
        {
            var logger = new NLogLogger();
            const string methodName = "public ActionResult GetTrackByType(string id)";
            try
            {
                return Json(HomeViewModelTracks.GetTrackByType(id, ReadUserSideStorage(UserSideStorageKeys.RoadsYear, 0)), JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                logger.Error(methodName, ex);
            }
            return new JsonResult { Data = new { success = false, error = methodName } };
        }
        [Authorize]
        public ActionResult GetTrackNameById(int id)
        {
            var logger = new NLogLogger();
            const string methodName = "public ActionResult GetTrackNameById(int id)";
            try
            {
                return Json(HomeViewModelTracks.GetTrackNameById(id, User.Identity.GetUserId()), JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                logger.Error(methodName, ex);
            }
            return new JsonResult { Data = new { success = false, error = methodName } };
        }
        [Authorize]
        public ActionResult GetTrackDateById(int id)
        {
            var logger = new NLogLogger();
            const string methodName = "public ActionResult GetTrackDateById(int id)";
            try
            {
                return Json(HomeViewModelTracks.GetTrackDateById(id, User.Identity.GetUserId()), JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                logger.Error(methodName, ex);
            }
            return new JsonResult { Data = new { success = false, error = methodName } };
        }
        [Authorize]
        public ActionResult GetTrackShare(string id)
        {
            var logger = new NLogLogger();
            const string methodName = "public ActionResult GetTrackShare(string id)";
            try
            {
                return Json(HomeViewModelTracks.GetTrackShare(id, User.Identity.GetUserId()), JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                logger.Error(methodName, ex);
            }
            return new JsonResult { Data = new { success = false, error = methodName } };
        }
        [Authorize]
        public ActionResult GetEventShare(string id)
        {
            var logger = new NLogLogger();
            const string methodName = "public ActionResult GetEventShare(string id)";
            try
            {
                return Json(HomeViewModelEvents.GetShare(id, User.Identity.GetUserId()), JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                logger.Error(methodName, ex);
            }
            return new JsonResult { Data = new { success = false, error = methodName } };
        }
        [Authorize]
        public ActionResult GenerateEventShare(string id)
        {
            var logger = new NLogLogger();
            const string methodName = "public ActionResult GenerateEventShare(string id)";
            try
            {
                return Json(HomeViewModelEvents.GenerateShare(id, User.Identity.GetUserId()), JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                logger.Error(methodName, ex);
            }
            return new JsonResult { Data = new { success = false, error = methodName } };
        }
        [Authorize]
        public ActionResult DeleteEventShare(string id)
        {
            var logger = new NLogLogger();
            const string methodName = "public ActionResult DeleteEventShare(string id)";
            try
            {
                return Json(HomeViewModelEvents.DeleteShare(id, User.Identity.GetUserId()), JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                logger.Error(methodName, ex);
            }
            return new JsonResult { Data = new { success = false, error = methodName } };
        }
        [Authorize]
        public ActionResult GenerateTrackShare(string id)
        {
            var logger = new NLogLogger();
            const string methodName = "public ActionResult GenerateTrackShare(string id)";
            try
            {
                return Json(HomeViewModelTracks.GenerateTrackShare(id, User.Identity.GetUserId()), JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                logger.Error(methodName, ex);
            }
            return new JsonResult { Data = new { success = false, error = methodName } };
        }
        [Authorize]
        public ActionResult DeleteTrackShare(string id)
        {
            var logger = new NLogLogger();
            const string methodName = "public ActionResult DeleteTrackShare(string id)";
            try
            {
                HomeViewModelTracks.DeleteTrackShare(id, User.Identity.GetUserId());
                return Json(new { success = true }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                logger.Error(methodName, ex);
            }
            return new JsonResult { Data = new { success = false, error = methodName } };
        }
        [Authorize]
        public ActionResult DeleteTrack(string id)
        {
            var logger = new NLogLogger();
            const string methodName = "public ActionResult DeleteTrack(string id)";
            try
            {
                HomeViewModelTracks.DeleteTrack(id, User.Identity.GetUserId());
                return Json(new { success = true }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                logger.Error(methodName, ex);
            }
            return new JsonResult { Data = new { success = false, error = methodName } };
        }
        [Authorize]
        public ActionResult GetTrackCoordinatesById(int id)
        {
            var logger = new NLogLogger();
            const string methodName = "public ActionResult GetTrackCoordinatesById(int id)";
            try
            {
                return Json(HomeViewModelTracks.GetTrackCoordinatesById(id, User.Identity.GetUserId()), JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                logger.Error(methodName, ex);
            }
            return new JsonResult { Data = new { success = false, error = methodName } };
        }
        [BrowserActionFilter]
        public ActionResult Home()
        {
            var logger = new NLogLogger();
            const string methodName = "public ActionResult Home()";
            try
            {
                WriteUserSideStorage(UserSideStorageKeys.HomeCategory, ReadUserSideStorage(UserSideStorageKeys.HomeCategoryPrev, Defaults.CategoryBase.Indexes.Films));
            }
            catch (Exception ex)
            {
                logger.Error(methodName, ex);
            }
            return RedirectToAction("Index");
        }
        [BrowserActionFilter]
        [Authorize]
        public ActionResult Logs()
        {
            var logger = new NLogLogger();
            const string methodName = "public ActionResult Logs()";
            try
            {
                if (!Defaults.CategoryBase.IsCategoryExt(ReadUserSideStorage(UserSideStorageKeys.HomeCategory, Defaults.CategoryBase.Indexes.Films)))
                    WriteUserSideStorage(UserSideStorageKeys.HomeCategoryPrev, ReadUserSideStorage(UserSideStorageKeys.HomeCategory, Defaults.CategoryBase.Indexes.Films));
                WriteUserSideStorage(UserSideStorageKeys.HomeCategory, Defaults.CategoryBase.IndexesExt.Logs);
            }
            catch (Exception ex)
            {
                logger.Error(methodName, ex);
            }
            return RedirectToAction("Index");
        }
        [BrowserActionFilter]
        [Authorize]
        public ActionResult Users()
        {
            var logger = new NLogLogger();
            const string methodName = "public ActionResult Users()";
            try
            {
                if (!Defaults.CategoryBase.IsCategoryExt(ReadUserSideStorage(UserSideStorageKeys.HomeCategory, Defaults.CategoryBase.Indexes.Films)))
                    WriteUserSideStorage(UserSideStorageKeys.HomeCategoryPrev, ReadUserSideStorage(UserSideStorageKeys.HomeCategory, Defaults.CategoryBase.Indexes.Films));
                WriteUserSideStorage(UserSideStorageKeys.HomeCategory, Defaults.CategoryBase.IndexesExt.Users);
            }
            catch (Exception ex)
            {
                logger.Error(methodName, ex);
            }
            return RedirectToAction("Index");
        }
        [BrowserActionFilter]
        [Authorize]
        public ActionResult Improvements()
        {
            var logger = new NLogLogger();
            const string methodName = "public ActionResult Improvements()";
            try
            {
                if (!Defaults.CategoryBase.IsCategoryExt(ReadUserSideStorage(UserSideStorageKeys.HomeCategory, Defaults.CategoryBase.Indexes.Films)))
                    WriteUserSideStorage(UserSideStorageKeys.HomeCategoryPrev, ReadUserSideStorage(UserSideStorageKeys.HomeCategory, Defaults.CategoryBase.Indexes.Films));
                WriteUserSideStorage(UserSideStorageKeys.HomeCategory, Defaults.CategoryBase.IndexesExt.Improvements);
            }
            catch (Exception ex)
            {
                logger.Error(methodName, ex);
            }
            return RedirectToAction("Index");
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