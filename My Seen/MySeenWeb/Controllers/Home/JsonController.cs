using System;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using MySeenLib;
using MySeenWeb.Add_Code.Services.Logging.NLog;
using MySeenWeb.Controllers._Base;
using MySeenWeb.Models;
using MySeenWeb.Models.Prepared;
using MySeenWeb.Models.TablesLogic;
using MySeenWeb.Models.Translations;

namespace MySeenWeb.Controllers.Home
{
    public class JsonController : BaseController
    {
        [HttpPost]
        public JsonResult GetPage(int pageId, int? page, string search, int? ended, int? year, int? complex,string shareKey)
        {
            //if (!User.Identity.IsAuthenticated) return Json(Auth.NoAuth);

            if (Admin.IsDebug)
            {
                //Thread.Sleep(2000); //чтобы увидеть загрузку
            }

            var logger = new NLogLogger();
            const string methodName = "public JsonResult GetPage(int pageId, int? page, string search)";
            try
            {
                switch (pageId)
                {
                    case (int)Defaults.CategoryBase.Indexes.Films:
                        return
                            Json(new HomeViewModelFilms(User.Identity.GetUserId(), page ?? 1, Rpp, search, shareKey));
                    case (int)Defaults.CategoryBase.Indexes.Serials:
                        return Json(new HomeViewModelSerials(User.Identity.GetUserId(), page ?? 1, Rpp, search, shareKey));
                    case (int)Defaults.CategoryBase.Indexes.Books:
                        return Json(new HomeViewModelBooks(User.Identity.GetUserId(), page ?? 1, Rpp, search, shareKey));
                    case (int)Defaults.CategoryBase.Indexes.Events:
                        return
                            Json(new HomeViewModelEvents(User.Identity.GetUserId(), page ?? 1, Rpp, search, ended ?? 0, shareKey));
                    case (int)Defaults.CategoryBase.Indexes.Roads:
                        return Json(new HomeViewModelRoads(User.Identity.GetUserId(), year ?? 0));
                    case (int)Defaults.CategoryBase.IndexesExt.Improvements:
                        return Json(new HomeViewModelImprovements(complex ?? (int)Defaults.ComplexBase.Indexes.All, page ?? 1, Rpp));
                    case (int)Defaults.CategoryBase.IndexesExt.Users:
                        return Json(new HomeViewModelUsers(page ?? 1, Rpp, search));
                    case (int)Defaults.CategoryBase.IndexesExt.Errors:
                        return Json(new HomeViewModelErrors(page ?? 1, Rpp, search));
                    case (int)Defaults.CategoryBase.IndexesExt.Logs:
                        return Json(new HomeViewModelLogs(page ?? 1, Rpp, search));
                    case (int)Defaults.CategoryBase.IndexesExt.Settings:
                        return Json(new HomeViewModelSettings(User.Identity.GetUserId()));
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
        public JsonResult GetTranslation(int pageId)
        {
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
                        return Json(new TranslationDataUsers());
                    case (int) Defaults.CategoryBase.IndexesExt.Logs:
                        return Json(new TranslationDataLogs());
                    case (int)Defaults.CategoryBase.IndexesExt.Errors:
                        return Json(new TranslationDataErrors());
                    case (int)Defaults.CategoryBase.Indexes.Roads:
                        return Json(new TranslationDataRoads());

                }
                return Json("NOT REALIZED");
            }
            catch (Exception ex)
            {
                logger.Error(methodName, ex);
            }
            return new JsonResult {Data = new {success = false, error = methodName}};
        }

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
                        var filmsLogic = new FilmsLogic();
                        return Json(filmsLogic.GetShare(recordId, User.Identity.GetUserId()));
                    case (int) Defaults.CategoryBase.Indexes.Serials:
                        var serialsLogic = new SerialsLogic();
                        return Json(serialsLogic.GetShare(recordId, User.Identity.GetUserId()));
                    case (int) Defaults.CategoryBase.Indexes.Books:
                        var booksLogic = new BooksLogic();
                        return Json(booksLogic.GetShare(recordId, User.Identity.GetUserId()));
                    case (int) Defaults.CategoryBase.Indexes.Events:
                        var eventsLogic = new EventsLogic();
                        return Json(eventsLogic.GetShare(recordId, User.Identity.GetUserId()));
                    case (int) Defaults.CategoryBase.Indexes.Roads:
                        var roadsLogic = new RoadsLogic();
                        return Json(roadsLogic.GetShare(recordId, User.Identity.GetUserId()));
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
        public JsonResult GenerateShare(int pageId, string recordId)
        {
            var logger = new NLogLogger();
            const string methodName = "public JsonResult GenerateShare(int pageId, string recordId)";
            try
            {
                switch (pageId)
                {
                    case (int)Defaults.CategoryBase.Indexes.Films:
                        var filmsLogic = new FilmsLogic();
                        return Json(filmsLogic.GenerateShare(recordId, User.Identity.GetUserId()));
                    case (int)Defaults.CategoryBase.Indexes.Serials:
                        var serialsLogic = new SerialsLogic();
                        return Json(serialsLogic.GenerateShare(recordId, User.Identity.GetUserId()));
                    case (int)Defaults.CategoryBase.Indexes.Books:
                        var booksLogic = new BooksLogic();
                        return Json(booksLogic.GenerateShare(recordId, User.Identity.GetUserId()));
                    case (int)Defaults.CategoryBase.Indexes.Events:
                        var eventsLogic = new EventsLogic();
                        return Json(eventsLogic.GenerateShare(recordId, User.Identity.GetUserId()));
                    case (int)Defaults.CategoryBase.Indexes.Roads:
                        var roadsLogic = new RoadsLogic();
                        return Json(roadsLogic.GenerateShare(recordId, User.Identity.GetUserId()));
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
        public JsonResult DeleteShare(int pageId, string recordId)
        {
            var logger = new NLogLogger();
            const string methodName = "public JsonResult DeleteShare(int pageId, string recordId)";
            try
            {
                switch (pageId)
                {
                    case (int)Defaults.CategoryBase.Indexes.Films:
                        var filmsLogic = new FilmsLogic();
                        return Json(filmsLogic.DeleteShare(recordId, User.Identity.GetUserId()));
                    case (int)Defaults.CategoryBase.Indexes.Serials:
                        var serialsLogic = new SerialsLogic();
                        return Json(serialsLogic.DeleteShare(recordId, User.Identity.GetUserId()));
                    case (int)Defaults.CategoryBase.Indexes.Books:
                        var booksLogic = new BooksLogic();
                        return Json(booksLogic.DeleteShare(recordId, User.Identity.GetUserId()));
                    case (int)Defaults.CategoryBase.Indexes.Events:
                        var eventsLogic = new EventsLogic();
                        return Json(eventsLogic.DeleteShare(recordId, User.Identity.GetUserId()));
                    case (int)Defaults.CategoryBase.Indexes.Roads:
                        var roadsLogic = new RoadsLogic();
                        return Json(roadsLogic.DeleteShare(recordId, User.Identity.GetUserId()));
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
        public JsonResult AddData(int pageId, string name, string year, string datetime, string genre, string rating, string season, string series, string authors, string type, string coordinates, string distance, string desc, string complex)
        {
            var logger = new NLogLogger();
            var methodName = "public JsonResult AddData(int pageId, string name, string year, string datetime, string genre, string rating, string season, string series, string authors, string type, string coordinates, string distance, string desc, string complex)";
            try
            {
                switch (pageId)
                {
                    case (int) Defaults.CategoryBase.Indexes.Films:
                        var filmsLogic = new FilmsLogic();
                        return !filmsLogic.Add(name, year, datetime, genre, rating, User.Identity.GetUserId())
                            ? new JsonResult { Data = new { success = false, error = filmsLogic.ErrorMessage } }
                            : Json(new {success = true});
                    case (int)Defaults.CategoryBase.Indexes.Serials:
                        var serialsLogic = new SerialsLogic();
                        return
                            !serialsLogic.Add(name, year, season, series, datetime, genre, rating,
                                User.Identity.GetUserId())
                                ? new JsonResult {Data = new {success = false, error = serialsLogic.ErrorMessage}}
                                : Json(new {success = true});
                    case (int)Defaults.CategoryBase.Indexes.Books:
                        var booksLogic = new BooksLogic();
                        return !booksLogic.Add(name, year, authors, datetime, genre, rating, User.Identity.GetUserId())
                            ? new JsonResult {Data = new {success = false, error = booksLogic.ErrorMessage}}
                            : Json(new {success = true});
                    case (int)Defaults.CategoryBase.Indexes.Roads:
                        var tracksLogic = new TracksLogic();
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
                        return !improvementLogic.Add(desc, complex, User.Identity.GetUserId())
                            ? new JsonResult { Data = new { success = false, error = improvementLogic.ErrorMessage } }
                            : Json(new {success = true});
                }
                return Json("NOT REALIZED");
            }
            catch (Exception ex)
            {
                logger.Error(methodName, ex);
            }
            return new JsonResult { Data = new { success = false, error = methodName } };
        }
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
                        var filmsLogic = new FilmsLogic();
                        return !filmsLogic.Delete(recordId, User.Identity.GetUserId())
                            ? new JsonResult { Data = new { success = false, error = filmsLogic.ErrorMessage } }
                            : Json(new { success = true });
                    case (int)Defaults.CategoryBase.Indexes.Serials:
                        var serialsLogic = new SerialsLogic();
                        return
                            !serialsLogic.Delete(recordId, User.Identity.GetUserId())
                                ? new JsonResult { Data = new { success = false, error = serialsLogic.ErrorMessage } }
                                : Json(new { success = true });
                    case (int)Defaults.CategoryBase.Indexes.Books:
                        var booksLogic = new BooksLogic();
                        return !booksLogic.Delete(recordId, User.Identity.GetUserId())
                            ? new JsonResult { Data = new { success = false, error = booksLogic.ErrorMessage } }
                            : Json(new { success = true });
                    case (int)Defaults.CategoryBase.Indexes.Roads:
                        var tracksLogic = new TracksLogic();
                        return !tracksLogic.Delete(recordId, User.Identity.GetUserId())
                            ? new JsonResult { Data = new { success = false, error = tracksLogic.ErrorMessage } }
                            : Json(new { success = true });
                    case (int)Defaults.CategoryBase.Indexes.Events:
                        var eventsLogic = new EventsLogic();
                        return !eventsLogic.Delete(recordId, User.Identity.GetUserId())
                            ? new JsonResult { Data = new { success = false, error = eventsLogic.ErrorMessage } }
                            : Json(new { success = true });
                }
                return Json("NOT REALIZED");
            }
            catch (Exception ex)
            {
                logger.Error(methodName, ex);
            }
            return new JsonResult { Data = new { success = false, error = methodName } };
        }
        [Authorize]
        [HttpPost]
        public JsonResult UpdateData(int pageId, string id, string name, string year, string datetime, string genre, string rating, string season, string series, string authors, string type, string coordinates, string distance, string desc, string complex)
        {
            var logger = new NLogLogger();
            var methodName = "public JsonResult UpdateData(int pageId, string name, string year, string datetime, string genre, string rating, string season, string series, string authors, string type, string coordinates, string distance, string desc, string complex)";
            try
            {
                switch (pageId)
                {
                    case (int)Defaults.CategoryBase.Indexes.Films:
                        var filmsLogic = new FilmsLogic();
                        return !filmsLogic.Update(id, name, year, datetime, genre, rating, User.Identity.GetUserId())
                            ? new JsonResult { Data = new { success = false, error = filmsLogic.ErrorMessage } }
                            : Json(new { success = true });
                    case (int)Defaults.CategoryBase.Indexes.Serials:
                        var serialsLogic = new SerialsLogic();
                        return
                            !serialsLogic.Update(id, name, year, season, series, datetime, genre, rating,
                                User.Identity.GetUserId())
                                ? new JsonResult { Data = new { success = false, error = serialsLogic.ErrorMessage } }
                                : Json(new { success = true });
                    case (int)Defaults.CategoryBase.Indexes.Books:
                        var booksLogic = new BooksLogic();
                        return !booksLogic.Update(id, name, year, authors, datetime, genre, rating, User.Identity.GetUserId())
                            ? new JsonResult { Data = new { success = false, error = booksLogic.ErrorMessage } }
                            : Json(new { success = true });
                    case (int)Defaults.CategoryBase.Indexes.Roads:
                        var tracksLogic = new TracksLogic();
                        return !tracksLogic.Update(id, name, datetime, type, coordinates, distance, User.Identity.GetUserId())
                            ? new JsonResult { Data = new { success = false, error = tracksLogic.ErrorMessage } }
                            : Json(new { success = true });
                    case (int)Defaults.CategoryBase.Indexes.Events:
                        var eventsLogic = new EventsLogic();
                        return !eventsLogic.Update(id, name, datetime, type, User.Identity.GetUserId())
                            ? new JsonResult { Data = new { success = false, error = eventsLogic.ErrorMessage } }
                            : Json(new { success = true });
                }
                return Json("NOT REALIZED");
            }
            catch (Exception ex)
            {
                logger.Error(methodName, ex);
            }
            return new JsonResult { Data = new { success = false, error = methodName } };
        }
        [Authorize]
        [HttpPost]
        public JsonResult AddSeries(string recordId)
        {
            var logger = new NLogLogger();
            var methodName = "public JsonResult AddSeries(string id)";
            try
            {
                var logic = new SerialsLogic();
                return !logic.AddSeries(recordId, User.Identity.GetUserId()) ? new JsonResult { Data = new { success = false, error = logic.ErrorMessage } } : Json(new { success = true });
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