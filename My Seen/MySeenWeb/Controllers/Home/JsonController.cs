using System;
using System.Collections.Generic;
using System.Threading;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using MySeenLib;
using MySeenWeb.ActionFilters;
using MySeenWeb.Add_Code;
using MySeenWeb.Add_Code.Services.Logging.NLog;
using MySeenWeb.Controllers._Base;
using MySeenWeb.Models;
using MySeenWeb.Models.Portal;
using MySeenWeb.Models.Prepared;
using MySeenWeb.Models.TablesLogic;
using MySeenWeb.Models.TablesLogic.Portal;
using MySeenWeb.Models.Translations;
using MySeenWeb.Models.Translations.Portal;

namespace MySeenWeb.Controllers.Home
{
    public class JsonController : BaseController
    {
        private readonly ICacheService _cache;
        public JsonController(ICacheService cache)
        {
            _cache = cache;
        }
        [Compress]
        public ActionResult Index()
        {
            var logger = new NLogLogger();
            const string methodName = "public ActionResult Index()";
            try
            {
                return View(new HomeViewModel(User.Identity.GetUserId(), MarkersOnRoads, Theme, Request));
            }
            catch (Exception ex)
            {
                logger.Error(methodName, ex);
            }
            return null;
        }
        [Compress]
        [HttpPost]
        public JsonResult GetPage(int pageId, int? page, string search, int? ended, int? year, int? complex, string shareKey, int? road, int? id, string dateMan, string dateWoman, int? price, int? deals, int? salary)
        {
            //Thread.Sleep(2000); //чтобы увидеть загрузку
            var logger = new NLogLogger();
            const string methodName = "public JsonResult GetPage(int pageId, int? page, string search, int? ended, int? year, int? complex,string shareKey)";
            try
            {
                switch (pageId)
                {
                    case (int) Defaults.CategoryBase.Indexes.Films:
                        if (!User.Identity.IsAuthenticated && string.IsNullOrEmpty(shareKey))
                            return new JsonResult {Data = new {success = false, error = Resource.NotAuthorized}};
                        return
                            Json(new HomeViewModelFilms(User.Identity.GetUserId(), page ?? 1, Rpp, search, shareKey,
                                _cache));
                    case (int) Defaults.CategoryBase.Indexes.Serials:
                        if (!User.Identity.IsAuthenticated && string.IsNullOrEmpty(shareKey))
                            return new JsonResult {Data = new {success = false, error = Resource.NotAuthorized}};
                        return
                            Json(new HomeViewModelSerials(User.Identity.GetUserId(), page ?? 1, Rpp, search, shareKey,
                                _cache));
                    case (int) Defaults.CategoryBase.Indexes.Books:
                        if (!User.Identity.IsAuthenticated && string.IsNullOrEmpty(shareKey))
                            return new JsonResult {Data = new {success = false, error = Resource.NotAuthorized}};
                        return
                            Json(new HomeViewModelBooks(User.Identity.GetUserId(), page ?? 1, Rpp, search, shareKey,
                                _cache));
                    case (int) Defaults.CategoryBase.Indexes.Events:
                        if (!User.Identity.IsAuthenticated && string.IsNullOrEmpty(shareKey))
                            return new JsonResult {Data = new {success = false, error = Resource.NotAuthorized}};
                        return
                            Json(new HomeViewModelEvents(User.Identity.GetUserId(), page ?? 1, Rpp, search, ended ?? 0,
                                shareKey));
                    case (int) Defaults.CategoryBase.Indexes.Roads:
                        if (!User.Identity.IsAuthenticated && string.IsNullOrEmpty(shareKey))
                            return new JsonResult {Data = new {success = false, error = Resource.NotAuthorized}};
                        return
                            Json(new HomeViewModelRoads(User.Identity.GetUserId(), year ?? 0, search, shareKey, _cache,
                                road ?? 0));
                    case (int) Defaults.CategoryBase.IndexesExt.Improvements:
                        if (!User.Identity.IsAuthenticated)
                            return new JsonResult {Data = new {success = false, error = Resource.NotAuthorized}};
                        return
                            Json(new HomeViewModelImprovements(User.Identity.GetUserId(),
                                complex ?? (int) Defaults.ComplexBase.Indexes.All, page ?? 1, Rpp, search, ended ?? 0));
                    case (int) Defaults.CategoryBase.IndexesExt.Users:
                        if (!User.Identity.IsAuthenticated && string.IsNullOrEmpty(shareKey))
                            return new JsonResult {Data = new {success = false, error = Resource.NotAuthorized}};
                        if (!UserRolesLogic.IsAdmin(User.Identity.GetUserId()))
                            return new JsonResult {Data = new {success = false, error = Resource.NoRights}};
                        return Json(new HomeViewModelUsers(User.Identity.GetUserId(), page ?? 1, Rpp, search));
                    case (int) Defaults.CategoryBase.IndexesExt.Errors:
                        if (!User.Identity.IsAuthenticated && string.IsNullOrEmpty(shareKey))
                            return new JsonResult {Data = new {success = false, error = Resource.NotAuthorized}};
                        if (!UserRolesLogic.IsAdmin(User.Identity.GetUserId()))
                            return new JsonResult {Data = new {success = false, error = Resource.NoRights}};
                        return Json(new HomeViewModelErrors(page ?? 1, Rpp, search));
                    case (int) Defaults.CategoryBase.IndexesExt.Logs:
                        if (!User.Identity.IsAuthenticated && string.IsNullOrEmpty(shareKey))
                            return new JsonResult {Data = new {success = false, error = Resource.NotAuthorized}};
                        if (!UserRolesLogic.IsAdmin(User.Identity.GetUserId()))
                            return new JsonResult {Data = new {success = false, error = Resource.NoRights}};
                        return Json(new HomeViewModelLogs(page ?? 1, Rpp, search));
                    case (int) Defaults.CategoryBase.IndexesExt.Settings:
                        if (!User.Identity.IsAuthenticated && string.IsNullOrEmpty(shareKey))
                            return new JsonResult {Data = new {success = false, error = Resource.NotAuthorized}};
                        return Json(new HomeViewModelSettings(User.Identity.GetUserId()));
                    case (int)Defaults.CategoryBase.IndexesMain.Memes:
                        return Json(new PortalViewModelMemes(User.Identity.GetUserId(), page ?? 1, 20, search, id ?? 0));//Всегда по 20 на странице
                    case (int) Defaults.CategoryBase.IndexesMain.Childs:
                        logger.Info("ChildCalculator for user=" + User.Identity.GetUserName() + ";M=" + dateMan + ";W=" +
                                    dateWoman + ";Y=" + (year ?? 0));
                        return Json(new PortalViewModelChildCalculator(year ?? 0, dateMan, dateWoman));
                    case (int)Defaults.CategoryBase.IndexesMain.Realt:
                        return Json(new PortalViewModelRealt(year ?? 0, price ?? 0, deals ?? 0, salary ?? 0));
                }
                logger.Info("CALL NOT REALIZED GetPage=" + pageId);
                return new JsonResult { Data = new { success = false, error = "NOT REALIZED" } };
            }
            catch (Exception ex)
            {
                logger.Error(methodName, ex);
            }
            return new JsonResult { Data = new { success = false, error = methodName } };
        }
        [Compress]
        [HttpPost]
        public JsonResult GetPrepared(int pageId)
        {
            var logger = new NLogLogger();
            const string methodName = "public JsonResult GetPrepared(int pageId)";
            try
            {
                switch (pageId)
                {
                    case (int)Defaults.CategoryBase.Indexes.Films:
                        return Json(new PreparedDataFilms());
                    case (int)Defaults.CategoryBase.Indexes.Serials:
                        return Json(new PreparedDataSerials());
                    case (int)Defaults.CategoryBase.Indexes.Books:
                        return Json(new PreparedDataBooks());
                    case (int)Defaults.CategoryBase.Indexes.Events:
                        return Json(new PreparedDataEvents());
                    case (int)Defaults.CategoryBase.Indexes.Roads:
                        return Json(new PreparedDataRoads());
                    case (int)Defaults.CategoryBase.IndexesExt.Improvements:
                        return Json(new PreparedDataImprovements());
                }
                logger.Info("CALL NOT REALIZED GetPrepared=" + pageId);
                return Json("NOT REALIZED");
            }
            catch (Exception ex)
            {
                logger.Error(methodName, ex);
            }
            return new JsonResult { Data = new { success = false, error = methodName } };
        }
        [Compress]
        [HttpPost]
        public JsonResult GetTranslation(int pageId)
        {
            //Thread.Sleep(2000); //чтобы увидеть загрузку

            var logger = new NLogLogger();
            const string methodName = "public JsonResult GetTranslation(int pageId)";
            try
            {
                switch (pageId)
                {
                    case (int) Defaults.CategoryBase.Indexes.Films:
                        return Json(new TranslationDataFilms());
                    case (int) Defaults.CategoryBase.Indexes.Serials:
                        return Json(new TranslationDataSerials());
                    case (int) Defaults.CategoryBase.Indexes.Books:
                        return Json(new TranslationDataBooks());
                    case (int) Defaults.CategoryBase.Indexes.Events:
                        return Json(new TranslationDataEvents());
                    case (int) Defaults.CategoryBase.IndexesExt.Users:
                        if (!UserRolesLogic.IsAdmin(User.Identity.GetUserId())) return new JsonResult { Data = new { success = false, error = Resource.NoRights } };
                        return Json(new TranslationDataUsers());
                    case (int) Defaults.CategoryBase.IndexesExt.Logs:
                        if (!UserRolesLogic.IsAdmin(User.Identity.GetUserId())) return new JsonResult { Data = new { success = false, error = Resource.NoRights } };
                        return Json(new TranslationDataLogs());
                    case (int)Defaults.CategoryBase.IndexesExt.Errors:
                        if (!UserRolesLogic.IsAdmin(User.Identity.GetUserId())) return new JsonResult { Data = new { success = false, error = Resource.NoRights } };
                        return Json(new TranslationDataErrors());
                    case (int)Defaults.CategoryBase.Indexes.Roads:
                        return Json(new TranslationDataRoads());
                    case (int)Defaults.CategoryBase.IndexesExt.Improvements:
                        return Json(new TranslationDataImprovements());
                    case (int)Defaults.CategoryBase.IndexesExt.Settings:
                        return Json(new TranslationDataSettings());
                    case (int)Defaults.CategoryBase.IndexesMain.Main:
                        return Json(new TranslationDataPortalMain());
                    case (int)Defaults.CategoryBase.IndexesMain.Memes:
                        return Json(new TranslationDataPortalMemes());
                    case (int)Defaults.CategoryBase.IndexesMain.Childs:
                        return Json(new TranslationDataPortalChildSexCalculator());
                    case (int)Defaults.CategoryBase.IndexesMain.Realt:
                        return Json(new TranslationDataPortalRealt());
                    case (int)Defaults.CategoryBase.IndexesMain.Imt:
                        return Json(new TranslationDataPortalImtCalculator());

                }
                logger.Info("CALL NOT REALIZED GetTranslation=" + pageId);
                return new JsonResult { Data = new { success = false, error = "NOT REALIZED" } };
            }
            catch (Exception ex)
            {
                logger.Error(methodName, ex);
            }
            return new JsonResult {Data = new {success = false, error = methodName}};
        }
        [Compress]
        [Authorize]
        [HttpPost]
        public JsonResult GetShare(int pageId, string recordId)
        {
            var logger = new NLogLogger();
            const string methodName = "public JsonResult GetShare(int pageId, string recordId)";
            try
            {
                switch (pageId)
                {
                    case (int) Defaults.CategoryBase.Indexes.Films:
                        var filmsLogic = new FilmsLogic(_cache);
                        return Json(filmsLogic.GetShare(recordId, User.Identity.GetUserId()));
                    case (int) Defaults.CategoryBase.Indexes.Serials:
                        var serialsLogic = new SerialsLogic(_cache);
                        return Json(serialsLogic.GetShare(recordId, User.Identity.GetUserId()));
                    case (int) Defaults.CategoryBase.Indexes.Books:
                        var booksLogic = new BooksLogic(_cache);
                        return Json(booksLogic.GetShare(recordId, User.Identity.GetUserId()));
                    case (int) Defaults.CategoryBase.Indexes.Events:
                        var eventsLogic = new EventsLogic();
                        return Json(eventsLogic.GetShare(recordId, User.Identity.GetUserId()));
                    case (int) Defaults.CategoryBase.Indexes.Roads:
                        var roadsLogic = new RoadsLogic(_cache);
                        return Json(roadsLogic.GetShare(recordId, User.Identity.GetUserId()));
                }
                logger.Info("CALL NOT REALIZED GetShare=" + pageId);
            }
            catch (Exception ex)
            {
                logger.Error(methodName, ex);
            }
            return Json("-");
        }
        [Compress]
        [Authorize]
        [HttpPost]
        public JsonResult GenerateShare(int pageId, string recordId)
        {
            var logger = new NLogLogger();
            const string methodName = "public JsonResult GenerateShare(int pageId, string recordId)";
            try
            {
                switch (pageId)
                {
                    case (int)Defaults.CategoryBase.Indexes.Films:
                        var filmsLogic = new FilmsLogic(_cache);
                        return Json(filmsLogic.GenerateShare(recordId, User.Identity.GetUserId()));
                    case (int)Defaults.CategoryBase.Indexes.Serials:
                        var serialsLogic = new SerialsLogic(_cache);
                        return Json(serialsLogic.GenerateShare(recordId, User.Identity.GetUserId()));
                    case (int)Defaults.CategoryBase.Indexes.Books:
                        var booksLogic = new BooksLogic(_cache);
                        return Json(booksLogic.GenerateShare(recordId, User.Identity.GetUserId()));
                    case (int)Defaults.CategoryBase.Indexes.Events:
                        var eventsLogic = new EventsLogic();
                        return Json(eventsLogic.GenerateShare(recordId, User.Identity.GetUserId()));
                    case (int)Defaults.CategoryBase.Indexes.Roads:
                        var roadsLogic = new RoadsLogic(_cache);
                        return Json(roadsLogic.GenerateShare(recordId, User.Identity.GetUserId()));
                }
                logger.Info("CALL NOT REALIZED GenerateShare=" + pageId);
            }
            catch (Exception ex)
            {
                logger.Error(methodName, ex);
            }
            return Json("-");
        }
        [Compress]
        [Authorize]
        [HttpPost]
        public JsonResult DeleteShare(int pageId, string recordId)
        {
            var logger = new NLogLogger();
            const string methodName = "public JsonResult DeleteShare(int pageId, string recordId)";
            try
            {
                switch (pageId)
                {
                    case (int)Defaults.CategoryBase.Indexes.Films:
                        var filmsLogic = new FilmsLogic(_cache);
                        return Json(filmsLogic.DeleteShare(recordId, User.Identity.GetUserId()));
                    case (int)Defaults.CategoryBase.Indexes.Serials:
                        var serialsLogic = new SerialsLogic(_cache);
                        return Json(serialsLogic.DeleteShare(recordId, User.Identity.GetUserId()));
                    case (int)Defaults.CategoryBase.Indexes.Books:
                        var booksLogic = new BooksLogic(_cache);
                        return Json(booksLogic.DeleteShare(recordId, User.Identity.GetUserId()));
                    case (int)Defaults.CategoryBase.Indexes.Events:
                        var eventsLogic = new EventsLogic();
                        return Json(eventsLogic.DeleteShare(recordId, User.Identity.GetUserId()));
                    case (int)Defaults.CategoryBase.Indexes.Roads:
                        var roadsLogic = new RoadsLogic(_cache);
                        return Json(roadsLogic.DeleteShare(recordId, User.Identity.GetUserId()));
                }
                logger.Info("CALL NOT REALIZED DeleteShare=" + pageId);
            }
            catch (Exception ex)
            {
                logger.Error(methodName, ex);
            }
            return Json("-");
        }
        [Compress]
        [Authorize]
        [HttpPost]
        public JsonResult AddData(int pageId, string name, string year, string datetime, string genre, string rating, string season
            , string series, string authors, string type, string coordinates, string distance, string link)
        {
            var logger = new NLogLogger();
            var methodName = "public JsonResult AddData(int pageId, string name, string year, string datetime, string genre, string rating, string season, string series, string authors, string type, string coordinates, string distance, string desc, string complex)";
            try
            {
                switch (pageId)
                {
                    case (int) Defaults.CategoryBase.Indexes.Films:
                        var filmsLogic = new FilmsLogic(_cache);
                        return !filmsLogic.Add(name, year, datetime, genre, rating, User.Identity.GetUserId())
                            ? new JsonResult { Data = new { success = false, error = filmsLogic.ErrorMessage } }
                            : Json(new {success = true});
                    case (int)Defaults.CategoryBase.Indexes.Serials:
                        var serialsLogic = new SerialsLogic(_cache);
                        return
                            !serialsLogic.Add(name, year, season, series, datetime, genre, rating,
                                User.Identity.GetUserId())
                                ? new JsonResult {Data = new {success = false, error = serialsLogic.ErrorMessage}}
                                : Json(new {success = true});
                    case (int)Defaults.CategoryBase.Indexes.Books:
                        var booksLogic = new BooksLogic(_cache);
                        return !booksLogic.Add(name, year, authors, datetime, genre, rating, User.Identity.GetUserId())
                            ? new JsonResult {Data = new {success = false, error = booksLogic.ErrorMessage}}
                            : Json(new {success = true});
                    case (int)Defaults.CategoryBase.Indexes.Roads:
                        var tracksLogic = new RoadsLogic(_cache);
                        return !tracksLogic.Add(name, datetime, type, coordinates, distance, User.Identity.GetUserId())
                            ? new JsonResult {Data = new {success = false, error = tracksLogic.ErrorMessage}}
                            : Json(new {success = true});
                    case (int)Defaults.CategoryBase.Indexes.Events:
                        var eventsLogic = new EventsLogic();
                        return !eventsLogic.Add(name, datetime, type, User.Identity.GetUserId())
                            ? new JsonResult {Data = new {success = false, error = eventsLogic.ErrorMessage}}
                            : Json(new {success = true});
                    case (int)Defaults.CategoryBase.IndexesExt.Improvements:
                        var improvementLogic = new ImprovementLogic();
                        return !improvementLogic.Add(name, type, User.Identity.GetUserId())
                            ? new JsonResult { Data = new { success = false, error = improvementLogic.ErrorMessage } }
                            : Json(new {success = true});
                    case (int)Defaults.CategoryBase.IndexesMain.Memes:
                        var memesLogic = new MemesLogic();
                        return !memesLogic.Add(name, link, User.Identity.GetUserId())
                            ? new JsonResult {Data = new {success = false, error = memesLogic.ErrorMessage}}
                            : Json(new {success = true});
                }
                logger.Info("CALL NOT REALIZED AddData=" + pageId);
                return Json("NOT REALIZED");
            }
            catch (Exception ex)
            {
                logger.Error(methodName, ex);
            }
            return new JsonResult { Data = new { success = false, error = methodName } };
        }
        [Compress]
        [Authorize]
        [HttpPost]
        public JsonResult DeleteData(int pageId, string recordId)
        {
            var logger = new NLogLogger();
            var methodName = "public JsonResult DelData(int pageId, int recordId)";
            try
            {
                switch (pageId)
                {
                    case (int)Defaults.CategoryBase.Indexes.Films:
                        var filmsLogic = new FilmsLogic(_cache);
                        return !filmsLogic.Delete(recordId, User.Identity.GetUserId())
                            ? new JsonResult { Data = new { success = false, error = filmsLogic.ErrorMessage } }
                            : Json(new { success = true });
                    case (int)Defaults.CategoryBase.Indexes.Serials:
                        var serialsLogic = new SerialsLogic(_cache);
                        return
                            !serialsLogic.Delete(recordId, User.Identity.GetUserId())
                                ? new JsonResult { Data = new { success = false, error = serialsLogic.ErrorMessage } }
                                : Json(new { success = true });
                    case (int)Defaults.CategoryBase.Indexes.Books:
                        var booksLogic = new BooksLogic(_cache);
                        return !booksLogic.Delete(recordId, User.Identity.GetUserId())
                            ? new JsonResult { Data = new { success = false, error = booksLogic.ErrorMessage } }
                            : Json(new { success = true });
                    case (int)Defaults.CategoryBase.Indexes.Roads:
                        var tracksLogic = new RoadsLogic(_cache);
                        return !tracksLogic.Delete(recordId, User.Identity.GetUserId())
                            ? new JsonResult { Data = new { success = false, error = tracksLogic.ErrorMessage } }
                            : Json(new { success = true });
                    case (int)Defaults.CategoryBase.Indexes.Events:
                        var eventsLogic = new EventsLogic();
                        return !eventsLogic.Delete(recordId, User.Identity.GetUserId())
                            ? new JsonResult { Data = new { success = false, error = eventsLogic.ErrorMessage } }
                            : Json(new { success = true });
                    case (int)Defaults.CategoryBase.IndexesExt.Improvements:
                        if (!UserRolesLogic.IsAdmin(User.Identity.GetUserId())) return new JsonResult { Data = new { success = false, error = Resource.NoRights } };
                        var improvementsLogic = new ImprovementLogic();
                        return !improvementsLogic.Delete(recordId, User.Identity.GetUserId())
                            ? new JsonResult { Data = new { success = false, error = improvementsLogic.ErrorMessage } }
                            : Json(new { success = true });
                }
                logger.Info("CALL NOT REALIZED DeleteData=" + pageId);
                return Json("NOT REALIZED");
            }
            catch (Exception ex)
            {
                logger.Error(methodName, ex);
            }
            return new JsonResult { Data = new { success = false, error = methodName } };
        }
        [Compress]
        [Authorize]
        [HttpPost]
        public JsonResult UpdateData(int pageId, string id, string name, string year, string datetime, string genre, string rating, string season, string series, string authors, string type, string coordinates, string distance)
        {
            var logger = new NLogLogger();
            var methodName = "public JsonResult UpdateData(int pageId, string name, string year, string datetime, string genre, string rating, string season, string series, string authors, string type, string coordinates, string distance, string desc, string complex)";
            try
            {
                switch (pageId)
                {
                    case (int)Defaults.CategoryBase.Indexes.Films:
                        var filmsLogic = new FilmsLogic(_cache);
                        return !filmsLogic.Update(id, name, year, datetime, genre, rating, User.Identity.GetUserId())
                            ? new JsonResult { Data = new { success = false, error = filmsLogic.ErrorMessage } }
                            : Json(new { success = true });
                    case (int)Defaults.CategoryBase.Indexes.Serials:
                        var serialsLogic = new SerialsLogic(_cache);
                        return
                            !serialsLogic.Update(id, name, year, season, series, datetime, genre, rating,
                                User.Identity.GetUserId())
                                ? new JsonResult { Data = new { success = false, error = serialsLogic.ErrorMessage } }
                                : Json(new { success = true });
                    case (int)Defaults.CategoryBase.Indexes.Books:
                        var booksLogic = new BooksLogic(_cache);
                        return !booksLogic.Update(id, name, year, authors, datetime, genre, rating, User.Identity.GetUserId())
                            ? new JsonResult { Data = new { success = false, error = booksLogic.ErrorMessage } }
                            : Json(new { success = true });
                    case (int)Defaults.CategoryBase.Indexes.Roads:
                        var tracksLogic = new RoadsLogic(_cache);
                        return !tracksLogic.Update(id, name, datetime, type, coordinates, distance, User.Identity.GetUserId())
                            ? new JsonResult { Data = new { success = false, error = tracksLogic.ErrorMessage } }
                            : Json(new { success = true });
                    case (int)Defaults.CategoryBase.Indexes.Events:
                        var eventsLogic = new EventsLogic();
                        return !eventsLogic.Update(id, name, datetime, type, User.Identity.GetUserId())
                            ? new JsonResult { Data = new { success = false, error = eventsLogic.ErrorMessage } }
                            : Json(new { success = true });
                    case (int)Defaults.CategoryBase.IndexesExt.Improvements:
                        if (!UserRolesLogic.IsAdmin(User.Identity.GetUserId())) return new JsonResult { Data = new { success = false, error = Resource.NoRights } };
                        var improvementsLogic = new ImprovementLogic();
                        return !improvementsLogic.Update(id, name, type, User.Identity.GetUserId())
                            ? new JsonResult { Data = new { success = false, error = improvementsLogic.ErrorMessage } }
                            : Json(new { success = true });
                }
                logger.Info("CALL NOT REALIZED UpdateData=" + pageId);
                return Json("NOT REALIZED");
            }
            catch (Exception ex)
            {
                logger.Error(methodName, ex);
            }
            return new JsonResult { Data = new { success = false, error = methodName } };
        }
        [Compress]
        [Authorize]
        [IsAdmin]
        [HttpPost]
        public JsonResult EndImprovement(string id, string name, string version)
        {
            var logger = new NLogLogger();
            var methodName = "public JsonResult EndImprovement(string recordId, string name,string version)";
            try
            {
                var logic = new ImprovementLogic();
                return !logic.End(id, name, version, User.Identity.GetUserId())
                    ? new JsonResult {Data = new {success = false, error = logic.ErrorMessage}}
                    : Json(new {success = true});
            }
            catch (Exception ex)
            {
                logger.Error(methodName, ex);
            }
            return new JsonResult {Data = new {success = false, error = methodName}};
        }
        [Compress]
        [Authorize]
        [IsAdmin]
        [HttpPost]
        public JsonResult RemoveAllError()
        {
            var logger = new NLogLogger();
            var methodName = "public JsonResult RemoveAllError()";
            try
            {
                var logic = new ErrorsLogic();

                return !logic.RemoveAll()
                    ? new JsonResult {Data = new {success = false, error = logic.ErrorMessage}}
                    : Json(new {success = true});
            }
            catch (Exception ex)
            {
                logger.Error(methodName, ex);
            }
            return new JsonResult {Data = new {success = false, error = methodName}};
        }
        [Compress]
        [Authorize]
        [HttpPost]
        public JsonResult AddSeries(string recordId)
        {
            var logger = new NLogLogger();
            var methodName = "public JsonResult AddSeries(string id)";
            try
            {
                var logic = new SerialsLogic(_cache);
                return !logic.AddSeries(recordId, User.Identity.GetUserId()) ? new JsonResult { Data = new { success = false, error = logic.ErrorMessage } } : Json(new { success = true });
            }
            catch (Exception ex)
            {
                logger.Error(methodName, ex);
            }
            return new JsonResult { Data = new { success = false, error = methodName } };
        }
        [Compress]
        [Authorize]
        [IsAdmin]
        [HttpPost]
        public JsonResult UpdateUser(string name, IEnumerable<string> roles)
        {
            var logger = new NLogLogger();
            var methodName = "public JsonResult UpdateUser(string name, List<string> roles)";
            try
            {
                var logic = new UserRolesLogic();
                return !logic.Update(name, roles)
                    ? new JsonResult { Data = new { success = false, error = logic.ErrorMessage } }
                    : Json(new { success = true });
            }
            catch (Exception ex)
            {
                logger.Error(methodName, ex);
            }
            return new JsonResult { Data = new { success = false, error = methodName } };
        }
        /* Angular translation Не умеет переводить плейсхолдеры.. не умеет переводить модальные...
        public JsonResult GetTranslation(string lang)
        {
            var logger = new NLogLogger();
            var methodName = "public JsonResult GetAllTranslations()";
            try
            {
                var resourceObject = new
                {
                    Name = Resource.Name,
                    Year = Resource.Year,
                    Date = Resource.Date,
                    Genre = Resource.Genre,
                    Rating = Resource.Rating
                };
                return Json(resourceObject, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                logger.Error(methodName, ex);
            }
            return new JsonResult { Data = new { success = false, error = methodName } };
        }
         */
    }
}