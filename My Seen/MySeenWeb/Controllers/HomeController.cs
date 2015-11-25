using System;
using System.Linq;
using System.Web.Mvc;
using MySeenWeb.Models;
using Microsoft.AspNet.Identity;
using MySeenLib;
using MySeenWeb.ActionFilters;
using MySeenWeb.Models.Tables;
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

            if (!User.Identity.IsAuthenticated) return View();
            return View(new HomeViewModel(
                ReadCookie(CookieKeys.HomeCategory, Defaults.CategoryBase.FilmIndex).ToString(),
                User.Identity.GetUserId(),
                page ?? 1,
                Rpp,
                ReadCookie(CookieKeys.ImprovementsCategory, Defaults.ComplexBase.IndexAll),
                search
                ));
        }
        [Authorize]
        [HttpPost]
        public JsonResult ChangeCookies(string selected)
        {
            WriteCookie(CookieKeys.HomeCategory, selected);
            WriteCookie(CookieKeys.HomeCategoryPrev, selected);
            return Json(new { success = true });
        }
        [Authorize]
        public JsonResult ChangeCookiesImprovement(string selected)
        {
            WriteCookie(CookieKeys.ImprovementsCategory, selected);
            return Json(new { success = true });
        }
        [Authorize]
        [HttpPost]
        public JsonResult AddFilm(string name, string datetime, string genre, string rating)
        {
            LogSave.Save(User.Identity.IsAuthenticated ? User.Identity.GetUserId() : "", Request.UserHostAddress, Request.UserAgent, "Home/AddFilm", name);
            var errorMessage = string.Empty;
            var userId = User.Identity.GetUserId();
            var ac = new ApplicationDbContext();
            
            if (string.IsNullOrEmpty(errorMessage) && string.IsNullOrEmpty(name))errorMessage = Resource.EnterFilmName;
            if (string.IsNullOrEmpty(errorMessage) && string.IsNullOrEmpty(datetime)) errorMessage = "datetime";
            if (string.IsNullOrEmpty(errorMessage) && ac.Films.Any(f => f.Name == name && f.UserId == userId))errorMessage = Resource.FilmNameAlreadyExists;

            if (string.IsNullOrEmpty(errorMessage))
            {
                try
                {
                    var f = new Films { Name = name, Genre = Convert.ToInt32(genre), Rating = Convert.ToInt32(rating), DateSee = UmtTime.To(Convert.ToDateTime(datetime)), DateChange = UmtTime.To(DateTime.Now), UserId = userId };
                    ac.Films.Add(f);
                    ac.SaveChanges();
                }
                catch (Exception e)
                {
                    errorMessage = Resource.ErrorWorkWithDB + "=" + e.Message;
                }
            }
            if (!string.IsNullOrEmpty(errorMessage))
            {
                return new JsonResult { Data = new { success = false, error = errorMessage } };
            }
            return Json(new { success = true });
        }
        [Authorize]
        [HttpPost]
        public JsonResult EditFilm(string id, string name, string datetime, string genre, string rating)
        {
            LogSave.Save(User.Identity.IsAuthenticated ? User.Identity.GetUserId() : "", Request.UserHostAddress, Request.UserAgent, "Home/EditFilm", id);
            var errorMessage = string.Empty;
            var userId = User.Identity.GetUserId();
            var ac = new ApplicationDbContext();
            var iid = (Convert.ToInt32(id));

            if (string.IsNullOrEmpty(errorMessage) && string.IsNullOrEmpty(name)) errorMessage = Resource.EnterFilmName;
            if (string.IsNullOrEmpty(errorMessage) && string.IsNullOrEmpty(datetime)) errorMessage = "datetime";
            if (string.IsNullOrEmpty(errorMessage) && ac.Films.Any(f => f.Name == name && f.UserId == userId && f.Id != iid)) errorMessage = Resource.FilmNameAlreadyExists;

            if (string.IsNullOrEmpty(errorMessage))
            {
                try
                {
                    var film = ac.Films.First(f => f.UserId == userId && f.Id == iid);
                    film.Name = name;
                    film.Genre = Convert.ToInt32(genre);
                    film.Rating = Convert.ToInt32(rating);
                    film.DateChange = UmtTime.To(DateTime.Now);
                    film.DateSee = UmtTime.To(Convert.ToDateTime(datetime));
                    ac.SaveChanges();
                }
                catch (Exception e)
                {
                    errorMessage = Resource.ErrorWorkWithDB + "=" + e.Message;
                }
            }
            return !string.IsNullOrEmpty(errorMessage) ? new JsonResult { Data = new { success = false, error = errorMessage } } : Json(new { success = true });
        }
        [Authorize]
        [HttpPost]
        public JsonResult AddSerial(string name, string season, string series, string datetime, string genre, string rating)
        {
            LogSave.Save(User.Identity.IsAuthenticated ? User.Identity.GetUserId() : "", Request.UserHostAddress, Request.UserAgent, "Home/AddSerial", name);
            var errorMessage = string.Empty;
            var userId = User.Identity.GetUserId();
            var ac = new ApplicationDbContext();

            if (string.IsNullOrEmpty(errorMessage) && string.IsNullOrEmpty(name)) errorMessage = Resource.EnterSerialName;
            if (string.IsNullOrEmpty(errorMessage) && string.IsNullOrEmpty(datetime))errorMessage = "datetime";
            if (string.IsNullOrEmpty(errorMessage) && ac.Serials.Any(f => f.Name == name && f.UserId == userId)) errorMessage = Resource.SerialNameAlreadyExists;

            if (string.IsNullOrEmpty(errorMessage))
            {
                try
                {
                    if (string.IsNullOrEmpty(season)) season = "1";
                    if (string.IsNullOrEmpty(series)) series = "1";
                    var s = new Serials { Name = name, LastSeason = Convert.ToInt32(season), LastSeries = Convert.ToInt32(series), Genre = Convert.ToInt32(genre), Rating = Convert.ToInt32(rating), DateBegin = UmtTime.To(Convert.ToDateTime(datetime)), DateLast = UmtTime.To(DateTime.Now), DateChange = UmtTime.To(DateTime.Now), UserId = userId };
                    ac.Serials.Add(s);
                    ac.SaveChanges();
                }
                catch (Exception e)
                {
                    errorMessage = Resource.ErrorWorkWithDB + "=" + e.Message;
                }
            }
            return !string.IsNullOrEmpty(errorMessage) ? new JsonResult { Data = new { success = false, error = errorMessage } } : Json(new { success = true });
        }
        [Authorize]
        [HttpPost]
        public JsonResult EditSerial(string id, string name, string season, string series, string datetime, string genre, string rating)
        {
            LogSave.Save(User.Identity.IsAuthenticated ? User.Identity.GetUserId() : "", Request.UserHostAddress, Request.UserAgent, "Home/EditSerial", id);
            var errorMessage = string.Empty;
            var userId = User.Identity.GetUserId();
            var ac = new ApplicationDbContext();
            var iid = (Convert.ToInt32(id));

            if (string.IsNullOrEmpty(errorMessage) && string.IsNullOrEmpty(name))errorMessage = Resource.EnterSerialName;
            if (string.IsNullOrEmpty(errorMessage) && string.IsNullOrEmpty(datetime)) errorMessage = "datetime";
            if (string.IsNullOrEmpty(errorMessage) && ac.Serials.Any(f => f.Name == name && f.UserId == userId && f.Id != iid)) errorMessage = Resource.FilmNameAlreadyExists;

            if (string.IsNullOrEmpty(errorMessage))
            {
                try
                {
                    var film = ac.Serials.First(f => f.UserId == userId && f.Id == iid);
                    film.Name = name;
                    if (film.LastSeason != Convert.ToInt32(season) || film.LastSeries != Convert.ToInt32(series))
                    {
                        film.DateLast = UmtTime.To(DateTime.Now);
                    }
                    film.LastSeason = Convert.ToInt32(season);
                    film.LastSeries = Convert.ToInt32(series);
                    film.Genre = Convert.ToInt32(genre);
                    film.Rating = Convert.ToInt32(rating);
                    film.DateChange = UmtTime.To(DateTime.Now);
                    film.DateBegin = UmtTime.To(Convert.ToDateTime(datetime));
                    ac.SaveChanges();
                }
                catch (Exception e)
                {
                    errorMessage = Resource.ErrorWorkWithDB + "=" + e.Message;
                }
            }
            return !string.IsNullOrEmpty(errorMessage) ? new JsonResult { Data = new { success = false, error = errorMessage } } : Json(new { success = true });
        }
        [Authorize]
        [HttpPost]
        public JsonResult AddBook(string name, string authors, string datetime, string genre, string rating)
        {
            LogSave.Save(User.Identity.IsAuthenticated ? User.Identity.GetUserId() : "", Request.UserHostAddress, Request.UserAgent, "Home/AddBook", name);
            var errorMessage = string.Empty;
            var userId = User.Identity.GetUserId();
            var ac = new ApplicationDbContext();

            if (string.IsNullOrEmpty(errorMessage) && string.IsNullOrEmpty(authors))errorMessage = Resource.EnterBookName;
            if (string.IsNullOrEmpty(errorMessage) && string.IsNullOrEmpty(authors))errorMessage = Resource.EnterBookAuthors;
            if (string.IsNullOrEmpty(errorMessage) && string.IsNullOrEmpty(datetime))errorMessage = "Чота дата кривая";
            if (string.IsNullOrEmpty(errorMessage) && ac.Books.Any(f => f.Name == name && f.UserId == userId)) errorMessage = Resource.BookNameAlreadyExists;

            if (string.IsNullOrEmpty(errorMessage))
            {
                try
                {
                    var f = new Books { Name = name, Authors = authors, Genre = Convert.ToInt32(genre), Rating = Convert.ToInt32(rating), DateRead = UmtTime.To(Convert.ToDateTime(datetime)), DateChange = UmtTime.To(DateTime.Now), UserId = userId };
                    ac.Books.Add(f);
                    ac.SaveChanges();
                }
                catch (Exception e)
                {
                    errorMessage = Resource.ErrorWorkWithDB + "=" + e.Message;
                }
            }
            return !string.IsNullOrEmpty(errorMessage) ? new JsonResult { Data = new { success = false, error = errorMessage } } : Json(new { success = true });
        }
        [Authorize]
        [HttpPost]
        public JsonResult EditBook(string id, string name, string authors,string datetime, string genre, string rating)
        {
            LogSave.Save(User.Identity.IsAuthenticated ? User.Identity.GetUserId() : "", Request.UserHostAddress, Request.UserAgent, "Home/EditBook", id);
            var errorMessage = string.Empty;
            var userId = User.Identity.GetUserId();
            var ac = new ApplicationDbContext();
            var iid = Convert.ToInt32(id);

            if (string.IsNullOrEmpty(errorMessage) && string.IsNullOrEmpty(authors))errorMessage = Resource.EnterBookName;
            if (string.IsNullOrEmpty(errorMessage) && string.IsNullOrEmpty(authors))errorMessage = Resource.EnterBookAuthors;
            if (string.IsNullOrEmpty(errorMessage) && string.IsNullOrEmpty(datetime))errorMessage = "Чота дата кривая";
            if (string.IsNullOrEmpty(errorMessage) && ac.Books.Any(f => f.Name == name && f.UserId == userId && f.Id != iid)) errorMessage = Resource.BookNameAlreadyExists;

            if (string.IsNullOrEmpty(errorMessage))
            {
                try
                {
                    var film = ac.Books.First(f => f.UserId == userId && f.Id == iid);
                    film.Name = name;
                    film.Authors = authors;
                    film.Genre = Convert.ToInt32(genre);
                    film.Rating = Convert.ToInt32(rating);
                    film.DateRead = UmtTime.To(Convert.ToDateTime(datetime));
                    film.DateChange = UmtTime.To(DateTime.Now);
                    ac.SaveChanges();
                }
                catch (Exception e)
                {
                    errorMessage = Resource.ErrorWorkWithDB + "=" + e.Message;
                }
            }
            return !string.IsNullOrEmpty(errorMessage) ? new JsonResult { Data = new { success = false, error = errorMessage } } : Json(new { success = true });
        }
        [Authorize]
        [HttpPost]
        public JsonResult DeleteFilm(string id)
        {
            LogSave.Save(User.Identity.IsAuthenticated ? User.Identity.GetUserId() : "", Request.UserHostAddress, Request.UserAgent, "Home/DeleteFilm", id);
            var errorMessage = string.Empty;
            var userId = User.Identity.GetUserId();
            var ac = new ApplicationDbContext();
            var iid = (Convert.ToInt32(id));

            if (string.IsNullOrEmpty(errorMessage))
            {
                try
                {
                    var film = ac.Films.First(f => f.UserId == userId && f.Id == iid);
                    film.isDeleted = true;
                    ac.SaveChanges();
                }
                catch (Exception e)
                {
                    errorMessage = Resource.ErrorWorkWithDB + "=" + e.Message;
                }
            }
            return !string.IsNullOrEmpty(errorMessage) ? new JsonResult { Data = new { success = false, error = errorMessage } } : Json(new { success = true });
        }
        [Authorize]
        [HttpPost]
        public JsonResult DeleteSerial(string id)
        {
            LogSave.Save(User.Identity.IsAuthenticated ? User.Identity.GetUserId() : "", Request.UserHostAddress, Request.UserAgent, "Home/DeleteSerial", id);
            var errorMessage = string.Empty;
            var userId = User.Identity.GetUserId();
            var ac = new ApplicationDbContext();
            var iid = (Convert.ToInt32(id));

            if (string.IsNullOrEmpty(errorMessage))
            {
                try
                {
                    var film = ac.Serials.First(f => f.UserId == userId && f.Id == iid);
                    film.isDeleted = true;
                    ac.SaveChanges();
                }
                catch (Exception e)
                {
                    errorMessage = Resource.ErrorWorkWithDB + "=" + e.Message;
                }
            }
            return !string.IsNullOrEmpty(errorMessage) ? new JsonResult { Data = new { success = false, error = errorMessage } } : Json(new { success = true });
        }
        [Authorize]
        [HttpPost]
        public JsonResult DeleteBook(string id)
        {
            LogSave.Save(User.Identity.IsAuthenticated ? User.Identity.GetUserId() : "", Request.UserHostAddress, Request.UserAgent, "Home/DeleteBook", id);
            var errorMessage = string.Empty;
            var userId = User.Identity.GetUserId();
            var ac = new ApplicationDbContext();
            var iid = (Convert.ToInt32(id));

            if (string.IsNullOrEmpty(errorMessage))
            {
                try
                {
                    var film = ac.Books.First(f => f.UserId == userId && f.Id == iid);
                    film.isDeleted = true;
                    ac.SaveChanges();
                }
                catch (Exception e)
                {
                    errorMessage = Resource.ErrorWorkWithDB + "=" + e.Message;
                }
            }
            return !string.IsNullOrEmpty(errorMessage) ? new JsonResult { Data = new { success = false, error = errorMessage } } : Json(new { success = true });
        }
        [Authorize]
        [HttpPost]
        public JsonResult AddImprovement(string desc, string complex)
        {
            LogSave.Save(User.Identity.IsAuthenticated ? User.Identity.GetUserId() : "", Request.UserHostAddress, Request.UserAgent, "Home/AddImprovement", desc);
            var errorMessage = string.Empty;
            var ac = new ApplicationDbContext();
            var id = -1;

            if (string.IsNullOrEmpty(errorMessage) && desc.Length == 0)errorMessage = Resource.DescToShort;
            if (string.IsNullOrEmpty(errorMessage))
            {
                try
                {
                    id = Convert.ToInt32(complex);
                    if (id < 0) throw new Exception();
                }
                catch
                {
                    errorMessage = "Корявая айдишка";
                }
            }
            if (string.IsNullOrEmpty(errorMessage) && ac.Bugs.Any(f => f.Text == desc && f.Complex == id)) errorMessage = Resource.BugAlreadyExists;
            if (string.IsNullOrEmpty(errorMessage))
            {
                try
                {
                    ac.Bugs.Add(new Bugs { Text = desc, DateFound = DateTime.Now, UserId = User.Identity.GetUserId(), Complex = id });
                    ac.SaveChanges();
                }
                catch (Exception e)
                {
                    errorMessage = Resource.ErrorWorkWithDB + "=" + e.Message;
                }
            }
            return !string.IsNullOrEmpty(errorMessage) ? new JsonResult { Data = new { success = false, error = errorMessage } } : Json(new { success = true });
        }
        [Authorize]
        [HttpPost]
        public JsonResult EndImprovement(string id, string desc, string version)
        {
            LogSave.Save(User.Identity.IsAuthenticated ? User.Identity.GetUserId() : "", Request.UserHostAddress, Request.UserAgent, "Home/EndImprovement", id + " " + desc + " " + version);
            var errorMessage = string.Empty;
            var ac = new ApplicationDbContext();
            var recordId = -1;
            var versionNum = -1;

            if (string.IsNullOrEmpty(errorMessage) && desc.Length == 0) errorMessage = Resource.DescToShort;
            if (string.IsNullOrEmpty(errorMessage))
            {
                try
                {
                    recordId = Convert.ToInt32(id);
                    if (recordId < 0) throw new Exception();
                }
                catch
                {
                    errorMessage = Resource.WrongId;
                }
            }
            if (string.IsNullOrEmpty(errorMessage))
            {
                try
                {
                    versionNum = Convert.ToInt32(version);
                    if (versionNum < 0) throw new Exception();
                }
                catch
                {
                    errorMessage = Resource.WrongVersion;
                }
            }
            if (string.IsNullOrEmpty(errorMessage))
            {
                try
                {
                    var bug = ac.Bugs.First(b => b.Id == recordId);
                    bug.TextEnd = desc;
                    bug.DateEnd = DateTime.Now;
                    bug.Version = versionNum;
                    ac.SaveChanges();
                }
                catch (Exception e)
                {
                    errorMessage = Resource.ErrorWorkWithDB + "=" + e.Message;
                }
            }
            return !string.IsNullOrEmpty(errorMessage) ? new JsonResult { Data = new { success = false, error = errorMessage } } : Json(new { success = true });
        }
        [Authorize]
        [HttpPost]
        public JsonResult DeleteImprovement(string id)
        {
            LogSave.Save(User.Identity.IsAuthenticated ? User.Identity.GetUserId() : "", Request.UserHostAddress, Request.UserAgent, "Home/DeleteImprovement", id);
            var errorMessage = string.Empty;
            var ac = new ApplicationDbContext();
            var recordId = 0;

            if (string.IsNullOrEmpty(errorMessage))
            {
                recordId = Convert.ToInt32(id);
                try
                {
                    if (recordId < 0) throw new Exception();
                }
                catch
                {
                    errorMessage = "Корявая айдишка";
                }
            }
            if (string.IsNullOrEmpty(errorMessage))
            {
                try
                {
                    ac.Bugs.RemoveRange(ac.Bugs.Where(b => b.Id == recordId));
                    ac.SaveChanges();
                }
                catch (Exception e)
                {
                    errorMessage = Resource.ErrorWorkWithDB + "=" + e.Message;
                }
            }
            if (!string.IsNullOrEmpty(errorMessage))
            {
                return new JsonResult { Data = new { success = false, error = errorMessage } };
            }
            return Json(new { success = true });
        }
        [Authorize]
        public ActionResult GetTrack(int id)
        {
            if (User.Identity.IsAuthenticated)
            {
                return Json(HomeViewModelTracks.GetTrack(id, User.Identity.GetUserId()), JsonRequestBehavior.AllowGet);
            }
            return RedirectToAction("Index");
        }

        public ActionResult GetTrackByKey(string id)
        {
            return Json(HomeViewModelTracks.GetTrackByKey(id), JsonRequestBehavior.AllowGet);
        }
        
        [Authorize]
        public ActionResult GetTrackNameById(int id)
        {
            if (User.Identity.IsAuthenticated)
            {
                return Json(HomeViewModelTracks.GetTrackNameById(id, User.Identity.GetUserId()), JsonRequestBehavior.AllowGet);
            }
            return RedirectToAction("Index");
        }
        [Authorize]
        public ActionResult GetTrackDateById(int id)
        {
            if (User.Identity.IsAuthenticated)
            {
                return Json(HomeViewModelTracks.GetTrackDateById(id, User.Identity.GetUserId()), JsonRequestBehavior.AllowGet);
            }
            return RedirectToAction("Index");
        }
        [Authorize]
        public ActionResult GetTrackShare(string id)
        {
            if (User.Identity.IsAuthenticated)
            {
                return Json(HomeViewModelTracks.GetTrackShare(id, User.Identity.GetUserId()), JsonRequestBehavior.AllowGet);
            }
            return RedirectToAction("Index");
        }
        [Authorize]
        public ActionResult GenerateTrackShare(string id)
        {
            if (User.Identity.IsAuthenticated)
            {
                return Json(HomeViewModelTracks.GenerateTrackShare(id, User.Identity.GetUserId()), JsonRequestBehavior.AllowGet);
            }
            return RedirectToAction("Index");
        }
        [Authorize]
        public ActionResult DeleteTrackShare(string id)
        {
            if (User.Identity.IsAuthenticated)
            {
                HomeViewModelTracks.DeleteTrackShare(id, User.Identity.GetUserId());
                return Json(new { success = true }, JsonRequestBehavior.AllowGet);
            }
            return RedirectToAction("Index");
        }
        [Authorize]
        public ActionResult DeleteTrack(string id)
        {
            if (User.Identity.IsAuthenticated)
            {
                HomeViewModelTracks.DeleteTrack(id, User.Identity.GetUserId());
                return Json(new { success = true }, JsonRequestBehavior.AllowGet);
            }
            return RedirectToAction("Index");
        }
        [Authorize]
        public ActionResult GetTrackCoordinatesById(int id)
        {
            if (User.Identity.IsAuthenticated)
            {
                return Json(HomeViewModelTracks.GetTrackCoordinatesById(id, User.Identity.GetUserId()), JsonRequestBehavior.AllowGet);
            }
            return RedirectToAction("Index");
        }
        [Authorize]
        [HttpPost]
        public JsonResult AddTrack(string name, string datetime, string type, string coordinates, string distance)
        {
            LogSave.Save(User.Identity.IsAuthenticated ? User.Identity.GetUserId() : "", Request.UserHostAddress, Request.UserAgent, "Home/AddTrack", name);
            var errorMessage = string.Empty;
            var ac = new ApplicationDbContext();
            var id = -1;

            if (string.IsNullOrEmpty(errorMessage) && string.IsNullOrEmpty(name)) errorMessage = "Короткое название";
            if (string.IsNullOrEmpty(errorMessage) && string.IsNullOrEmpty(datetime))errorMessage = "Ошибка в дате";
            if (string.IsNullOrEmpty(errorMessage))
            {
                try
                {
                    id = Convert.ToInt32(type);
                    if (id < 0) throw new Exception();
                }
                catch
                {
                    errorMessage = "Корявый тип";
                }
            }
            if (string.IsNullOrEmpty(errorMessage) && coordinates.Length == 0)errorMessage = "Нет координат";
            if (string.IsNullOrEmpty(errorMessage) && (distance.Length == 0 || distance.ToLower() == "nan".ToLower() || distance == "0"))errorMessage = "Ошибка вычисления расстояния, перепроверьте координаты";

            double toDouble = -1;
            if (string.IsNullOrEmpty(errorMessage))
            {
                var sDistance = distance;
                if (distance.Contains('.')) sDistance = sDistance.Remove(distance.IndexOf('.'));
                try
                {
                    toDouble = Convert.ToDouble(sDistance);
                    if (toDouble < 0) throw new Exception();
                }
                catch (Exception e)
                {
                    errorMessage = "Нереальное расстояние =" + distance + " s_=" + sDistance + " mes=" + e.Message;
                }
            }
            if (string.IsNullOrEmpty(errorMessage) && (!distance.Split(';').Any() || distance.Split(';').Length > 100))errorMessage = "Ошибка колличества координат =" + distance.Split(';').Length;
            if (string.IsNullOrEmpty(errorMessage))
            {
                try
                {
                    ac.Tracks.Add(new Tracks { UserId = User.Identity.GetUserId(), Type = id, Coordinates = coordinates, Date = UmtTime.To(Convert.ToDateTime(datetime)), Name = name, Distance = toDouble });
                    ac.SaveChanges();
                }
                catch (Exception e)
                {
                    errorMessage = Resource.ErrorWorkWithDB + "=" + e.Message;
                }
            }
            return !string.IsNullOrEmpty(errorMessage) ? new JsonResult { Data = new { success = false, error = errorMessage } } : Json(new { success = true });
        }
        [Authorize]
        [HttpPost]
        public JsonResult EditTrack(int id, string name, string datetime, string type, string coordinates, string distance)
        {
            LogSave.Save(User.Identity.IsAuthenticated ? User.Identity.GetUserId() : "", Request.UserHostAddress, Request.UserAgent, "Home/EditTrack", name);
            var errorMessage = string.Empty;
            var ac = new ApplicationDbContext();
            var recordId = -1;
            double toDouble = -1;

            if (string.IsNullOrEmpty(errorMessage) && string.IsNullOrEmpty(name))errorMessage = "Короткое название";
            if (string.IsNullOrEmpty(errorMessage) && string.IsNullOrEmpty(datetime))errorMessage = "Ошибка в дате";
            if (string.IsNullOrEmpty(errorMessage))
            {
                try
                {
                    recordId = Convert.ToInt32(type);
                    if (recordId < 0) throw new Exception();
                }
                catch
                {
                    errorMessage = "Корявый тип";
                }
            }
            if (string.IsNullOrEmpty(errorMessage) && coordinates.Length == 0)errorMessage = "Нет координат";
            if (string.IsNullOrEmpty(errorMessage) && (distance.Length == 0 || distance.ToLower() == "nan".ToLower() || distance == "0"))errorMessage = "Ошибка вычисления расстояния, перепроверьте координаты";
            if (string.IsNullOrEmpty(errorMessage))
            {
                var sDistance = distance;
                if (distance.Contains('.')) sDistance = sDistance.Remove(distance.IndexOf('.'));
                try
                {
                    toDouble = Convert.ToDouble(sDistance);
                    if (toDouble < 0) throw new Exception();
                }
                catch (Exception e)
                {
                    errorMessage = "Нереальное расстояние =" + distance + " s_=" + sDistance + " mes=" + e.Message;
                }
            }
            if (string.IsNullOrEmpty(errorMessage) && (!distance.Split(';').Any() || distance.Split(';').Length > 100))errorMessage = "Ошибка колличества координат =" + distance.Split(';').Length;
            if (string.IsNullOrEmpty(errorMessage))
            {
                try
                {
                    var user = User.Identity.GetUserId();
                    var track = ac.Tracks.First(t => t.Id == id && t.UserId == user);
                    track.Name = name;
                    track.Type = recordId;
                    track.Coordinates = coordinates;
                    track.Date = UmtTime.To(Convert.ToDateTime(datetime));
                    track.Distance = toDouble;
                    ac.SaveChanges();
                }
                catch (Exception e)
                {
                    errorMessage = Resource.ErrorWorkWithDB + "=" + e.Message;
                }
            }
            return !string.IsNullOrEmpty(errorMessage) ? new JsonResult { Data = new { success = false, error = errorMessage } } : Json(new { success = true });
        }
        [BrowserActionFilter]
        public ActionResult Home()
        {
            WriteCookie(CookieKeys.HomeCategory, ReadCookie(CookieKeys.HomeCategoryPrev, Defaults.CategoryBase.FilmIndex));
            return RedirectToAction("Index");
        }
        [BrowserActionFilter]
        [Authorize]
        public ActionResult Logs()
        {
            if (!HomeViewModel.IsCategoryExt(ReadCookie(CookieKeys.HomeCategory, Defaults.CategoryBase.FilmIndex)))
                WriteCookie(CookieKeys.HomeCategoryPrev, ReadCookie(CookieKeys.HomeCategory, Defaults.CategoryBase.FilmIndex));
            WriteCookie(CookieKeys.HomeCategory, (int)HomeViewModel.CategoryExt.Logs);
            return RedirectToAction("Index");
        }
        [BrowserActionFilter]
        [Authorize]
        public ActionResult Users()
        {
            if (!HomeViewModel.IsCategoryExt(ReadCookie(CookieKeys.HomeCategory, Defaults.CategoryBase.FilmIndex)))
                WriteCookie(CookieKeys.HomeCategoryPrev, ReadCookie(CookieKeys.HomeCategory, Defaults.CategoryBase.FilmIndex));

            WriteCookie(CookieKeys.HomeCategory, (int)HomeViewModel.CategoryExt.Users);
            return RedirectToAction("Index");
        }
        [BrowserActionFilter]
        [Authorize]
        public ActionResult Improvements()
        {
            if (!HomeViewModel.IsCategoryExt(ReadCookie(CookieKeys.HomeCategory, Defaults.CategoryBase.FilmIndex)))
                WriteCookie(CookieKeys.HomeCategoryPrev, ReadCookie(CookieKeys.HomeCategory, Defaults.CategoryBase.FilmIndex));
            WriteCookie(CookieKeys.HomeCategory, (int)HomeViewModel.CategoryExt.Improvements);
            return RedirectToAction("Index");
        }
        [BrowserActionFilter]
        [Authorize]
        public ActionResult TrackEditor()
        {
            LogSave.Save(User.Identity.IsAuthenticated ? User.Identity.GetUserId() : "", Request.UserHostAddress, Request.UserAgent, "Home/TrackEditor");
            return View(new HomeViewModelTrackEditor(User.Identity.GetUserId()));
        }
        
    }
}