using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MySeenWeb.Models;
using Microsoft.AspNet.Identity;

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
            ApplicationDbContext ac = new ApplicationDbContext();
            Films f = new Films { Name = name, Genre = Convert.ToInt32(genre), Rate = Convert.ToInt32(rating), DateSee = DateTime.Now, DateChange = DateTime.Now, UserId = User.Identity.GetUserId() };
            ac.Films.Add(f);
            ac.SaveChanges();
            return Json(new { success = true });
        }
    }
}