using System;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MySeenWeb.Models;
using Microsoft.AspNet.Identity;
using MySeenLib;
using MySeenWeb.Models.Tools;

namespace MySeenWeb.Controllers
{
    public class BaseController : Controller
    {
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
            HttpCookie cookie = ControllerContext.HttpContext.Request.Cookies[key];
            if (cookie == null)
            {
                cookie = new HttpCookie(key);
                cookie.Value = defaultValue;
                cookie.Expires = DateTime.Now.AddDays(1);
                ControllerContext.HttpContext.Response.Cookies.Add(cookie);
                return defaultValue;
            }
            return cookie.Value;
        }
        public bool TryReadCookie(string key)
        {
            HttpCookie cookie = ControllerContext.HttpContext.Request.Cookies[key];
            if (cookie == null) return false;
            return true;
        }
        public void WriteCookie(string key, object value)
        {
            WriteCookie(key, value.ToString());
        }
        public void WriteCookie(string key, string value)
        {
            HttpCookie cookie = ControllerContext.HttpContext.Request.Cookies[key];
            if (cookie == null)
            {
                cookie = new HttpCookie(key);
            }
            cookie.Value = value;
            cookie.Expires = DateTime.Now.AddDays(1);
            ControllerContext.HttpContext.Response.Cookies.Add(cookie);
            //ControllerContext.HttpContext.Session[key] = cookie;
        }

        public int Rpp
        {
            get
            {
                if (TryReadCookie(CookieKeys.RecordPerPage))
                {
                    int ret = ReadCookie(CookieKeys.RecordPerPage, Defaults.RecordPerPageBase.IndexAll);
                    if (string.IsNullOrEmpty(Defaults.RecordPerPage.GetById(ret)))
                    {
                        string userId = string.Empty;
                        try
                        {
                            ApplicationDbContext ac = new ApplicationDbContext();
                            userId = User.Identity.GetUserId();
                            ApplicationUser au = ac.Users.First(u => u.Id == userId);
                            ret = au.RecordPerPage;
                            WriteCookie(CookieKeys.RecordPerPage, ret);
                        }
                        catch
                        {
                            LogSave.Save(User.Identity.IsAuthenticated ? User.Identity.GetUserId() : "", Request.UserHostAddress, Request.UserAgent, "RecordPerPage catch No USER", userId);
                        }
                    }
                    return ret == Defaults.RecordPerPageBase.IndexAll ? Defaults.RecordPerPageBase.ValAll : Convert.ToInt32(Defaults.RecordPerPage.GetById(ret));
                }
                else
                {
                    string userId = string.Empty;
                    int ret = Defaults.RecordPerPageBase.IndexAll;
                    try
                    {
                        ApplicationDbContext ac = new ApplicationDbContext();
                        userId = User.Identity.GetUserId();
                        ApplicationUser au = ac.Users.First(u => u.Id == userId);
                        ret = au.RecordPerPage;
                        WriteCookie(CookieKeys.RecordPerPage, au.RecordPerPage);
                    }
                    catch
                    {
                        LogSave.Save(User.Identity.IsAuthenticated ? User.Identity.GetUserId() : "", Request.UserHostAddress, Request.UserAgent, "RecordPerPage catch No USER", userId);
                    }
                    return ret == Defaults.RecordPerPageBase.IndexAll ? Defaults.RecordPerPageBase.ValAll : Convert.ToInt32(Defaults.RecordPerPage.GetById(ret));
                }
            }
            set
            {
                WriteCookie(CookieKeys.RecordPerPage, value);
            }
        }
        private void SetLang()
        {
            if (TryReadCookie(CookieKeys.Language))
            {
                int lang = ReadCookie(CookieKeys.Language, Defaults.LanguagesBase.Indexes.English);
                try
                {
                    CultureInfoTool.SetCulture(Defaults.Languages.GetValDB(lang));
                }
                catch
                {
                    LogSave.Save(User.Identity.IsAuthenticated ? User.Identity.GetUserId() : "", Request.UserHostAddress, Request.UserAgent, "catch SetCulture");
                }
            }
            else
            {
                if (User.Identity.IsAuthenticated)
                {
                    string userId = User.Identity.GetUserId();
                    try
                    {
                        ApplicationDbContext ac = new ApplicationDbContext();
                        ApplicationUser au;
                        au = ac.Users.First(u => u.Id == userId);
                        try
                        {
                            CultureInfoTool.SetCulture(au.Culture);
                        }
                        catch
                        {
                            LogSave.Save(User.Identity.IsAuthenticated ? User.Identity.GetUserId() : "", Request.UserHostAddress, Request.UserAgent, "catch SetCulture");
                        }
                    }
                    catch
                    {
                        LogSave.Save(User.Identity.IsAuthenticated ? User.Identity.GetUserId() : "", Request.UserHostAddress, Request.UserAgent, "catch No USER", userId);
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
            LogSave.Save(User.Identity.IsAuthenticated ? User.Identity.GetUserId() : "", Request.UserHostAddress, Request.UserAgent, "Base");
            SetLang();
            base.ExecuteCore();
        }
        protected override IAsyncResult BeginExecuteCore(AsyncCallback callback, object state)
        {
            LogSave.Save(User.Identity.IsAuthenticated ? User.Identity.GetUserId() : "", Request.UserHostAddress, Request.UserAgent, "Base");
            SetLang();
            return base.BeginExecuteCore(callback, state);
        }
    }
}