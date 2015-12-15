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
        public class MySessionObject
        {
            public string Value;

            public MySessionObject(string value)
            {
                Value = value;
            }
        }
        public bool IsCookieEnabled
        {
            get { return TryReadCookies("TestCookes"); }
            //get { return false; }
        }
        private bool TryReadCookies(string key)
        {
            return ControllerContext.HttpContext.Request.Cookies[key] != null;
        }
        private bool TryReadSession(string key)
        {
            return ControllerContext.HttpContext.Session != null && ControllerContext.HttpContext.Session[key] != null;
        }

        private bool TryReadUserSideStorage(string key)
        {
            return IsCookieEnabled ? TryReadCookies(key) : TryReadSession(key);
        }

        private void WriteCookie(string key, string value)
        {
            var cookie = ControllerContext.HttpContext.Request.Cookies[key] ?? new HttpCookie(key);
            cookie.Value = value;
            cookie.Expires = DateTime.Now.AddDays(14);
            ControllerContext.HttpContext.Response.Cookies.Add(cookie);
        }
        private void WriteSession(string key, string value)
        {
            if (ControllerContext.HttpContext.Session != null) ControllerContext.HttpContext.Session.Add(key, new MySessionObject(value));
        }
        private void WriteUserSideStorage(string key, string value)
        {
            if (IsCookieEnabled) WriteCookie(key, value);
            else WriteSession(key, value);
        }

        private string ReadCookie(string key, string defaultValue)
        {
            var cookie = ControllerContext.HttpContext.Request.Cookies[key];
            if (cookie != null) return cookie.Value;
            WriteCookie(key, defaultValue);
            return defaultValue;
        }
        private string ReadSession(string key, string defaultValue)
        {
            if (ControllerContext.HttpContext.Session == null) return string.Empty;

            var obj = (MySessionObject)ControllerContext.HttpContext.Session[key];
            if (obj != null) return obj.Value;
            WriteSession(key, defaultValue);

            return string.Empty;
        }
        private string ReadUserSideStorage(string key, string defaultValue)
        {
            return IsCookieEnabled ? ReadCookie(key, defaultValue) : ReadSession(key, defaultValue);
        }

        public int ReadUserSideStorage(string key, int defaultValue)
        {
            int readed;
            try
            {
                readed = Convert.ToInt32(ReadUserSideStorage(key, defaultValue.ToString()));
            }
            catch
            {
                WriteUserSideStorage(key, defaultValue);
                readed = defaultValue;
            }
            return readed;
        }
        public void WriteUserSideStorage(string key, object value)
        {
            WriteUserSideStorage(key, value.ToString());
        }
        public int MarkersOnRoads
        {
            get
            {
                if (TryReadUserSideStorage(UserSideStorageKeys.MarkersOnRoads))
                {
                    var ret = ReadUserSideStorage(UserSideStorageKeys.MarkersOnRoads, 0);
                    if (!string.IsNullOrEmpty(Defaults.MarkersOnRoads.GetById(ret))) return ret;
                    var userId = string.Empty;
                    if (User.Identity.IsAuthenticated)
                    {
                        try
                        {
                            var ac = new ApplicationDbContext();
                            userId = User.Identity.GetUserId();
                            var au = ac.Users.First(u => u.Id == userId);
                            ret = au.MarkersOnRoads;
                            WriteUserSideStorage(UserSideStorageKeys.RecordPerPage, ret);
                        }
                        catch
                        {
                            LogSave.Save(User.Identity.IsAuthenticated ? User.Identity.GetUserId() : "",
                                Request.UserHostAddress, Request.UserAgent, "MarkersOnRoads catch No USER", userId);
                        }
                    }
                    return ret;
                }
                else
                {
                    var userId = string.Empty;
                    var ret = 0;
                    if (User.Identity.IsAuthenticated)
                    {
                        try
                        {
                            var ac = new ApplicationDbContext();
                            userId = User.Identity.GetUserId();
                            var au = ac.Users.First(u => u.Id == userId);
                            ret = au.MarkersOnRoads;
                            WriteUserSideStorage(UserSideStorageKeys.MarkersOnRoads, au.MarkersOnRoads);
                        }
                        catch
                        {
                            LogSave.Save(User.Identity.IsAuthenticated ? User.Identity.GetUserId() : "",
                                Request.UserHostAddress, Request.UserAgent, "MarkersOnRoads catch No USER", userId);
                        }
                    }
                    return ret;
                }
            }
            set
            {
                WriteUserSideStorage(UserSideStorageKeys.MarkersOnRoads, value);
            }
        }
        public int Rpp
        {
            get
            {
                if (TryReadUserSideStorage(UserSideStorageKeys.RecordPerPage))
                {
                    var ret = ReadUserSideStorage(UserSideStorageKeys.RecordPerPage, Defaults.RecordPerPageBase.Indexes.All);
                    if (!string.IsNullOrEmpty(Defaults.RecordPerPage.GetById(ret)))
                        return ret == Defaults.RecordPerPageBase.Indexes.All
                            ? Defaults.RecordPerPageBase.Values.All
                            : Convert.ToInt32(Defaults.RecordPerPage.GetById(ret));
                    var userId = string.Empty;
                    if (User.Identity.IsAuthenticated)
                    {
                        try
                        {
                            var ac = new ApplicationDbContext();
                            userId = User.Identity.GetUserId();
                            var au = ac.Users.First(u => u.Id == userId);
                            ret = au.RecordPerPage;
                            WriteUserSideStorage(UserSideStorageKeys.RecordPerPage, ret);
                        }
                        catch
                        {
                            LogSave.Save(User.Identity.IsAuthenticated ? User.Identity.GetUserId() : "",
                                Request.UserHostAddress, Request.UserAgent, "RecordPerPage catch No USER", userId);
                        }
                    }
                    return ret == Defaults.RecordPerPageBase.Indexes.All ? Defaults.RecordPerPageBase.Values.All : Convert.ToInt32(Defaults.RecordPerPage.GetById(ret));
                }
                else
                {
                    var userId = string.Empty;
                    var ret = Defaults.RecordPerPageBase.Indexes.All;
                    if (User.Identity.IsAuthenticated)
                    {
                        try
                        {
                            var ac = new ApplicationDbContext();
                            userId = User.Identity.GetUserId();
                            var au = ac.Users.First(u => u.Id == userId);
                            ret = au.RecordPerPage;
                            WriteUserSideStorage(UserSideStorageKeys.RecordPerPage, au.RecordPerPage);
                        }
                        catch
                        {
                            LogSave.Save(User.Identity.IsAuthenticated ? User.Identity.GetUserId() : "",
                                Request.UserHostAddress, Request.UserAgent, "RecordPerPage catch No USER", userId);
                        }
                    }
                    return ret == Defaults.RecordPerPageBase.Indexes.All ? Defaults.RecordPerPageBase.Values.All : Convert.ToInt32(Defaults.RecordPerPage.GetById(ret));
                }
            }
            set
            {
                WriteUserSideStorage(UserSideStorageKeys.RecordPerPage, value);
            }
        }
        private void SetLang()
        {
            if (TryReadUserSideStorage(UserSideStorageKeys.Language))
            {
                var lang = ReadUserSideStorage(UserSideStorageKeys.Language, Defaults.LanguagesBase.Indexes.English);
                try
                {
                    CultureInfoTool.SetCulture(Defaults.Languages.GetValDb(lang));
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

        private void SaveBotLog()
        {
            var user = Request.UserAgent != null && Request.UserAgent.Contains("YandexMetrika")
                ? "YandexMetrika"
                : Request.UserAgent != null && Request.UserAgent.Contains("Googlebot")
                    ? "Googlebot"
                    : Request.UserAgent != null && Request.UserAgent.Contains("Google favicon")
                        ? "Google favicon"
                        : Request.UserAgent;
            LogSave.Save(user, Request.UserHostAddress, Request.UserAgent, "Base"
                ,
                Request.Path + " " +
                (Request.QueryString.HasKeys()
                    ? Request.QueryString.Cast<object>().Aggregate(string.Empty, (current, str) => current + str)
                    : ""));
        }

        protected override void ExecuteCore()
        {
            if (Request.UserAgent != null &&
                (Request.UserAgent.Contains("YandexMetrika") || Request.UserAgent.Contains("Googlebot") ||
                 Request.UserAgent.Contains("Google favicon")))
            {
                SaveBotLog();
            }
            else
            {
                LogSave.Save(User.Identity.IsAuthenticated ? User.Identity.GetUserId() : "", Request.UserHostAddress,
                    Request.UserAgent, "Base");
            }
            SetLang();
            base.ExecuteCore();
        }
        protected override IAsyncResult BeginExecuteCore(AsyncCallback callback, object state)
        {
            if (Request.UserAgent != null &&
                (Request.UserAgent.Contains("YandexMetrika") || Request.UserAgent.Contains("Googlebot") ||
                 Request.UserAgent.Contains("Google favicon")))
            {
                SaveBotLog();
            }
            else
            {
                LogSave.Save(User.Identity.IsAuthenticated ? User.Identity.GetUserId() : "", Request.UserHostAddress,
                    Request.UserAgent, "BaseA", Request.Path);
            }
            SetLang();
            return base.BeginExecuteCore(callback, state);
        }
    }
}