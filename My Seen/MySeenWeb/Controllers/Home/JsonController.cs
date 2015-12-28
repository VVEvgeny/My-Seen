using System;
using System.Threading;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using MySeenLib;
using MySeenWeb.Add_Code.Services.Logging.NLog;
using MySeenWeb.Controllers._Base;
using MySeenWeb.Models;
using MySeenWeb.Models.ShareViewModels;
using MySeenWeb.Models.TablesLogic;
using MySeenWeb.Models.Tools;

namespace MySeenWeb.Controllers.Home
{
    public class JsonController : BaseController
    {
        [Authorize]
        [HttpPost]
        public JsonResult GetShare(string name, string id)
        {
            var logger = new NLogLogger();
            const string methodName = "public JsonResult GetShare(string name, string id)";
            try
            {
                switch (name.ToLower())
                {
                    case "films":
                        {
                            var logic = new FilmsLogic();
                            return Json(logic.GetShare(id, User.Identity.GetUserId()));
                        }
                    case "serials":
                        {
                            var logic = new SerialsLogic();
                            return Json(logic.GetShare(id, User.Identity.GetUserId()));
                        }
                    case "books":
                        {
                            var logic = new BooksLogic();
                            return Json(logic.GetShare(id, User.Identity.GetUserId()));
                        }
                    case "events":
                        {
                            var logic = new EventsLogic();
                            return Json(logic.GetShare(id, User.Identity.GetUserId()));
                        }
                }
            }
            catch (Exception ex)
            {
                logger.Error(methodName, ex);
            }
            return Json("-");
        }

        [Authorize]
        [HttpPost]
        public JsonResult GenerateShare(string name, string id)
        {
            var logger = new NLogLogger();
            const string methodName = "public JsonResult GenerateShare(string name, string id)";
            try
            {
                switch (name.ToLower())
                {
                    case "films":
                        {
                            var logic = new FilmsLogic();
                            return Json(logic.GenerateShare(id, User.Identity.GetUserId()));
                        }
                    case "serials":
                        {
                            var logic = new SerialsLogic();
                            return Json(logic.GenerateShare(id, User.Identity.GetUserId()));
                        }
                    case "books":
                        {
                            var logic = new BooksLogic();
                            return Json(logic.GenerateShare(id, User.Identity.GetUserId()));
                        }
                    case "events":
                        {
                            var logic = new EventsLogic();
                            return Json(logic.GenerateShare(id, User.Identity.GetUserId()));
                        }
                }
            }
            catch (Exception ex)
            {
                logger.Error(methodName, ex);
            }
            return Json("-");
        }

        [Authorize]
        [HttpPost]
        public JsonResult DeleteShare(string name, string id)
        {
            var logger = new NLogLogger();
            const string methodName = "public JsonResult DeleteShare(string name, string id)";
            try
            {
                switch (name.ToLower())
                {
                    case "films":
                        {
                            var logic = new FilmsLogic();
                            return Json(logic.DeleteShare(id, User.Identity.GetUserId()));
                        }
                    case "serials":
                        {
                            var logic = new SerialsLogic();
                            return Json(logic.DeleteShare(id, User.Identity.GetUserId()));
                        }
                    case "books":
                        {
                            var logic = new BooksLogic();
                            return Json(logic.DeleteShare(id, User.Identity.GetUserId()));
                        }
                    case "events":
                        {
                            var logic = new EventsLogic();
                            return Json(logic.DeleteShare(id, User.Identity.GetUserId()));
                        }
                }
            }
            catch (Exception ex)
            {
                logger.Error(methodName, ex);
            }
            return Json("-");
        }

        public JsonResult ChangeShowEndedEvents(string selected)
        {
            var logger = new NLogLogger();
            const string methodName = "public JsonResult ChangeShowEndedEvents(string selected)";
            try
            {
                WriteUserSideStorage(UserSideStorageKeys.EndedEvents, selected);
                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                logger.Error(methodName, ex);
            }
            return new JsonResult { Data = new { success = false, error = methodName } };
        }

