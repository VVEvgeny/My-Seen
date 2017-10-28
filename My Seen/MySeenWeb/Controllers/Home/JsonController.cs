using System;
using System.Collections.Generic;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using MySeenLib;
using MySeenWeb.ActionFilters;
using MySeenWeb.Add_Code;
using MySeenWeb.Controllers._Base;
using MySeenWeb.Models;
using MySeenWeb.Models.Portal;
using MySeenWeb.Models.Prepared;
using MySeenWeb.Models.TablesLogic;
using MySeenWeb.Models.TablesLogic.Base;
using MySeenWeb.Models.TablesLogic.Portal;
using MySeenWeb.Models.Translations;
using MySeenWeb.Models.Translations.Portal;
using static MySeenLib.Defaults;

namespace MySeenWeb.Controllers.Home
{
    public class JsonController : BaseController
    {
        public JsonController(ICacheService cache):base(cache)
        {
            
        }

        [Compress]
        public ActionResult Index()
        {
            try
            {
                return View(new HomeViewModel(User.Identity.GetUserId(), MarkersOnRoads, Theme, EnableAnimation, Request, Cache));
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        [Compress] 
        [HttpPost]
        public JsonResult GetPage(int pageId, int? page, string search, int? ended, int? year, int? complex,
            string shareKey, int? road, int? id, string dateMan, string dateWoman, int? price, int? deals, int? salary,
            bool? bots, int? period)
        {
            try
            {
                switch (pageId)
                {
                    case (int) CategoryBase.Indexes.Films:
                        if (!User.Identity.IsAuthenticated && string.IsNullOrEmpty(shareKey))
                            return new JsonResult {Data = new {success = false, error = Resource.NotAuthorized}};
                        return
                            Json(new HomeViewModelFilms(User.Identity.GetUserId(), page ?? 1, Rpp, search, shareKey,
                                Cache));
                    case (int) CategoryBase.Indexes.Serials:
                        if (!User.Identity.IsAuthenticated && string.IsNullOrEmpty(shareKey))
                            return new JsonResult {Data = new {success = false, error = Resource.NotAuthorized}};
                        return
                            Json(new HomeViewModelSerials(User.Identity.GetUserId(), page ?? 1, Rpp, search, shareKey,
                                Cache));
                    case (int) CategoryBase.Indexes.Books:
                        if (!User.Identity.IsAuthenticated && string.IsNullOrEmpty(shareKey))
                          return new JsonResult {Data = new {success = false, error = Resource.NotAuthorized}};
                        return
                            Json(new HomeViewModelBooks(User.Identity.GetUserId(), page ?? 1, Rpp, search, shareKey,
                                Cache));
                    case (int) CategoryBase.Indexes.Events:
                        if (!User.Identity.IsAuthenticated && string.IsNullOrEmpty(shareKey))
                            return new JsonResult {Data = new {success = false, error = Resource.NotAuthorized}};
                        return
                            Json(new HomeViewModelEvents(User.Identity.GetUserId(), page ?? 1, Rpp, search, ended ?? 0,
                                shareKey));
                    case (int) CategoryBase.Indexes.Roads:
                        if (!User.Identity.IsAuthenticated && string.IsNullOrEmpty(shareKey))
                            return new JsonResult {Data = new {success = false, error = Resource.NotAuthorized}};
                        return
                            Json(new HomeViewModelRoads(User.Identity.GetUserId(), year ?? 0, search, shareKey, Cache,
                                road ?? 0));
                    case (int) CategoryBase.IndexesExt.Improvements:
                        if (!User.Identity.IsAuthenticated)
                            return new JsonResult {Data = new {success = false, error = Resource.NotAuthorized}};
                        return
                            Json(new HomeViewModelImprovements(User.Identity.GetUserId(),
                                complex ?? (int) ComplexBase.Indexes.All, page ?? 1, Rpp, search, ended ?? 0, Cache));

                    case (int) CategoryBase.IndexesExt.Users:
                        if (!User.Identity.IsAuthenticated && string.IsNullOrEmpty(shareKey))
                            return new JsonResult {Data = new {success = false, error = Resource.NotAuthorized}};
                        if (!UserRolesLogic.IsAdmin(User.Identity.GetUserId(), Cache))
                            return new JsonResult {Data = new {success = false, error = Resource.NoRights}};
                        return Json(new HomeViewModelUsers(User.Identity.GetUserId(), page ?? 1, Rpp, search, Cache));
                    case (int) CategoryBase.IndexesExt.Errors:
                        if (!User.Identity.IsAuthenticated && string.IsNullOrEmpty(shareKey))
                            return new JsonResult {Data = new {success = false, error = Resource.NotAuthorized}};
                        if (!UserRolesLogic.IsAdmin(User.Identity.GetUserId(), Cache))
                            return new JsonResult {Data = new {success = false, error = Resource.NoRights}};
                        return Json(new HomeViewModelErrors(page ?? 1, Rpp, search));
                    case (int) CategoryBase.IndexesExt.Logs:
                        if (!User.Identity.IsAuthenticated && string.IsNullOrEmpty(shareKey))
                            return new JsonResult {Data = new {success = false, error = Resource.NotAuthorized}};
                        if (!UserRolesLogic.IsAdmin(User.Identity.GetUserId(), Cache))
                            return new JsonResult {Data = new {success = false, error = Resource.NoRights}};
                        return Json(new HomeViewModelLogs(page ?? 1, Rpp, search, bots ?? false, period ?? 0, Cache));

                    case (int) CategoryBase.IndexesExt.Settings:
                        return Json(new HomeViewModelSettings(User.Identity.GetUserId(), Language, Rpp, Theme, MarkersOnRoads, EnableAnimation));

                    case (int) CategoryBase.IndexesMain.Memes:
                        return
                            Json(new PortalViewModelMemes(User.Identity.GetUserId(), page ?? 1, Rpp, search, id ?? 0,
                                Cache)); //Всегда по 20 на странице
                    case (int) CategoryBase.IndexesMain.Childs:
                        return Json(new PortalViewModelChildCalculator(year ?? 0, dateWoman, dateMan));
                    case (int) CategoryBase.IndexesMain.Realt:
                        return Json(new PortalViewModelRealt(year ?? 0, price ?? 0, deals ?? 0, salary ?? 0, Cache));
                }
                return new JsonResult {Data = new {success = false, error = "NOT REALIZED"}};
            }
            catch (Exception ex)
            {
                if (UserRolesLogic.IsAdmin(User.Identity.GetUserId(), Cache))
                    return new JsonResult {Data = new {success = false, error = ex.Message + "|" + ex.StackTrace}};
            }
            return new JsonResult {Data = new {success = false, error = System.Reflection.MethodBase.GetCurrentMethod().Name } };
        }

        [Compress]
        [HttpPost]
        public JsonResult GetPrepared(int pageId)
        {
            try
            {
                switch (pageId)
                {
                    case (int) CategoryBase.Indexes.Films:
                        return Json(new PreparedDataFilms());
                    case (int) CategoryBase.Indexes.Serials:
                        return Json(new PreparedDataSerials());
                    case (int) CategoryBase.Indexes.Books:
                        return Json(new PreparedDataBooks());
                    case (int) CategoryBase.Indexes.Events:
                        return Json(new PreparedDataEvents());
                    case (int) CategoryBase.Indexes.Roads:
                        return Json(new PreparedDataRoads());
                    case (int) CategoryBase.IndexesExt.Improvements:
                        return Json(new PreparedDataImprovements());
                }
                return Json("NOT REALIZED");
            }
            catch (Exception ex)
            {
            }
            return new JsonResult {Data = new {success = false, error = System.Reflection.MethodBase.GetCurrentMethod().Name } };
        }

        [Compress]
        [HttpPost]
        public JsonResult GetTranslation(int pageId)
        {
            try
            {
                switch (pageId)
                {
                    case (int) CategoryBase.Indexes.Films:
                        return Json(new TranslationDataFilms());
                    case (int) CategoryBase.Indexes.Serials:
                        return Json(new TranslationDataSerials());
                    case (int) CategoryBase.Indexes.Books:
                        return Json(new TranslationDataBooks());
                    case (int) CategoryBase.Indexes.Events:
                        return Json(new TranslationDataEvents());
                    case (int) CategoryBase.IndexesExt.Users:
                        if (!UserRolesLogic.IsAdmin(User.Identity.GetUserId(), Cache))
                            return new JsonResult {Data = new {success = false, error = Resource.NoRights}};
                        return Json(new TranslationDataUsers());
                    case (int) CategoryBase.IndexesExt.Logs:
                        if (!UserRolesLogic.IsAdmin(User.Identity.GetUserId(), Cache))
                            return new JsonResult {Data = new {success = false, error = Resource.NoRights}};
                        return Json(new TranslationDataLogs());
                    case (int) CategoryBase.IndexesExt.Errors:
                        if (!UserRolesLogic.IsAdmin(User.Identity.GetUserId(), Cache))
                            return new JsonResult {Data = new {success = false, error = Resource.NoRights}};
                        return Json(new TranslationDataErrors());
                    case (int) CategoryBase.Indexes.Roads:
                        return Json(new TranslationDataRoads());
                    case (int) CategoryBase.IndexesExt.Improvements:
                        return Json(new TranslationDataImprovements());
                    case (int) CategoryBase.IndexesExt.Settings:
                        return Json(new TranslationDataSettings());
                    case (int) CategoryBase.IndexesMain.Main:
                        return Json(new TranslationDataPortalMain());
                    case (int) CategoryBase.IndexesMain.Memes:
                        return Json(new TranslationDataPortalMemes());
                    case (int) CategoryBase.IndexesMain.Childs:
                        return Json(new TranslationDataPortalChildSexCalculator());
                    case (int) CategoryBase.IndexesMain.Realt:
                        return Json(new TranslationDataPortalRealt());
                    case (int) CategoryBase.IndexesMain.Imt:
                        return Json(new TranslationDataPortalImtCalculator());
                }
                return new JsonResult {Data = new {success = false, error = "NOT REALIZED"}};
            }
            catch (Exception ex)
            {
            }
            return new JsonResult {Data = new {success = false, error = System.Reflection.MethodBase.GetCurrentMethod().Name } };
        }

        [Compress]
        [Authorize]
        [HttpPost]
        public JsonResult GetShare(int pageId, string recordId)
        {
            try
            {
                switch (pageId)
                {
                    case (int) CategoryBase.Indexes.Films:
                        var filmsLogic = new FilmsLogic(Cache);
                        return Json(filmsLogic.GetShare(recordId, User.Identity.GetUserId()));
                    case (int) CategoryBase.Indexes.Serials:
                        var serialsLogic = new SerialsLogic(Cache);
                        return Json(serialsLogic.GetShare(recordId, User.Identity.GetUserId()));
                    case (int) CategoryBase.Indexes.Books:
                        var booksLogic = new BooksLogic(Cache);
                        return Json(booksLogic.GetShare(recordId, User.Identity.GetUserId()));
                    case (int) CategoryBase.Indexes.Events:
                        var eventsLogic = new EventsLogic();
                        return Json(eventsLogic.GetShare(recordId, User.Identity.GetUserId()));
                    case (int) CategoryBase.Indexes.Roads:
                        var roadsLogic = new RoadsLogic(Cache);
                        return Json(roadsLogic.GetShare(recordId, User.Identity.GetUserId()));
                }
            }
            catch (Exception ex)
            {
            }
            return Json("-");
        }

        [Compress]
        [Authorize]
        [HttpPost]
        public JsonResult GenerateShare(int pageId, string recordId)
        {
            try
            {
                switch (pageId)
                {
                    case (int) CategoryBase.Indexes.Films:
                        var filmsLogic = new FilmsLogic(Cache);
                        return Json(filmsLogic.GenerateShare(recordId, User.Identity.GetUserId()));
                    case (int) CategoryBase.Indexes.Serials:
                        var serialsLogic = new SerialsLogic(Cache);
                        return Json(serialsLogic.GenerateShare(recordId, User.Identity.GetUserId()));
                    case (int) CategoryBase.Indexes.Books:
                        var booksLogic = new BooksLogic(Cache);
                        return Json(booksLogic.GenerateShare(recordId, User.Identity.GetUserId()));
                    case (int) CategoryBase.Indexes.Events:
                        var eventsLogic = new EventsLogic();
                        return Json(eventsLogic.GenerateShare(recordId, User.Identity.GetUserId()));
                    case (int) CategoryBase.Indexes.Roads:
                        var roadsLogic = new RoadsLogic(Cache);
                        return Json(roadsLogic.GenerateShare(recordId, User.Identity.GetUserId()));
                }
            }
            catch (Exception ex)
            {
            }
            return Json("-");
        }

        [Compress]
        [Authorize]
        [HttpPost]
        public JsonResult DeleteShare(int pageId, string recordId)
        {
            try
            {
                switch (pageId)
                {
                    case (int) CategoryBase.Indexes.Films:
                        var filmsLogic = new FilmsLogic(Cache);
                        return Json(filmsLogic.DeleteShare(recordId, User.Identity.GetUserId()));
                    case (int) CategoryBase.Indexes.Serials:
                        var serialsLogic = new SerialsLogic(Cache);
                        return Json(serialsLogic.DeleteShare(recordId, User.Identity.GetUserId()));
                    case (int) CategoryBase.Indexes.Books:
                        var booksLogic = new BooksLogic(Cache);
                        return Json(booksLogic.DeleteShare(recordId, User.Identity.GetUserId()));
                    case (int) CategoryBase.Indexes.Events:
                        var eventsLogic = new EventsLogic();
                        return Json(eventsLogic.DeleteShare(recordId, User.Identity.GetUserId()));
                    case (int) CategoryBase.Indexes.Roads:
                        var roadsLogic = new RoadsLogic(Cache);
                        return Json(roadsLogic.DeleteShare(recordId, User.Identity.GetUserId()));
                }
            }
            catch (Exception ex)
            {
            }
            return Json("-");
        }

        [Compress]
        [Authorize]
        [HttpPost]
        public JsonResult AddData(int pageId, string name, string year, string datetime, string genre, string rating,
            string season
            , string series, string authors, string type, string coordinates, string distance, string link, string other)
        {
            try
            {
                switch (pageId)
                {
                    case (int) CategoryBase.Indexes.Films:
                        var filmsLogic = new FilmsLogic(Cache);
                        return !filmsLogic.Add(name, year, datetime, genre, rating, User.Identity.GetUserId())
                            ? new JsonResult {Data = new {success = false, error = filmsLogic.ErrorMessage}}
                            : Json(new {success = true});
                    case (int) CategoryBase.Indexes.Serials:
                        var serialsLogic = new SerialsLogic(Cache);
                        return
                            !serialsLogic.Add(name, year, season, series, datetime, genre, rating,
                                User.Identity.GetUserId())
                                ? new JsonResult {Data = new {success = false, error = serialsLogic.ErrorMessage}}
                                : Json(new {success = true});
                    case (int) CategoryBase.Indexes.Books:
                        var booksLogic = new BooksLogic(Cache);
                        return !booksLogic.Add(name, year, authors, datetime, genre, rating, User.Identity.GetUserId())
                            ? new JsonResult {Data = new {success = false, error = booksLogic.ErrorMessage}}
                            : Json(new {success = true});
                    case (int) CategoryBase.Indexes.Roads:
                        var tracksLogic = new RoadsLogic(Cache);
                        return !tracksLogic.Add(name, datetime, type, coordinates, distance, User.Identity.GetUserId())
                            ? new JsonResult {Data = new {success = false, error = tracksLogic.ErrorMessage}}
                            : Json(new {success = true});
                    case (int) CategoryBase.Indexes.Events:
                        var eventsLogic = new EventsLogic();
                        return !eventsLogic.Add(name, datetime, type, User.Identity.GetUserId())
                            ? new JsonResult {Data = new {success = false, error = eventsLogic.ErrorMessage}}
                            : Json(new {success = true});
                    case (int) CategoryBase.IndexesExt.Improvements:
                        var improvementLogic = new ImprovementLogic();
                        return !improvementLogic.Add(name, type, User.Identity.GetUserId())
                            ? new JsonResult {Data = new {success = false, error = improvementLogic.ErrorMessage}}
                            : Json(new {success = true});
                    case (int) CategoryBase.IndexesMain.Memes:
                        var memesLogic = new MemesLogic(Cache);
                        return !memesLogic.Add(name, link, User.Identity.GetUserId())
                            ? new JsonResult {Data = new {success = false, error = memesLogic.ErrorMessage}}
                            : Json(new {success = true});

                    case (int) CategoryBase.IndexesMain.Realt:
                        if (!UserRolesLogic.IsAdmin(User.Identity.GetUserId(), Cache))
                            return new JsonResult {Data = new {success = false, error = Resource.NoRights}};

                        var realLogic = new SalaryLogic(Cache);
                        return !realLogic.Add(name, datetime, other)
                            ? new JsonResult {Data = new {success = false, error = realLogic.ErrorMessage}}
                            : Json(new {success = true});
                }
                return Json("NOT REALIZED");
            }
            catch (Exception ex)
            {
                if (UserRolesLogic.IsAdmin(User.Identity.GetUserId(), Cache))
                    return new JsonResult { Data = new { success = false, error = ex.Message + "|" + ex.StackTrace } };
            }
            return new JsonResult {Data = new {success = false, error = System.Reflection.MethodBase.GetCurrentMethod().Name } };
        }

        [Compress]
        [Authorize]
        [HttpPost]
        public JsonResult DeleteData(int pageId, string recordId)
        {
            try
            {
                //verify
                switch (pageId)
                {
                    case (int) CategoryBase.IndexesExt.Improvements:
                    case (int) CategoryBase.IndexesMain.Memes:
                        if (!UserRolesLogic.IsAdmin(User.Identity.GetUserId(), Cache))
                            return new JsonResult {Data = new {success = false, error = Resource.NoRights}};
                        break;
                }
                //action
                var logic = BaseLogicFactory.Create(pageId, Cache);
                if (logic != null)
                {
                    return !logic.Delete(recordId, User.Identity.GetUserId())
                        ? new JsonResult {Data = new {success = false, error = logic.GetError()}}
                        : Json(new {success = true});
                }
                return Json("NOT REALIZED");
            }
            catch (Exception ex)
            {
            }
            return new JsonResult {Data = new {success = false, error = System.Reflection.MethodBase.GetCurrentMethod().Name } };
        }

        [Compress]
        [Authorize]
        [HttpPost]
        public JsonResult UpdateData(int pageId, string id, string name, string year, string datetime, string genre,
            string rating, string season, string series, string authors, string type, string coordinates,
            string distance)
        {
            try
            {
                switch (pageId)
                {
                    case (int) CategoryBase.Indexes.Films:
                        var filmsLogic = new FilmsLogic(Cache);
                        return !filmsLogic.Update(id, name, year, datetime, genre, rating, User.Identity.GetUserId())
                            ? new JsonResult {Data = new {success = false, error = filmsLogic.ErrorMessage}}
                            : Json(new {success = true});
                    case (int) CategoryBase.Indexes.Serials:
                        var serialsLogic = new SerialsLogic(Cache);
                        return
                            !serialsLogic.Update(id, name, year, season, series, datetime, genre, rating,
                                User.Identity.GetUserId())
                                ? new JsonResult {Data = new {success = false, error = serialsLogic.ErrorMessage}}
                                : Json(new {success = true});
                    case (int) CategoryBase.Indexes.Books:
                        var booksLogic = new BooksLogic(Cache);
                        return
                            !booksLogic.Update(id, name, year, authors, datetime, genre, rating,
                                User.Identity.GetUserId())
                                ? new JsonResult {Data = new {success = false, error = booksLogic.ErrorMessage}}
                                : Json(new {success = true});
                    case (int) CategoryBase.Indexes.Roads:
                        var tracksLogic = new RoadsLogic(Cache);
                        return
                            !tracksLogic.Update(id, name, datetime, type, coordinates, distance,
                                User.Identity.GetUserId())
                                ? new JsonResult {Data = new {success = false, error = tracksLogic.ErrorMessage}}
                                : Json(new {success = true});
                    case (int) CategoryBase.Indexes.Events:
                        var eventsLogic = new EventsLogic();
                        return !eventsLogic.Update(id, name, datetime, type, User.Identity.GetUserId())
                            ? new JsonResult {Data = new {success = false, error = eventsLogic.ErrorMessage}}
                            : Json(new {success = true});
                    case (int) CategoryBase.IndexesExt.Improvements:
                        if (!UserRolesLogic.IsAdmin(User.Identity.GetUserId(), Cache))
                            return new JsonResult {Data = new {success = false, error = Resource.NoRights}};
                        var improvementsLogic = new ImprovementLogic();
                        return !improvementsLogic.Update(id, name, type, User.Identity.GetUserId())
                            ? new JsonResult {Data = new {success = false, error = improvementsLogic.ErrorMessage}}
                            : Json(new {success = true});
                }
                return Json("NOT REALIZED");
            }
            catch (Exception ex)
            {
            }
            return new JsonResult {Data = new {success = false, error = System.Reflection.MethodBase.GetCurrentMethod().Name } };
        }

        [Compress]
        [Authorize]
        [IsAdmin]
        [HttpPost]
        public JsonResult EndImprovement(string id, string name, string version)
        {
            try
            {
                var logic = new ImprovementLogic();
                return !logic.End(id, name, version)
                    ? new JsonResult {Data = new {success = false, error = logic.ErrorMessage}}
                    : Json(new {success = true});
            }
            catch (Exception ex)
            {
            }
            return new JsonResult {Data = new {success = false, error = System.Reflection.MethodBase.GetCurrentMethod().Name } };
        }

        [Compress]
        [Authorize]
        [IsAdmin]
        [HttpPost]
        public JsonResult RemoveAllError()
        {
            try
            {
                var logic = new ErrorsLogic();

                return !logic.RemoveAll()
                    ? new JsonResult {Data = new {success = false, error = logic.ErrorMessage}}
                    : Json(new {success = true});
            }
            catch (Exception ex)
            {
            }
            return new JsonResult {Data = new {success = false, error = System.Reflection.MethodBase.GetCurrentMethod().Name } };
        }

        [Compress]
        [Authorize]
        [HttpPost]
        public JsonResult AddSeries(string recordId)
        {
            try
            {
                var logic = new SerialsLogic(Cache);
                return !logic.AddSeries(recordId, User.Identity.GetUserId())
                    ? new JsonResult {Data = new {success = false, error = logic.ErrorMessage}}
                    : Json(new {success = true});
            }
            catch (Exception ex)
            {
            }
            return new JsonResult {Data = new {success = false, error = System.Reflection.MethodBase.GetCurrentMethod().Name } };
        }

        [Compress]
        [Authorize]
        [IsAdmin]
        [HttpPost]
        public JsonResult UpdateUser(string name, IEnumerable<string> roles)
        {
            try
            {
                var logic = new UserRolesLogic(Cache);
                return !logic.Update(name, roles)
                    ? new JsonResult {Data = new {success = false, error = logic.ErrorMessage}}
                    : Json(new {success = true});
            }
            catch (Exception ex)
            {
            }
            return new JsonResult {Data = new {success = false, error = System.Reflection.MethodBase.GetCurrentMethod().Name } };
        }
    }
}