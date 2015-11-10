using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MySeenWeb.Models;
using Microsoft.AspNet.Identity;
using MySeenLib;

namespace MySeenWeb.Controllers
{
    //[RequireHttps]
    public class HomeController : BaseController
    {
        public ActionResult Index(int? page)
        {
            LogSave.Save(User.Identity.IsAuthenticated?User.Identity.GetUserId():"", Request.UserHostAddress, Request.UserAgent, "Home/Index");
            if (User.Identity.IsAuthenticated)
            {
                HomeViewModel af = new HomeViewModel();
                HttpCookie cookie = ControllerContext.HttpContext.Request.Cookies[HomeViewModel.AFCookies.CoockieSelectedKey];
                if (cookie == null)
                {
                    af.Selected = Defaults.Categories.GetById(Defaults.CategoryBase.FilmIndex);
                    cookie = new HttpCookie(HomeViewModel.AFCookies.CoockieSelectedKey);
                    cookie.Value = Defaults.CategoryBase.FilmIndex.ToString();
                    cookie.Expires = DateTime.Now.AddDays(1);
                    ControllerContext.HttpContext.Response.Cookies.Add(cookie);
                }
                else
                {
                    if (cookie.Value == Defaults.CategoryBase.FilmIndex.ToString())af.Selected = Defaults.Categories.GetById(Defaults.CategoryBase.FilmIndex);
                    else if (cookie.Value == Defaults.CategoryBase.SerialIndex.ToString())af.Selected = Defaults.Categories.GetById(Defaults.CategoryBase.SerialIndex);
                    else af.Selected = Defaults.Categories.GetById(Defaults.CategoryBase.BookIndex);
                }
                af.LoadSelectList();
                if (af.Selected == Defaults.Categories.GetById(Defaults.CategoryBase.FilmIndex)) af.LoadFilms(User.Identity.GetUserId(), page == null ? 1 : page.Value, RPP);
                else if (af.Selected == Defaults.Categories.GetById(Defaults.CategoryBase.SerialIndex)) af.LoadSerials(User.Identity.GetUserId(), page == null ? 1 : page.Value, RPP);
                else af.LoadBooks(User.Identity.GetUserId(), page == null ? 1 : page.Value, RPP);

                return View(af);
            }
            return View();
        }
        [HttpPost]
        public JsonResult ChangeCookies(string selected)
        {
            HttpCookie cc = ControllerContext.HttpContext.Request.Cookies[HomeViewModel.AFCookies.CoockieSelectedKey];
            if (cc == null)
            {
                cc = new HttpCookie(HomeViewModel.AFCookies.CoockieSelectedKey);
            }
            cc.Value = Defaults.Categories.GetId(selected).ToString();
            cc.Expires = DateTime.Now.AddDays(1);
            ControllerContext.HttpContext.Response.Cookies.Add(cc);
            return Json(new { success = true });
        }
        [HttpPost]
        public JsonResult ChangeCookiesBugs(string selected)
        {
            HttpCookie cc = ControllerContext.HttpContext.Request.Cookies[BugsViewModel.AFCookies.CoockieSelectedKey];
            if (cc == null)
            {
                cc = new HttpCookie(BugsViewModel.AFCookies.CoockieSelectedKey);
            }
            cc.Value = selected;
            cc.Expires = DateTime.Now.AddDays(1);
            ControllerContext.HttpContext.Response.Cookies.Add(cc);
            return Json(new { success = true });
        }
        [Authorize]
        [HttpPost]
        public JsonResult AddFilm(string name, string genre, string rating)
        {
            LogSave.Save(User.Identity.IsAuthenticated ? User.Identity.GetUserId() : "", Request.UserHostAddress, Request.UserAgent, "Home/AddFilm",name);
            string errorMessage = string.Empty;
            string user_id = User.Identity.GetUserId();
            if (string.IsNullOrEmpty(errorMessage))
            {
                if (string.IsNullOrEmpty(name)) errorMessage = Resource.EnterFilmName;
            }
            ApplicationDbContext ac = new ApplicationDbContext();
            if (string.IsNullOrEmpty(errorMessage))
            {
                if (ac.Films.Count(f => f.Name == name && f.UserId == user_id) != 0)//айди проверяем только для редактируемых, чтобы не налететь по названию на чужой
                {
                    errorMessage = Resource.FilmNameAlreadyExists;
                }
            }
            if (string.IsNullOrEmpty(errorMessage))
            {
                try
                {
                    Films f = new Films { Name = name, Genre = Convert.ToInt32(genre), Rating = Convert.ToInt32(rating), DateSee = UMTTime.To(DateTime.Now), DateChange = UMTTime.To(DateTime.Now), UserId = user_id };
                    ac.Films.Add(f);
                    ac.SaveChanges();
                }
                catch (Exception e)
                {
                    errorMessage = Resource.ErrorWorkWithDB +"="+ e.Message;
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
        public JsonResult EditFilm(string id,string name, string genre, string rating)
        {
            LogSave.Save(User.Identity.IsAuthenticated ? User.Identity.GetUserId() : "", Request.UserHostAddress, Request.UserAgent, "Home/EditFilm",id);
            string errorMessage = string.Empty;
            string user_id = User.Identity.GetUserId();
            if (string.IsNullOrEmpty(errorMessage))
            {
                if (string.IsNullOrEmpty(name)) errorMessage = Resource.EnterFilmName;
            }
            ApplicationDbContext ac = new ApplicationDbContext();
            int iid = (Convert.ToInt32(id));
            if (ac.Films.Count(f => f.Name == name && f.UserId == user_id && f.Id != iid) != 0)//айди проверяем только для редактируемых, чтобы не налететь по названию на чужой
            {
                errorMessage = Resource.FilmNameAlreadyExists;
            }
            if (string.IsNullOrEmpty(errorMessage))
            {
                try
                {
                    Films film = ac.Films.Where(f => f.UserId == user_id && f.Id == iid).First();
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
            LogSave.Save(User.Identity.IsAuthenticated ? User.Identity.GetUserId() : "", Request.UserHostAddress, Request.UserAgent, "Home/AddSerial",name);
            string errorMessage = string.Empty;
            string user_id = User.Identity.GetUserId();
            if (string.IsNullOrEmpty(errorMessage))
            {
                if (string.IsNullOrEmpty(name)) errorMessage = Resource.EnterSerialName;
            }
            ApplicationDbContext ac = new ApplicationDbContext();
            if (string.IsNullOrEmpty(errorMessage))
            {
                if (ac.Serials.Count(f => f.Name == name && f.UserId == user_id) != 0)//айди проверяем только для редактируемых, чтобы не налететь по названию на чужой
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
                    Serials s = new Serials { Name = name, LastSeason = Convert.ToInt32(season), LastSeries = Convert.ToInt32(series), Genre = Convert.ToInt32(genre), Rating = Convert.ToInt32(rating), DateBegin = UMTTime.To(DateTime.Now), DateLast = UMTTime.To(DateTime.Now), DateChange = UMTTime.To(DateTime.Now), UserId = user_id };
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
            LogSave.Save(User.Identity.IsAuthenticated ? User.Identity.GetUserId() : "", Request.UserHostAddress, Request.UserAgent, "Home/EditSerial",id);
            string errorMessage = string.Empty;
            string user_id = User.Identity.GetUserId();
            if (string.IsNullOrEmpty(errorMessage))
            {
                if (string.IsNullOrEmpty(name)) errorMessage = Resource.EnterSerialName;
            }
            ApplicationDbContext ac = new ApplicationDbContext();
            int iid = (Convert.ToInt32(id));
            if (ac.Serials.Count(f => f.Name == name && f.UserId == user_id && f.Id != iid) != 0)//айди проверяем только для редактируемых, чтобы не налететь по названию на чужой
            {
                errorMessage = Resource.FilmNameAlreadyExists;
            }
            if (string.IsNullOrEmpty(errorMessage))
            {
                try
                {
                    Serials film = ac.Serials.Where(f => f.UserId == user_id && f.Id == iid).First();
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
            string user_id = User.Identity.GetUserId();
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
                if (ac.Books.Count(f => f.Name == name && f.UserId == user_id) != 0)//айди проверяем только для редактируемых, чтобы не налететь по названию на чужой
                {
                    errorMessage = Resource.BookNameAlreadyExists;
                }
            }
            if (string.IsNullOrEmpty(errorMessage))
            {
                try
                {
                    Books f = new Books { Name = name, Authors = authors, Genre = Convert.ToInt32(genre), Rating = Convert.ToInt32(rating), DateRead = UMTTime.To(DateTime.Now), DateChange = UMTTime.To(DateTime.Now), UserId = user_id };
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
            string user_id = User.Identity.GetUserId();
            if (string.IsNullOrEmpty(errorMessage))
            {
                if (string.IsNullOrEmpty(name)) errorMessage = Resource.EnterBookName;
            }
            ApplicationDbContext ac = new ApplicationDbContext();
            int iid = (Convert.ToInt32(id));
            if (ac.Books.Count(f => f.Name == name && f.UserId == user_id && f.Id != iid) != 0)//айди проверяем только для редактируемых, чтобы не налететь по названию на чужой
            {
                errorMessage = Resource.BookNameAlreadyExists;
            }
            if (string.IsNullOrEmpty(errorMessage))
            {
                try
                {
                    Books film = ac.Books.Where(f => f.UserId == user_id && f.Id == iid).First();
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
            LogSave.Save(User.Identity.IsAuthenticated ? User.Identity.GetUserId() : "", Request.UserHostAddress, Request.UserAgent, "Home/DeleteFilm",id);
            string errorMessage = string.Empty;
            string user_id = User.Identity.GetUserId();
            ApplicationDbContext ac = new ApplicationDbContext();
            int iid = (Convert.ToInt32(id));

            if (string.IsNullOrEmpty(errorMessage))
            {
                try
                {
                    Films film = ac.Films.Where(f => f.UserId == user_id && f.Id == iid).First();
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
            LogSave.Save(User.Identity.IsAuthenticated ? User.Identity.GetUserId() : "", Request.UserHostAddress, Request.UserAgent, "Home/DeleteSerial",id);
            string errorMessage = string.Empty;
            string user_id = User.Identity.GetUserId();
            ApplicationDbContext ac = new ApplicationDbContext();
            int iid = (Convert.ToInt32(id));

            if (string.IsNullOrEmpty(errorMessage))
            {
                try
                {
                    Serials film = ac.Serials.Where(f => f.UserId == user_id && f.Id == iid).First();
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
            string user_id = User.Identity.GetUserId();
            ApplicationDbContext ac = new ApplicationDbContext();
            int iid = (Convert.ToInt32(id));

            if (string.IsNullOrEmpty(errorMessage))
            {
                try
                {
                    Books film = ac.Books.Where(f => f.UserId == user_id && f.Id == iid).First();
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
        public ActionResult Users(int? page)
        {
            LogSave.Save(User.Identity.IsAuthenticated ? User.Identity.GetUserId() : "", Request.UserHostAddress, Request.UserAgent, "Home/Users");
            if (User.Identity.IsAuthenticated && Admin.isAdmin(User.Identity.Name))
            {
                UsersViewModel model = new UsersViewModel();
                model.Load(page == null ? 1 : page.Value, RPP);
                return View(model);
            }
            return RedirectToAction("Index");
        }
        [Authorize]
        public ActionResult Bugs(int? page)
        {
            LogSave.Save(User.Identity.IsAuthenticated ? User.Identity.GetUserId() : "", Request.UserHostAddress, Request.UserAgent, "Home/Bugs");
            BugsViewModel model = new BugsViewModel();
            HttpCookie cookie = ControllerContext.HttpContext.Request.Cookies[BugsViewModel.AFCookies.CoockieSelectedKey];
            int complex_cookie = Defaults.ComplexBase.IndexAll;
            if (cookie == null)
            {
                cookie = new HttpCookie(BugsViewModel.AFCookies.CoockieSelectedKey);
                cookie.Value = complex_cookie.ToString();
                cookie.Expires = DateTime.Now.AddDays(1);
                ControllerContext.HttpContext.Response.Cookies.Add(cookie);
            }
            else
            {
                try
                {
                    complex_cookie = Convert.ToInt32(cookie.Value);
                    if (complex_cookie < 0) throw new Exception();
                }
                catch
                {
                    complex_cookie = Defaults.ComplexBase.IndexAll;
                    ControllerContext.HttpContext.Response.Cookies.Remove(BugsViewModel.AFCookies.CoockieSelectedKey);
                    cookie.Value = complex_cookie.ToString();
                    cookie.Expires = DateTime.Now.AddDays(1);
                    ControllerContext.HttpContext.Response.Cookies.Add(cookie);
                }
            }
            model.Load(complex_cookie, page == null ? 1 : page.Value, RPP);
            return View(model);
        }
        [Authorize]
        [HttpPost]
        public JsonResult AddBug(string desc, string complex)
        {
            LogSave.Save(User.Identity.IsAuthenticated ? User.Identity.GetUserId() : "", Request.UserHostAddress, Request.UserAgent, "Home/AddBug", desc);
            string errorMessage = string.Empty;
            string user_id = User.Identity.GetUserId();
            ApplicationDbContext ac = new ApplicationDbContext();
            if (string.IsNullOrEmpty(errorMessage))
            {
                if (desc.Length==0)
                {
                    errorMessage = Resource.DescToShort;
                }
            }
            int _id=-1;
            if (string.IsNullOrEmpty(errorMessage))
            {
                try
                {
                    _id = Convert.ToInt32(complex);
                    if (_id < 0) throw new Exception();
                }
                catch
                {
                    errorMessage = "Корявая айдишка";
                }
            }
            if (string.IsNullOrEmpty(errorMessage))
            {
                if (ac.Bugs.Count(f => f.Text == desc && f.Complex == _id) != 0)
                {
                    errorMessage = Resource.BugAlreadyExists;
                }
            }
            if (string.IsNullOrEmpty(errorMessage))
            {
                try
                {
                    ac.Bugs.Add(new Bugs { Text = desc, DateFound = DateTime.Now, UserId = User.Identity.GetUserId(), Complex = _id });
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
        public JsonResult EndBug(string id,string desc,string version)
        {
            LogSave.Save(User.Identity.IsAuthenticated ? User.Identity.GetUserId() : "", Request.UserHostAddress, Request.UserAgent, "Home/EndBug", id + " " + desc + " " + version);
            string errorMessage = string.Empty;
            string user_id = User.Identity.GetUserId();
            ApplicationDbContext ac = new ApplicationDbContext();
            int _id = -1;
            int _version = -1;
            if (string.IsNullOrEmpty(errorMessage))
            {
                if(desc.Length==0)
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
                    var bug = ac.Bugs.Where(b => b.Id == _id).First();
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
        public JsonResult DeleteBug(string id)
        {
            LogSave.Save(User.Identity.IsAuthenticated ? User.Identity.GetUserId() : "", Request.UserHostAddress, Request.UserAgent, "Home/DeleteBug", id);
            string errorMessage = string.Empty;
            string user_id = User.Identity.GetUserId();
            ApplicationDbContext ac = new ApplicationDbContext();
            int _id = -1;
            if (string.IsNullOrEmpty(errorMessage))
            {
                try
                {
                    _id = Convert.ToInt32(id);
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
        public ActionResult Logs(int? page)
        {
            int _page = page == null ? 1 : page.Value;
            LogSave.Save(User.Identity.IsAuthenticated ? User.Identity.GetUserId() : "", Request.UserHostAddress, Request.UserAgent, "Home/Logs");
            if (User.Identity.IsAuthenticated && Admin.isAdmin(User.Identity.GetUserName()))
            {
                LogsViewModel model = new LogsViewModel();
                model.Load(_page, RPP);
                return View(model);
            }
            return RedirectToAction("Index");
        }
    }
}