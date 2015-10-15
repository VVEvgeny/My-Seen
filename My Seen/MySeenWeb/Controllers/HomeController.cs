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
    [RequireHttps]
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            if (User.Identity.IsAuthenticated)
            {
                HomeViewModel af = new HomeViewModel();
                HttpCookie cookie = ControllerContext.HttpContext.Request.Cookies[HomeViewModel.AFCookies.CoockieSelectedKey];
                if (cookie == null)
                {
                    af.Selected = HomeViewModel.eSelected.Films;
                    cookie = new HttpCookie(HomeViewModel.AFCookies.CoockieSelectedKey);
                    cookie.Value = HomeViewModel.AFCookies.CoockieSelectedValueFilms;
                    cookie.Expires = DateTime.Now.AddDays(1);
                    ControllerContext.HttpContext.Response.Cookies.Add(cookie);
                }
                else
                {
                    if (cookie.Value == HomeViewModel.AFCookies.CoockieSelectedValueSerials)af.Selected = HomeViewModel.eSelected.Serials;
                    else af.Selected = HomeViewModel.eSelected.Films;
                }
                af.LoadSelectList();
                if(af.Selected==HomeViewModel.eSelected.Films) af.LoadFilms(User.Identity.GetUserId());
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
            cc.Value = selected;
            cc.Expires = DateTime.Now.AddDays(1);
            ControllerContext.HttpContext.Response.Cookies.Add(cc);
            return Json(new { success = true });
        }
        [HttpPost]
        public JsonResult AddFilm(string name, string genre, string rating)
        {
            string errorMessage = string.Empty;
            if (string.IsNullOrEmpty(errorMessage))
            {
                LibTools.Validation.ValidateName(ref errorMessage, name);
            }
            if (string.IsNullOrEmpty(errorMessage))
            {
                try
                {
                    ApplicationDbContext ac = new ApplicationDbContext();
                    Films f = new Films { Name = name, Genre = Convert.ToInt32(genre), Rate = Convert.ToInt32(rating), DateSee = DateTime.Now, DateChange = DateTime.Now, UserId = User.Identity.GetUserId() };
                    ac.Films.Add(f);
                    ac.SaveChanges();
                }
                catch (Exception e)
                {
                    errorMessage = "Error work with DB=" + e.Message;
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
            string errorMessage = string.Empty;
            if (string.IsNullOrEmpty(errorMessage))
            {
                LibTools.Validation.ValidateName(ref errorMessage, name);
            }
            if (string.IsNullOrEmpty(errorMessage))
            {
                ApplicationDbContext ac = new ApplicationDbContext();
                try
                {
                    string user_id = User.Identity.GetUserId();
                    int iid = (Convert.ToInt32(id));
                    Films film = ac.Films.Where(f => f.UserId == user_id && f.Id == iid).First();
                    film.Name = name;
                    film.Genre = Convert.ToInt32(genre);
                    film.Rate = Convert.ToInt32(rating);
                    film.DateChange = DateTime.Now;
                    ac.SaveChanges();
                }
                catch (Exception e)
                {
                    errorMessage = "Error work with DB=" + e.Message;
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
            string errorMessage = string.Empty;
            if (string.IsNullOrEmpty(errorMessage))
            {
                LibTools.Validation.ValidateName(ref errorMessage, name);
            }
            if (string.IsNullOrEmpty(errorMessage))
            {
                try
                {
                    ApplicationDbContext ac = new ApplicationDbContext();
                    if (string.IsNullOrEmpty(season)) season = "1";
                    if (string.IsNullOrEmpty(series)) series = "1";
                    Serials s = new Serials { Name = name, LastSeason = Convert.ToInt32(season), LastSeries = Convert.ToInt32(series), Genre = Convert.ToInt32(genre), Rate = Convert.ToInt32(rating), DateBegin = DateTime.Now, DateLast = DateTime.Now, DateChange = DateTime.Now, UserId = User.Identity.GetUserId() };
                    ac.Serials.Add(s);
                    ac.SaveChanges();
                }
                catch (Exception e)
                {
                    errorMessage = "Error work with DB=" + e.Message;
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
            string errorMessage = string.Empty;
            if (string.IsNullOrEmpty(errorMessage))
            {
                LibTools.Validation.ValidateName(ref errorMessage, name);
            }
            if (string.IsNullOrEmpty(errorMessage))
            {
                ApplicationDbContext ac = new ApplicationDbContext();
                try
                {
                    string user_id = User.Identity.GetUserId();
                    int iid = (Convert.ToInt32(id));
                    Serials film = ac.Serials.Where(f => f.UserId == user_id && f.Id == iid).First();
                    film.Name = name;
                    if (film.LastSeason != Convert.ToInt32(season) || film.LastSeries != Convert.ToInt32(series))
                    {
                        film.DateLast = DateTime.Now;
                    }
                    film.LastSeason = Convert.ToInt32(season);
                    film.LastSeries = Convert.ToInt32(series);
                    film.Genre = Convert.ToInt32(genre);
                    film.Rate = Convert.ToInt32(rating);
                    film.DateChange = DateTime.Now;
                    ac.SaveChanges();
                }
                catch (Exception e)
                {
                    errorMessage = "Error work with DB=" + e.Message;
                }
            }
            if (!string.IsNullOrEmpty(errorMessage))
            {
                return new JsonResult { Data = new { success = false, error = errorMessage } };
            }
            return Json(new { success = true });
        }
    }
}