        [HttpPost]
        public JsonResult GetPage(int? page, string search)
        {
            if (!User.Identity.IsAuthenticated) return Json(Auth.NoAuth);

            if (Admin.IsDebug)
            {
                Thread.Sleep(2000); //чтобы увидеть загрузку
            }

            var logger = new NLogLogger();
            const string methodName = "public JsonResult GetPage(int? page, string search)";
            try
            {
                if (ReadUserSideStorage(UserSideStorageKeys.HomeCategory, Defaults.CategoryBase.Indexes.Films) ==
                Defaults.CategoryBase.Indexes.Events)
                {
                    return
                        Json(new HomeViewModelEvents(User.Identity.GetUserId(), page ?? 1, Rpp, search,
                            ReadUserSideStorage(UserSideStorageKeys.EndedEvents, 0) == 1));
                }
                else if (ReadUserSideStorage(UserSideStorageKeys.HomeCategory, Defaults.CategoryBase.Indexes.Films) ==
                         Defaults.CategoryBase.Indexes.Books)
                {
                    return Json(new HomeViewModelBooks(User.Identity.GetUserId(), page ?? 1, Rpp, search));
                }
                else if (ReadUserSideStorage(UserSideStorageKeys.HomeCategory, Defaults.CategoryBase.Indexes.Films) ==
                         Defaults.CategoryBase.Indexes.Serials)
                {
                    return Json(new HomeViewModelSerials(User.Identity.GetUserId(), page ?? 1, Rpp, search));
                }
                else if (ReadUserSideStorage(UserSideStorageKeys.HomeCategory, Defaults.CategoryBase.Indexes.Films) ==
                         Defaults.CategoryBase.Indexes.Films)
                {
                    return Json(new HomeViewModelFilms(User.Identity.GetUserId(), page ?? 1, Rpp, search));
                }
                else if (ReadUserSideStorage(UserSideStorageKeys.HomeCategory, Defaults.CategoryBase.Indexes.Films) ==
                         Defaults.CategoryBase.IndexesExt.Users)
                {
                    return Json(new HomeViewModelUsers(page ?? 1, Rpp));
                }
                else if (ReadUserSideStorage(UserSideStorageKeys.HomeCategory, Defaults.CategoryBase.Indexes.Films) ==
                         Defaults.CategoryBase.IndexesExt.Logs)
                {
                    return Json(new HomeViewModelLogs(page ?? 1, Rpp));
                }
                else if (ReadUserSideStorage(UserSideStorageKeys.HomeCategory, Defaults.CategoryBase.Indexes.Films) ==
                         Defaults.CategoryBase.IndexesExt.Improvements)
                {
                    return
                        Json(
                            new HomeViewModelImprovements(
                                ReadUserSideStorage(UserSideStorageKeys.ImprovementsCategory,
                                    Defaults.ComplexBase.Indexes.All), page ?? 1, Rpp));
                }
                else if (ReadUserSideStorage(UserSideStorageKeys.HomeCategory, Defaults.CategoryBase.Indexes.Films) ==
                            Defaults.CategoryBase.Indexes.Roads)
                {
                    return
                        Json(
                            new HomeViewModelRoads(User.Identity.GetUserId(), MarkersOnRoads,
                                ReadUserSideStorage(UserSideStorageKeys.RoadsYear, 0)));
                }
                else if (ReadUserSideStorage(UserSideStorageKeys.HomeCategory, Defaults.CategoryBase.Indexes.Films) ==
                        Defaults.CategoryBase.IndexesExt.Errors)
                {
                    return Json(new HomeViewModelErrors(page ?? 1, Rpp));
                }
                return Json("NOT REALIZED");
            }
            catch (Exception ex)
            {
                logger.Error(methodName, ex);
            }
            return new JsonResult { Data = new { success = false, error = methodName } };
        }

        [HttpPost]
        public JsonResult GetSharedPage(string pageName, string key, int? page, string search)
        {
            var logger = new NLogLogger();
            const string methodName = "public JsonResult GetSharedPage(string pageName, string key, int? page, string search)";
            try
            {
                if (pageName.ToLower() == "films")
                {
                    return Json(new ShareViewModelFilms(key, page ?? 1, Rpp));
                }
                else if (pageName.ToLower() == "serials")
                {
                    return Json(new ShareViewModelSerials(key, page ?? 1, Rpp));
                }
                else if (pageName.ToLower() == "books")
                {
                    return Json(new ShareViewModelBooks(key, page ?? 1, Rpp));
                }
                else if (pageName.ToLower() == "events")
                {
                    return Json(new ShareViewModelEvents(key, page ?? 1, Rpp));
                }
                return Json("NOT REALIZED");
            }
            catch (Exception ex)
            {
                logger.Error(methodName, ex);
            }
            return new JsonResult { Data = new { success = false, error = methodName } };
        }
    }
}