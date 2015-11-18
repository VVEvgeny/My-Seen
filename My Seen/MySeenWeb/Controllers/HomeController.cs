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

            if (User.Identity.IsAuthenticated)
            {
                HomeViewModel af = new HomeViewModel(
                    ReadCookie(CookieKeys.HomeCategory, Defaults.CategoryBase.FilmIndex).ToString(),
                    User.Identity.GetUserId(),
                    page == null ? 1 : page.Value,
                    Rpp,
                    ReadCookie(CookieKeys.ImprovementsCategory, Defaults.ComplexBase.IndexAll),
                    search
                    );
                return View(af);
            }
            return View();
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
        public JsonResult AddFilm(string name, string genre, string rating)
        {
            LogSave.Save(User.Identity.IsAuthenticated ? User.Identity.GetUserId() : "", Request.UserHostAddress, Request.UserAgent, "Home/AddFilm", name);
            string errorMessage = string.Empty;
            string userId = User.Identity.GetUserId();
            if (string.IsNullOrEmpty(errorMessage))
            {
                if (string.IsNullOrEmpty(name)) errorMessage = Resource.EnterFilmName;
            }
            ApplicationDbContext ac = new ApplicationDbContext();
            if (string.IsNullOrEmpty(errorMessage))
            {
                if (ac.Films.Count(f => f.Name == name && f.UserId == userId) != 0)//айди проверяем только для редактируемых, чтобы не налететь по названию на чужой
                {
                    errorMessage = Resource.FilmNameAlreadyExists;
                }
            }
            if (string.IsNullOrEmpty(errorMessage))
            {
                try
                {
                    Films f = new Films { Name = name, Genre = Convert.ToInt32(genre), Rating = Convert.ToInt32(rating), DateSee = UMTTime.To(DateTime.Now), DateChange = UMTTime.To(DateTime.Now), UserId = userId };
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
        public JsonResult EditFilm(string id, string name, string genre, string rating)
        {
            LogSave.Save(User.Identity.IsAuthenticated ? User.Identity.GetUserId() : "", Request.UserHostAddress, Request.UserAgent, "Home/EditFilm", id);
            string errorMessage = string.Empty;
            string userId = User.Identity.GetUserId();
            if (string.IsNullOrEmpty(errorMessage))
            {
                if (string.IsNullOrEmpty(name)) errorMessage = Resource.EnterFilmName;
            }
            ApplicationDbContext ac = new ApplicationDbContext();
            int iid = (Convert.ToInt32(id));
            if (ac.Films.Count(f => f.Name == name && f.UserId == userId && f.Id != iid) != 0)//айди проверяем только для редактируемых, чтобы не налететь по названию на чужой
            {
                errorMessage = Resource.FilmNameAlreadyExists;
            }
            if (string.IsNullOrEmpty(errorMessage))
            {
                try
                {
                    Films film = ac.Films.First(f => f.UserId == userId && f.Id == iid);
                    film.Name = name;
                    film.Genre = Convert.ToInt32(genre);
                    film.Rating = Convert.ToInt32(rating);
                    film.DateChange = UMTTime.To(DateTime.Now);
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
        public JsonResult AddSerial(string name, string season, string series, string genre, string rating)
        {
            LogSave.Save(User.Identity.IsAuthenticated ? User.Identity.GetUserId() : "", Request.UserHostAddress, Request.UserAgent, "Home/AddSerial", name);
            string errorMessage = string.Empty;
            string userId = User.Identity.GetUserId();
            if (string.IsNullOrEmpty(errorMessage))
            {
                if (string.IsNullOrEmpty(name)) errorMessage = Resource.EnterSerialName;
            }
            ApplicationDbContext ac = new ApplicationDbContext();
            if (string.IsNullOrEmpty(errorMessage))
            {
                if (ac.Serials.Count(f => f.Name == name && f.UserId == userId) != 0)//айди проверяем только для редактируемых, чтобы не налететь по названию на чужой
                {
                    errorMessage = Resource.SerialNameAlreadyExists;
                }
            }
            if (string.IsNullOrEmpty(errorMessage))
            {
                try
                {
                    if (string.IsNullOrEmpty(season)) season = "1";
                    if (string.IsNullOrEmpty(series)) series = "1";
                    Serials s = new Serials { Name = name, LastSeason = Convert.ToInt32(season), LastSeries = Convert.ToInt32(series), Genre = Convert.ToInt32(genre), Rating = Convert.ToInt32(rating), DateBegin = UMTTime.To(DateTime.Now), DateLast = UMTTime.To(DateTime.Now), DateChange = UMTTime.To(DateTime.Now), UserId = userId };
                    ac.Serials.Add(s);
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
        public JsonResult EditSerial(string id, string name, string season, string series, string genre, string rating)
        {
            LogSave.Save(User.Identity.IsAuthenticated ? User.Identity.GetUserId() : "", Request.UserHostAddress, Request.UserAgent, "Home/EditSerial", id);
            string errorMessage = string.Empty;
            string userId = User.Identity.GetUserId();
            if (string.IsNullOrEmpty(errorMessage))
            {
                if (string.IsNullOrEmpty(name)) errorMessage = Resource.EnterSerialName;
            }
            ApplicationDbContext ac = new ApplicationDbContext();
            int iid = (Convert.ToInt32(id));
            if (ac.Serials.Count(f => f.Name == name && f.UserId == userId && f.Id != iid) != 0)//айди проверяем только для редактируемых, чтобы не налететь по названию на чужой
            {
                errorMessage = Resource.FilmNameAlreadyExists;
            }
            if (string.IsNullOrEmpty(errorMessage))
            {
                try
                {
                    Serials film = ac.Serials.First(f => f.UserId == userId && f.Id == iid);
                    film.Name = name;
                    if (film.LastSeason != Convert.ToInt32(season) || film.LastSeries != Convert.ToInt32(series))
                    {
                        film.DateLast = UMTTime.To(DateTime.Now);
                    }
                    film.LastSeason = Convert.ToInt32(season);
                    film.LastSeries = Convert.ToInt32(series);
                    film.Genre = Convert.ToInt32(genre);
                    film.Rating = Convert.ToInt32(rating);
                    film.DateChange = UMTTime.To(DateTime.Now);
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
        public JsonResult AddBook(string name, string authors, string genre, string rating)
        {
            LogSave.Save(User.Identity.IsAuthenticated ? User.Identity.GetUserId() : "", Request.UserHostAddress, Request.UserAgent, "Home/AddBook", name);
            string errorMessage = string.Empty;
            string userId = User.Identity.GetUserId();
            if (string.IsNullOrEmpty(errorMessage))
            {
                if (string.IsNullOrEmpty(authors)) errorMessage = Resource.EnterBookName;
            }
            if (string.IsNullOrEmpty(errorMessage))
            {
                if (string.IsNullOrEmpty(authors)) errorMessage = Resource.EnterBookAuthors;
            }
            ApplicationDbContext ac = new ApplicationDbContext();
            if (string.IsNullOrEmpty(errorMessage))
            {
                if (ac.Books.Count(f => f.Name == name && f.UserId == userId) != 0)//айди проверяем только для редактируемых, чтобы не налететь по названию на чужой
                {
                    errorMessage = Resource.BookNameAlreadyExists;
                }
            }
            if (string.IsNullOrEmpty(errorMessage))
            {
                try
                {
                    Books f = new Books { Name = name, Authors = authors, Genre = Convert.ToInt32(genre), Rating = Convert.ToInt32(rating), DateRead = UMTTime.To(DateTime.Now), DateChange = UMTTime.To(DateTime.Now), UserId = userId };
                    ac.Books.Add(f);
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
        public JsonResult EditBook(string id, string name, string authors, string genre, string rating)
        {
            LogSave.Save(User.Identity.IsAuthenticated ? User.Identity.GetUserId() : "", Request.UserHostAddress, Request.UserAgent, "Home/EditBook", id);
            string errorMessage = string.Empty;
            string userId = User.Identity.GetUserId();
            if (string.IsNullOrEmpty(errorMessage))
            {
                if (string.IsNullOrEmpty(name)) errorMessage = Resource.EnterBookName;
            }
            ApplicationDbContext ac = new ApplicationDbContext();
            int iid = (Convert.ToInt32(id));
            if (ac.Books.Count(f => f.Name == name && f.UserId == userId && f.Id != iid) != 0)//айди проверяем только для редактируемых, чтобы не налететь по названию на чужой
            {
                errorMessage = Resource.BookNameAlreadyExists;
            }
            if (string.IsNullOrEmpty(errorMessage))
            {
                try
                {
                    Books film = ac.Books.First(f => f.UserId == userId && f.Id == iid);
                    film.Name = name;
                    film.Authors = authors;
                    film.Genre = Convert.ToInt32(genre);
                    film.Rating = Convert.ToInt32(rating);
                    film.DateChange = UMTTime.To(DateTime.Now);
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
        public JsonResult DeleteFilm(string id)
        {
            LogSave.Save(User.Identity.IsAuthenticated ? User.Identity.GetUserId() : "", Request.UserHostAddress, Request.UserAgent, "Home/DeleteFilm", id);
            string errorMessage = string.Empty;
            string userId = User.Identity.GetUserId();
            ApplicationDbContext ac = new ApplicationDbContext();
            int iid = (Convert.ToInt32(id));

            if (string.IsNullOrEmpty(errorMessage))
            {
                try
                {
                    Films film = ac.Films.First(f => f.UserId == userId && f.Id == iid);
                    film.isDeleted = true;
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
        public JsonResult DeleteSerial(string id)
        {
            LogSave.Save(User.Identity.IsAuthenticated ? User.Identity.GetUserId() : "", Request.UserHostAddress, Request.UserAgent, "Home/DeleteSerial", id);
            string errorMessage = string.Empty;
            string userId = User.Identity.GetUserId();
            ApplicationDbContext ac = new ApplicationDbContext();
            int iid = (Convert.ToInt32(id));

            if (string.IsNullOrEmpty(errorMessage))
            {
                try
                {
                    Serials film = ac.Serials.First(f => f.UserId == userId && f.Id == iid);
                    film.isDeleted = true;
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
        public JsonResult DeleteBook(string id)
        {
            LogSave.Save(User.Identity.IsAuthenticated ? User.Identity.GetUserId() : "", Request.UserHostAddress, Request.UserAgent, "Home/DeleteBook", id);
            string errorMessage = string.Empty;
            string userId = User.Identity.GetUserId();
            ApplicationDbContext ac = new ApplicationDbContext();
            int iid = (Convert.ToInt32(id));

            if (string.IsNullOrEmpty(errorMessage))
            {
                try
                {
                    Books film = ac.Books.First(f => f.UserId == userId && f.Id == iid);
                    film.isDeleted = true;
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
        public JsonResult AddImprovement(string desc, string complex)
        {
            LogSave.Save(User.Identity.IsAuthenticated ? User.Identity.GetUserId() : "", Request.UserHostAddress, Request.UserAgent, "Home/AddImprovement", desc);
            string errorMessage = string.Empty;
            ApplicationDbContext ac = new ApplicationDbContext();
            if (string.IsNullOrEmpty(errorMessage))
            {
                if (desc.Length == 0)
                {
                    errorMessage = Resource.DescToShort;
                }
            }
            int id = -1;
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
            if (string.IsNullOrEmpty(errorMessage))
            {
                if (ac.Bugs.Count(f => f.Text == desc && f.Complex == id) != 0)
                {
                    errorMessage = Resource.BugAlreadyExists;
                }
            }
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
            if (!string.IsNullOrEmpty(errorMessage))
            {
                return new JsonResult { Data = new { success = false, error = errorMessage } };
            }
            return Json(new { success = true });
        }
        [Authorize]
        [HttpPost]
        public JsonResult EndImprovement(string id, string desc, string version)
        {
            LogSave.Save(User.Identity.IsAuthenticated ? User.Identity.GetUserId() : "", Request.UserHostAddress, Request.UserAgent, "Home/EndImprovement", id + " " + desc + " " + version);
            string errorMessage = string.Empty;
            ApplicationDbContext ac = new ApplicationDbContext();
            var _id = -1;
            int _version = -1;
            if (string.IsNullOrEmpty(errorMessage))
            {
                if (desc.Length == 0)
                {
                    errorMessage = Resource.DescToShort;
                }
            }
            if (string.IsNullOrEmpty(errorMessage))
            {
                try
                {
                    _id = Convert.ToInt32(id);
                    if (_id < 0) throw new Exception();
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
                    _version = Convert.ToInt32(version);
                    if (_version < 0) throw new Exception();
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
                    var bug = ac.Bugs.First(b => b.Id == _id);
                    bug.TextEnd = desc;
                    bug.DateEnd = DateTime.Now;
                    bug.Version = _version;
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
        public JsonResult DeleteImprovement(string id)
        {
            LogSave.Save(User.Identity.IsAuthenticated ? User.Identity.GetUserId() : "", Request.UserHostAddress, Request.UserAgent, "Home/DeleteImprovement", id);
            string errorMessage = string.Empty;
            ApplicationDbContext ac = new ApplicationDbContext();
            int _id = 0;
            if (string.IsNullOrEmpty(errorMessage))
            {
                _id = Convert.ToInt32(id);
                try
                {
                    if (_id < 0) throw new Exception();
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
                    ac.Bugs.RemoveRange(ac.Bugs.Where(b => b.Id == _id));
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
                //TracksViewModel model = new TracksViewModel();
                return Json(HomeViewModelTracks.GetTrack(id, User.Identity.GetUserId()), JsonRequestBehavior.AllowGet);
            }
            return RedirectToAction("Index");
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
        public JsonResult AddTrack(string name, string type, string coordinates, string distance)
        {
            LogSave.Save(User.Identity.IsAuthenticated ? User.Identity.GetUserId() : "", Request.UserHostAddress, Request.UserAgent, "Home/AddTrack", name);
            string errorMessage = string.Empty;
            ApplicationDbContext ac = new ApplicationDbContext();

            //errorMessage = "name=" + name + " type=" + type + " coordinates=" + coordinates + " distance=" + distance;

            if (string.IsNullOrEmpty(errorMessage))
            {
                if (name.Length == 0)
                {
                    errorMessage = "Короткое название";
                }
            }
            int id = -1;
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
            if (string.IsNullOrEmpty(errorMessage))
            {
                if (coordinates.Length == 0)
                {
                    errorMessage = "Нет координат";
                }
            }
            if (string.IsNullOrEmpty(errorMessage))
            {
                if (distance.Length == 0 || distance.ToLower() == "nan".ToLower() || distance == "0")
                {
                    errorMessage = "Ошибка вычисления расстояния, перепроверьте координаты";
                }
            }
            double _distance = -1;
            if (string.IsNullOrEmpty(errorMessage))
            {
                string sDistance = distance;
                if (distance.Contains('.'))
                {
                    //if (distance.Length > (distance.IndexOf('.') + 1 + 4))
                    {
                        sDistance = sDistance.Remove(distance.IndexOf('.'));
                    }
                }
                try
                {
                    _distance = Convert.ToDouble(sDistance);
                    if (_distance < 0) throw new Exception();
                }
                catch (Exception e)
                {
                    errorMessage = "Нереальное расстояние =" + distance + " s_=" + sDistance + " mes=" + e.Message;
                }
            }
            if (string.IsNullOrEmpty(errorMessage))
            {
                if (!distance.Split(';').Any() || distance.Split(';').Count() > 100)
                {
                    errorMessage = "Ошибка колличества координат =" + distance.Split(';').Count().ToString();
                }
            }
            if (string.IsNullOrEmpty(errorMessage))
            {
                try
                {
                    ac.Tracks.Add(new Tracks { UserId = User.Identity.GetUserId(), Type = id, Coordinates = coordinates, Date = DateTime.Now, Name = name, Distance = _distance });
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
        public JsonResult EditTrack(int id, string name, string type, string coordinates, string distance)
        {
            LogSave.Save(User.Identity.IsAuthenticated ? User.Identity.GetUserId() : "", Request.UserHostAddress, Request.UserAgent, "Home/EditTrack", name);
            string errorMessage = string.Empty;
            ApplicationDbContext ac = new ApplicationDbContext();

            //errorMessage = "id="+id.ToString()+" name=" + name + " type=" + type + " coordinates=" + coordinates + " distance=" + distance;

            if (string.IsNullOrEmpty(errorMessage))
            {
                if (name.Length == 0)
                {
                    errorMessage = "Короткое название";
                }
            }
            int _id = -1;
            if (string.IsNullOrEmpty(errorMessage))
            {
                try
                {
                    _id = Convert.ToInt32(type);
                    if (_id < 0) throw new Exception();
                }
                catch
                {
                    errorMessage = "Корявый тип";
                }
            }
            if (string.IsNullOrEmpty(errorMessage))
            {
                if (coordinates.Length == 0)
                {
                    errorMessage = "Нет координат";
                }
            }
            if (string.IsNullOrEmpty(errorMessage))
            {
                if (distance.Length == 0 || distance.ToLower() == "nan".ToLower() || distance == "0")
                {
                    errorMessage = "Ошибка вычисления расстояния, перепроверьте координаты";
                }
            }
            double _distance = -1;
            if (string.IsNullOrEmpty(errorMessage))
            {
                string sDistance = distance;
                if (distance.Contains('.'))
                {
                    //if (distance.Length > (distance.IndexOf('.') + 1 + 4))
                    {
                        sDistance = sDistance.Remove(distance.IndexOf('.'));
                    }
                }
                try
                {
                    _distance = Convert.ToDouble(sDistance);
                    if (_distance < 0) throw new Exception();
                }
                catch (Exception e)
                {
                    errorMessage = "Нереальное расстояние =" + distance + " s_=" + sDistance + " mes=" + e.Message;
                }
            }
            if (string.IsNullOrEmpty(errorMessage))
            {
                if (!distance.Split(';').Any() || distance.Split(';').Count() > 100)
                {
                    errorMessage = "Ошибка колличества координат =" + distance.Split(';').Count().ToString();
                }
            }
            if (string.IsNullOrEmpty(errorMessage))
            {
                try
                {
                    string user = User.Identity.GetUserId();
                    Tracks track = ac.Tracks.First(t => t.Id == id && t.UserId == user);
                    track.Name = name;
                    track.Type = _id;
                    track.Coordinates = coordinates;
                    track.Distance = _distance;
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
    }
}