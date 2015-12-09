using System;
using System.Web.Mvc;
using MySeenWeb.Models;
using Microsoft.AspNet.Identity;
using MySeenLib;
using MySeenWeb.ActionFilters;
using MySeenWeb.Models.TablesLogic;
using MySeenWeb.Models.Tools;

namespace MySeenWeb.Controllers
{
    //[RequireHttps]
    public class HomeController : BaseController
    {
        [BrowserActionFilter]
        public ActionResult Index(string search, int? page)
        {
            LogSave.Save(User.Identity.IsAuthenticated ? User.Identity.GetUserId() : "", Request.UserHostAddress, Request.UserAgent, "Home/Index");

            //if (!User.Identity.IsAuthenticated) return View();
            return View(new HomeViewModel(
                ReadUserSideStorage(UserSideStorageKeys.HomeCategory, Defaults.CategoryBase.Indexes.Films).ToString(),
                User.Identity.IsAuthenticated?User.Identity.GetUserId():string.Empty,
                page ?? 1,
                Rpp,
                ReadUserSideStorage(UserSideStorageKeys.ImprovementsCategory, Defaults.ComplexBase.Indexes.All),
                search,
                MarkersOnRoads,
                ReadUserSideStorage(UserSideStorageKeys.RoadsYear, 0)
                ));
        }
        [Authorize]
        [HttpPost]
        public JsonResult ChangeCookies(string selected)
        {
            WriteUserSideStorage(UserSideStorageKeys.HomeCategory, selected);
            WriteUserSideStorage(UserSideStorageKeys.HomeCategoryPrev, selected);
            return Json(new { success = true });
        }
        [Authorize]
        [HttpPost]
        public JsonResult ChangeCookiesImprovement(string selected)
        {
            WriteUserSideStorage(UserSideStorageKeys.ImprovementsCategory, selected);
            return Json(new { success = true });
        }
        [Authorize]
        [HttpPost]
        public JsonResult ChangeCookiesRoads(string selected)
        {
            WriteUserSideStorage(UserSideStorageKeys.RoadsYear, selected);
            return Json(new { success = true });
        }
        [Authorize]
        [HttpPost]
        public JsonResult AddFilm(string name, string year, string datetime, string genre, string rating)
        {
            LogSave.Save(User.Identity.IsAuthenticated ? User.Identity.GetUserId() : "", Request.UserHostAddress, Request.UserAgent, "Home/AddFilm", name);

            var logic = new FilmsLogic();
            return !logic.Add(name, year, datetime, genre, rating, User.Identity.GetUserId()) ? new JsonResult { Data = new { success = false, error = logic.ErrorMessage } } : Json(new { success = true });
        }
        [Authorize]
        [HttpPost]
        public JsonResult EditFilm(string id, string name, string year, string datetime, string genre, string rating)
        {
            LogSave.Save(User.Identity.IsAuthenticated ? User.Identity.GetUserId() : "", Request.UserHostAddress, Request.UserAgent, "Home/EditFilm", id);

            var logic = new FilmsLogic();
            return !logic.Update(id, name, year, datetime, genre, rating, User.Identity.GetUserId()) ? new JsonResult { Data = new { success = false, error = logic.ErrorMessage } } : Json(new { success = true });
        }
        [Authorize]
        [HttpPost]
        public JsonResult AddSerial(string name, string year, string season, string series, string datetime, string genre, string rating)
        {
            LogSave.Save(User.Identity.IsAuthenticated ? User.Identity.GetUserId() : "", Request.UserHostAddress, Request.UserAgent, "Home/AddSerial", name);

            var logic=new SerialsLogic();
            return !logic.Add(name, year, season, series, datetime, genre, rating, User.Identity.GetUserId()) ? new JsonResult { Data = new { success = false, error = logic.ErrorMessage } } : Json(new { success = true });
        }
        [Authorize]
        [HttpPost]
        public JsonResult EditSerial(string id, string name, string year, string season, string series, string datetime, string genre, string rating)
        {
            LogSave.Save(User.Identity.IsAuthenticated ? User.Identity.GetUserId() : "", Request.UserHostAddress, Request.UserAgent, "Home/EditSerial", id);

            var logic = new SerialsLogic();
            return !logic.Update(id, name, year, season, series, datetime, genre, rating, User.Identity.GetUserId()) ? new JsonResult { Data = new { success = false, error = logic.ErrorMessage } } : Json(new { success = true });
        }
        [Authorize]
        [HttpPost]
        public JsonResult AddBook(string name, string year, string authors, string datetime, string genre, string rating)
        {
            LogSave.Save(User.Identity.IsAuthenticated ? User.Identity.GetUserId() : "", Request.UserHostAddress, Request.UserAgent, "Home/AddBook", name);

            var logic = new BooksLogic();
            return !logic.Add(name, year, authors, datetime, genre, rating, User.Identity.GetUserId()) ? new JsonResult { Data = new { success = false, error = logic.ErrorMessage } } : Json(new { success = true });
        }
        [Authorize]
        [HttpPost]
        public JsonResult AddEvent(string name, string datetime, string type)
        {
            LogSave.Save(User.Identity.IsAuthenticated ? User.Identity.GetUserId() : "", Request.UserHostAddress, Request.UserAgent, "Home/AddEvent", name);
            var logic = new EventsLogic();
            return !logic.Add(name, datetime, type, User.Identity.GetUserId()) ? new JsonResult { Data = new { success = false, error = logic.ErrorMessage } } : Json(new { success = true });
        }
        [Authorize]
        [HttpPost]
        public JsonResult EditEvent(string id, string name, string datetime, string type)
        {
            LogSave.Save(User.Identity.IsAuthenticated ? User.Identity.GetUserId() : "", Request.UserHostAddress, Request.UserAgent, "Home/EditEvent", id);

            var logic = new EventsLogic();
            return !logic.Update(id, name, datetime, type, User.Identity.GetUserId())
                ? new JsonResult { Data = new { success = false, error = logic.ErrorMessage } } : Json(new { success = true });
        }
        [Authorize]
        [HttpPost]
        public JsonResult DeleteEvent(string id)
        {
            LogSave.Save(User.Identity.IsAuthenticated ? User.Identity.GetUserId() : "", Request.UserHostAddress, Request.UserAgent, "Home/DeleteEvent", id);

            var logic = new EventsLogic();
            return !logic.Delete(id, User.Identity.GetUserId()) ? new JsonResult { Data = new { success = false, error = logic.ErrorMessage } } : Json(new { success = true });
        }
        [Authorize]
        [HttpPost]
        public JsonResult EditBook(string id, string name, string year, string authors, string datetime, string genre, string rating)
        {
            LogSave.Save(User.Identity.IsAuthenticated ? User.Identity.GetUserId() : "", Request.UserHostAddress, Request.UserAgent, "Home/EditBook", id);

            var logic = new BooksLogic();
            return !logic.Update(id, name, year, authors, datetime, genre, rating, User.Identity.GetUserId())
                ? new JsonResult { Data = new { success = false, error = logic.ErrorMessage } } : Json(new { success = true });
        }
        [Authorize]
        [HttpPost]
        public JsonResult DeleteFilm(string id)
        {
            LogSave.Save(User.Identity.IsAuthenticated ? User.Identity.GetUserId() : "", Request.UserHostAddress, Request.UserAgent, "Home/DeleteFilm", id);
           
            var logic= new FilmsLogic();
            return !logic.MarkDeleted(id, User.Identity.GetUserId()) ? new JsonResult { Data = new { success = false, error = logic.ErrorMessage } } : Json(new { success = true });
        }
        [Authorize]
        [HttpPost]
        public JsonResult DeleteSerial(string id)
        {
            LogSave.Save(User.Identity.IsAuthenticated ? User.Identity.GetUserId() : "", Request.UserHostAddress, Request.UserAgent, "Home/DeleteSerial", id);

            var logic = new SerialsLogic();
            return !logic.MarkDeleted(id, User.Identity.GetUserId()) ? new JsonResult { Data = new { success = false, error = logic.ErrorMessage } } : Json(new { success = true });
        }
        [Authorize]
        [HttpPost]
        public JsonResult DeleteBook(string id)
        {
            LogSave.Save(User.Identity.IsAuthenticated ? User.Identity.GetUserId() : "", Request.UserHostAddress, Request.UserAgent, "Home/DeleteBook", id);

            var logic = new BooksLogic();
            return !logic.MarkDeleted(id, User.Identity.GetUserId()) ? new JsonResult { Data = new { success = false, error = logic.ErrorMessage } } : Json(new { success = true });
        }
        [Authorize]
        [HttpPost]
        public JsonResult AddImprovement(string desc, string complex)
        {
            LogSave.Save(User.Identity.IsAuthenticated ? User.Identity.GetUserId() : "", Request.UserHostAddress, Request.UserAgent, "Home/AddImprovement", desc);

            var logic = new ImprovementLogic();
            return !logic.Add(desc, complex, User.Identity.GetUserId()) ? new JsonResult { Data = new { success = false, error = logic.ErrorMessage } } : Json(new { success = true });
        }
        [Authorize]
        [HttpPost]
        public JsonResult EndImprovement(string id, string desc, string version)
        {
            LogSave.Save(User.Identity.IsAuthenticated ? User.Identity.GetUserId() : "", Request.UserHostAddress, Request.UserAgent, "Home/EndImprovement", id + " " + desc + " " + version);

            var logic = new ImprovementLogic();
            return !logic.Update(id, desc, version, User.Identity.GetUserId()) ? new JsonResult { Data = new { success = false, error = logic.ErrorMessage } } : Json(new { success = true });
        }
        [Authorize]
        [HttpPost]
        public JsonResult DeleteImprovement(string id)
        {
            LogSave.Save(User.Identity.IsAuthenticated ? User.Identity.GetUserId() : "", Request.UserHostAddress, Request.UserAgent, "Home/DeleteImprovement", id);

            var logic = new ImprovementLogic();
            return !logic.Delete(id, User.Identity.GetUserId()) ? new JsonResult { Data = new { success = false, error = logic.ErrorMessage } } : Json(new { success = true });
        }
        [Authorize]
        [HttpPost]
        public JsonResult AddTrack(string name, string datetime, string type, string coordinates, string distance)
        {
            LogSave.Save(User.Identity.IsAuthenticated ? User.Identity.GetUserId() : "", Request.UserHostAddress, Request.UserAgent, "Home/AddTrack", name);

            var logic = new TracksLogic();
            return !logic.Add(name, datetime, type, coordinates, distance, User.Identity.GetUserId()) ? new JsonResult { Data = new { success = false, error = logic.ErrorMessage } } : Json(new { success = true });
        }
        [Authorize]
        [HttpPost]
        public JsonResult EditTrack(string id, string name, string datetime, string type, string coordinates, string distance)
        {
            LogSave.Save(User.Identity.IsAuthenticated ? User.Identity.GetUserId() : "", Request.UserHostAddress, Request.UserAgent, "Home/EditTrack", name);

            var logic = new TracksLogic();
            return !logic.Update(id, name, datetime, type, coordinates, distance, User.Identity.GetUserId()) ? new JsonResult { Data = new { success = false, error = logic.ErrorMessage } } : Json(new { success = true });
        }
        [Authorize]
        public ActionResult GetTrack(int id)
        {
            return Json(HomeViewModelTracks.GetTrack(id, User.Identity.GetUserId()), JsonRequestBehavior.AllowGet);
        }
        //[Authorize]
        public ActionResult GetTrackByKey(string id)
        {
            return Json(HomeViewModelTracks.GetTrackByKey(id, ReadUserSideStorage(UserSideStorageKeys.RoadsYear, 0)), JsonRequestBehavior.AllowGet);
        }
        [Authorize]
        public ActionResult GetTrackByType(string id)
        {
            return Json(HomeViewModelTracks.GetTrackByType(id, ReadUserSideStorage(UserSideStorageKeys.RoadsYear, 0)), JsonRequestBehavior.AllowGet);
        }
        [Authorize]
        public ActionResult GetTrackNameById(int id)
        {
            return Json(HomeViewModelTracks.GetTrackNameById(id, User.Identity.GetUserId()), JsonRequestBehavior.AllowGet);
        }
        [Authorize]
        public ActionResult GetTrackDateById(int id)
        {
            return Json(HomeViewModelTracks.GetTrackDateById(id, User.Identity.GetUserId()), JsonRequestBehavior.AllowGet);
        }
        [Authorize]
        public ActionResult GetTrackShare(string id)
        {
            return Json(HomeViewModelTracks.GetTrackShare(id, User.Identity.GetUserId()), JsonRequestBehavior.AllowGet);
        }
        [Authorize]
        public ActionResult GetEventShare(string id)
        {
            return Json(HomeViewModelEvents.GetShare(id, User.Identity.GetUserId()), JsonRequestBehavior.AllowGet);
        }
        [Authorize]
        public ActionResult GenerateEventShare(string id)
        {
            return Json(HomeViewModelEvents.GenerateShare(id, User.Identity.GetUserId()), JsonRequestBehavior.AllowGet);
        }
        [Authorize]
        public ActionResult DeleteEventShare(string id)
        {
            HomeViewModelEvents.DeleteShare(id, User.Identity.GetUserId());
            //return Json(new { success = true }, JsonRequestBehavior.AllowGet);
            return Json(HomeViewModelEvents.DeleteShare(id, User.Identity.GetUserId()), JsonRequestBehavior.AllowGet);
        }
        [Authorize]
        public ActionResult GenerateTrackShare(string id)
        {
            return Json(HomeViewModelTracks.GenerateTrackShare(id, User.Identity.GetUserId()), JsonRequestBehavior.AllowGet);
        }
        [Authorize]
        public ActionResult DeleteTrackShare(string id)
        {
            HomeViewModelTracks.DeleteTrackShare(id, User.Identity.GetUserId());
            return Json(new { success = true }, JsonRequestBehavior.AllowGet);
        }
        [Authorize]
        public ActionResult DeleteTrack(string id)
        {
            HomeViewModelTracks.DeleteTrack(id, User.Identity.GetUserId());
            return Json(new { success = true }, JsonRequestBehavior.AllowGet);
        }
        [Authorize]
        public ActionResult GetTrackCoordinatesById(int id)
        {
            return Json(HomeViewModelTracks.GetTrackCoordinatesById(id, User.Identity.GetUserId()), JsonRequestBehavior.AllowGet);
        }
        [BrowserActionFilter]
        public ActionResult Home()
        {
            WriteUserSideStorage(UserSideStorageKeys.HomeCategory, ReadUserSideStorage(UserSideStorageKeys.HomeCategoryPrev, Defaults.CategoryBase.Indexes.Films));
            return RedirectToAction("Index");
        }
        [BrowserActionFilter]
        [Authorize]
        public ActionResult Logs()
        {
            if (!HomeViewModel.IsCategoryExt(ReadUserSideStorage(UserSideStorageKeys.HomeCategory, Defaults.CategoryBase.Indexes.Films)))
                WriteUserSideStorage(UserSideStorageKeys.HomeCategoryPrev, ReadUserSideStorage(UserSideStorageKeys.HomeCategory, Defaults.CategoryBase.Indexes.Films));
            WriteUserSideStorage(UserSideStorageKeys.HomeCategory, (int)HomeViewModel.CategoryExt.Logs);
            return RedirectToAction("Index");
        }
        [BrowserActionFilter]
        [Authorize]
        public ActionResult Users()
        {
            if (!HomeViewModel.IsCategoryExt(ReadUserSideStorage(UserSideStorageKeys.HomeCategory, Defaults.CategoryBase.Indexes.Films)))
                WriteUserSideStorage(UserSideStorageKeys.HomeCategoryPrev, ReadUserSideStorage(UserSideStorageKeys.HomeCategory, Defaults.CategoryBase.Indexes.Films));

            WriteUserSideStorage(UserSideStorageKeys.HomeCategory, (int)HomeViewModel.CategoryExt.Users);
            return RedirectToAction("Index");
        }
        [BrowserActionFilter]
        [Authorize]
        public ActionResult Improvements()
        {
            if (!HomeViewModel.IsCategoryExt(ReadUserSideStorage(UserSideStorageKeys.HomeCategory, Defaults.CategoryBase.Indexes.Films)))
                WriteUserSideStorage(UserSideStorageKeys.HomeCategoryPrev, ReadUserSideStorage(UserSideStorageKeys.HomeCategory, Defaults.CategoryBase.Indexes.Films));
            WriteUserSideStorage(UserSideStorageKeys.HomeCategory, (int)HomeViewModel.CategoryExt.Improvements);
            return RedirectToAction("Index");
        }
        [BrowserActionFilter]
        [Authorize]
        public ActionResult TrackEditor(string id)
        {
            LogSave.Save(User.Identity.IsAuthenticated ? User.Identity.GetUserId() : "", Request.UserHostAddress, Request.UserAgent, "Home/TrackEditor");
            return View(new HomeViewModelTrackEditor(id, User.Identity.GetUserId()));
        }
        
    }
}