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
                AllFilms af = new AllFilms();
                if(CookieStore.GetCookie(AllFilms.AFCookies.CoockieSelectedKey)==string.Empty)
                {
                    af.Selected = AllFilms.eSelected.Films;
                    CookieStore.SetCookie(AllFilms.AFCookies.CoockieSelectedKey, AllFilms.AFCookies.CoockieSelectedValueFilms);
                }
                af.LoadFilms(User.Identity.GetUserId());
                return View(af);
            }
            return View();
        }
    }
}