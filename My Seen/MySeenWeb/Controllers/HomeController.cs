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
        public ActionResult Index()
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
                    //cookie.Value = HomeViewModel.AFCookies.CoockieSelectedValueFilms;
                    cookie.Value = Defaults.CategoryBase.FilmIndex.ToString();
                    cookie.Expires = DateTime.Now.AddDays(1);
                    ControllerContext.HttpContext.Response.Cookies.Add(cookie);
                }
                else
                {
                    if (cookie.Value == Defaults.CategoryBase.SerialIndex.ToString()) af.Selected = Defaults.Categories.GetById(Defaults.CategoryBase.SerialIndex);
                    else af.Selected = Defaults.Categories.GetById(Defaults.CategoryBase.FilmIndex);
                }
                af.LoadSelectList();
                if (af.Selected == Defaults.Categories.GetById(Defaults.CategoryBase.FilmIndex)) af.LoadFilms(User.Identity.GetUserId());
                else af.LoadSerials(User.Identity.GetUserId());

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
        public JsonResult AddFilm(string name, string genre, string rating)
        {
            LogSave.Save(User.Identity.IsAuthenticated ? User.Identity.GetUserId() : "", Request.UserHostAddress, Request.UserAgent, "Home/AddFilm",name);
            string errorMessage = string.Empty;
            string user_id = User.Identity.GetUserId();
            if (string.IsNullOrEmpty(errorMessage))
            {
                Validations.ValidateName(ref errorMessage, name);
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
        [HttpPost]
        public JsonResult EditFilm(string id,string name, string genre, string rating)
        {
            LogSave.Save(User.Identity.IsAuthenticated ? User.Identity.GetUserId() : "", Request.UserHostAddress, Request.UserAgent, "Home/EditFilm",id);
            string errorMessage = string.Empty;
            string user_id = User.Identity.GetUserId();
            if (string.IsNullOrEmpty(errorMessage))
            {
                Validations.ValidateName(ref errorMessage, name);
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
        [HttpPost]
        public JsonResult AddSerial(string name, string season, string series, string genre, string rating)
        {
            LogSave.Save(User.Identity.IsAuthenticated ? User.Identity.GetUserId() : "", Request.UserHostAddress, Request.UserAgent, "Home/AddSerial",name);
            string errorMessage = string.Empty;
            string user_id = User.Identity.GetUserId();
            if (string.IsNullOrEmpty(errorMessage))
            {
                Validations.ValidateName(ref errorMessage, name);
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
        [HttpPost]
        public JsonResult EditSerial(string id, string name, string season, string series, string genre, string rating)
        {
            LogSave.Save(User.Identity.IsAuthenticated ? User.Identity.GetUserId() : "", Request.UserHostAddress, Request.UserAgent, "Home/EditSerial",id);
            string errorMessage = string.Empty;
            string user_id = User.Identity.GetUserId();
            if (string.IsNullOrEmpty(errorMessage))
            {
                Validations.ValidateName(ref errorMessage, name);
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
        public ActionResult Users()
        {
            LogSave.Save(User.Identity.IsAuthenticated ? User.Identity.GetUserId() : "", Request.UserHostAddress, Request.UserAgent, "Home/Users");
            if (User.Identity.IsAuthenticated && Admin.isAdmin(User.Identity.Name))
            {
                UsersViewModel model = new UsersViewModel();
                return View(model);
            }
            return RedirectToAction("Index");
        }
    }
}