using System;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using MySeenLib;
using MySeenWeb.Models.Database;
using MySeenWeb.Models.Tools;

namespace MySeenWeb.Controllers
{
    public class BaseController : Controller
    {
        public int Rpp
        {
            get
            {
                if (TryReadCookie(CookieKeys.RecordPerPage))
                {
                    var ret = ReadCookie(CookieKeys.RecordPerPage, Defaults.RecordPerPageBase.IndexAll);
                    if (!string.IsNullOrEmpty(Defaults.RecordPerPage.GetById(ret)))
                        return ret == Defaults.RecordPerPageBase.IndexAll
                            ? Defaults.RecordPerPageBase.ValAll
                            : Convert.ToInt32(Defaults.RecordPerPage.GetById(ret));
                    var userId = string.Empty;
                    try
                    {
                        var ac = new ApplicationDbContext();
                        userId = User.Identity.GetUserId();
                        var au = ac.Users.First(u => u.Id == userId);
                        ret = au.RecordPerPage;
                        WriteCookie(CookieKeys.RecordPerPage, ret);
                    }
                    catch
                    {
                        LogSave.Save(User.Identity.IsAuthenticated ? User.Identity.GetUserId() : "",
                            Request.UserHostAddress, Request.UserAgent, "RecordPerPage catch No USER", userId);
                    }
                    return ret == Defaults.RecordPerPageBase.IndexAll
                        ? Defaults.RecordPerPageBase.ValAll
                        : Convert.ToInt32(Defaults.RecordPerPage.GetById(ret));
                }
                else
                {
                    var userId = string.Empty;
                    var ret = Defaults.RecordPerPageBase.IndexAll;
                    try
                    {
                        var ac = new ApplicationDbContext();
                        userId = User.Identity.GetUserId();
                        var au = ac.Users.First(u => u.Id == userId);
                        ret = au.RecordPerPage;
                        WriteCookie(CookieKeys.RecordPerPage, au.RecordPerPage);
                    }
                    catch
                    {
                        LogSave.Save(User.Identity.IsAuthenticated ? User.Identity.GetUserId() : "",
                            Request.UserHostAddress, Request.UserAgent, "RecordPerPage catch No USER", userId);
                    }
                    return ret == Defaults.RecordPerPageBase.IndexAll
                        ? Defaults.RecordPerPageBase.ValAll
                        : Convert.ToInt32(Defaults.RecordPerPage.GetById(ret));
                }
            }
            set { WriteCookie(CookieKeys.RecordPerPage, value); }
        }

        public int ReadCookie(string key, int defaultValue)
        {
            int readed;
            try
            {
                readed = Convert.ToInt32(ReadCookie(key, defaultValue.ToString()));
            }
            catch
            {
                WriteCookie(key, defaultValue);
                readed = defaultValue;
            }
            return readed;
        }

        public string ReadCookie(string key, string defaultValue)
        {
            var cookie = ControllerContext.HttpContext.Request.Cookies[key];
            if (cookie != null) return cookie.Value;
            cookie = new HttpCookie(key)
            {
                Value = defaultValue,
                Expires = DateTime.Now.AddDays(1)
            };
            ControllerContext.HttpContext.Response.Cookies.Add(cookie);
            return defaultValue;
        }

        public bool TryReadCookie(string key)
        {
            var cookie = ControllerContext.HttpContext.Request.Cookies[key];
            return cookie != null;
        }

        public void WriteCookie(string key, object value)
        {
            WriteCookie(key, value.ToString());
        }

        public void WriteCookie(string key, string value)
        {
            var cookie = ControllerContext.HttpContext.Request.Cookies[key] ?? new HttpCookie(key);
            cookie.Value = value;
            cookie.Expires = DateTime.Now.AddDays(1);
            ControllerContext.HttpContext.Response.Cookies.Add(cookie);
            //ControllerContext.HttpContext.Session[key] = cookie;
        }

        private void SetLang()
        {
            if (TryReadCookie(CookieKeys.Language))
            {
                var lang = ReadCookie(CookieKeys.Language, Defaults.LanguagesBase.Indexes.English);
                try
                {
                    CultureInfoTool.SetCulture(Defaults.Languages.GetValDb(lang));
                }
                catch
                {
                    LogSave.Save(User.Identity.IsAuthenticated ? User.Identity.GetUserId() : "", Request.UserHostAddress,
                        Request.UserAgent, "catch SetCulture");
                }
            }
            else
            {
                if (User.Identity.IsAuthenticated)
                {
                    var userId = User.Identity.GetUserId();
                    try
                    {
                        var ac = new ApplicationDbContext();
                        var au = ac.Users.First(u => u.Id == userId);
                        try
                        {
                            CultureInfoTool.SetCulture(au.Culture);
                        }
                        catch
                        {
                            LogSave.Save(User.Identity.IsAuthenticated ? User.Identity.GetUserId() : "",
                                Request.UserHostAddress, Request.UserAgent, "catch SetCulture");
                        }
                    }
                    catch
                    {
                        LogSave.Save(User.Identity.IsAuthenticated ? User.Identity.GetUserId() : "",
                            Request.UserHostAddress, Request.UserAgent, "catch No USER", userId);
                    }
                }
                else
                {
                    var userLanguages = Request.UserLanguages;
                    if (userLanguages != null && userLanguages.Any())
                    {
                        try
                        {
                            CultureInfoTool.SetCulture(userLanguages[0]);
                        }
                        catch
                        {
                            CultureInfoTool.SetCulture(CultureInfoTool.Cultures.English);
                        }
                    }
                    else
                    {
                        CultureInfoTool.SetCulture(CultureInfoTool.Cultures.English);
                    }
                }
            }
        }

        protected override void ExecuteCore()
        {
            LogSave.Save(User.Identity.IsAuthenticated ? User.Identity.GetUserId() : "", Request.UserHostAddress,
                Request.UserAgent, "Base");
            SetLang();
            base.ExecuteCore();
        }

        protected override IAsyncResult BeginExecuteCore(AsyncCallback callback, object state)
        {
            LogSave.Save(User.Identity.IsAuthenticated ? User.Identity.GetUserId() : "", Request.UserHostAddress,
                Request.UserAgent, "Base");
            SetLang();
            return base.BeginExecuteCore(callback, state);
        }
    }
}