using System;
using System.Linq;
using System.Security.Principal;
using System.Threading;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using MySeenLib;
using MySeenWeb.Add_Code.Services.Logging.NLog;
using MySeenWeb.Models.Meta;
using MySeenWeb.Models.OtherViewModels;
using MySeenWeb.Models.TablesLogic;
using MySeenWeb.Models.Tools;

namespace MySeenWeb.Controllers._Base
{
    public class BaseController : Controller
    {
        private class MySessionObject
        {
            public readonly string Value;

            public MySessionObject(string value)
            {
                Value = value;
            }
        }

        private bool IsCookieEnabled
        {
            //get { return TryReadCookies("TestCookes"); }
            get { return true; }//Чтот голову дурит сессия
        }
        private bool TryReadCookies(string key)
        {
            return ControllerContext.HttpContext.Request.Cookies[key] != null;
        }
        private bool TryReadSession(string key)
        {
            var logger = new NLogLogger();
            logger.Info("TryReadSession key=" + key);
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
            cookie.Path = "/";
            cookie.Expires = DateTime.Now.AddMonths(1);
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
            if (cookie != null)
            {
                if (cookie.Expires.AddDays(7) < DateTime.Now) WriteCookie(key, cookie.Value);//если меньше недели, обновим на месяц
                return cookie.Value;
            }
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

        protected int Theme
        {
            get
            {
                if (TryReadUserSideStorage(UserSideStorageKeys.Theme))
                {
                    var retc = ReadUserSideStorage(UserSideStorageKeys.Theme, -1);
                    if (retc != -1) return retc;
                }

                var ret = 0;
                if (User.Identity.IsAuthenticated)
                {
                    try
                    {
                        var ac = new ApplicationDbContext();
                        var userId = User.Identity.GetUserId();
                        ret = ac.Users.First(u => u.Id == userId).Theme;
                        WriteUserSideStorage(UserSideStorageKeys.Theme, ret);
                    }
                    catch
                    {
                        LogSave.Save(User.Identity.IsAuthenticated ? User.Identity.GetUserId() : "",
                            Request.UserHostAddress, Request.UserAgent, "Theme catch No USER");
                    }
                }
                else ret = 0;
                return ret;
            }
            set
            {
                WriteUserSideStorage(UserSideStorageKeys.Theme, value);
            }
        }
        protected int MarkersOnRoads
        {
            get
            {
                if (TryReadUserSideStorage(UserSideStorageKeys.MarkersOnRoads))
                {
                    var retc = ReadUserSideStorage(UserSideStorageKeys.MarkersOnRoads, 0);
                    if (!string.IsNullOrEmpty(Defaults.EnabledDisabled.GetById(retc))) return retc;
                }

                var ret = 0;
                if (User.Identity.IsAuthenticated)
                {
                    try
                    {
                        var ac = new ApplicationDbContext();
                        var userId = User.Identity.GetUserId();
                        ret = ac.Users.First(u => u.Id == userId).MarkersOnRoads;
                        WriteUserSideStorage(UserSideStorageKeys.MarkersOnRoads, ret);
                    }
                    catch
                    {
                        LogSave.Save(User.Identity.IsAuthenticated ? User.Identity.GetUserId() : "",
                            Request.UserHostAddress, Request.UserAgent, "MarkersOnRoads catch No USER");
                    }
                }
                return ret;
            }
            set
            {
                WriteUserSideStorage(UserSideStorageKeys.MarkersOnRoads, value);
            }
        }

        protected int Rpp
        {
            get
            {
                if (TryReadUserSideStorage(UserSideStorageKeys.RecordPerPage))
                {
                    var retc = ReadUserSideStorage(UserSideStorageKeys.RecordPerPage,
                        (int) Defaults.RecordPerPageBase.Indexes.All);
                    if (!string.IsNullOrEmpty(Defaults.RecordPerPage.GetById(retc)))
                        return retc == (int)Defaults.RecordPerPageBase.Indexes.All
                            ? Defaults.RecordPerPageBase.Values.All
                            : Convert.ToInt32(Defaults.RecordPerPage.GetById(retc));
                }
                var ret = (int) Defaults.RecordPerPageBase.Indexes.All;
                if (User.Identity.IsAuthenticated)
                {
                    try
                    {
                        var ac = new ApplicationDbContext();
                        var userId = User.Identity.GetUserId();
                        ret = ac.Users.First(u => u.Id == userId).RecordPerPage;
                        WriteUserSideStorage(UserSideStorageKeys.RecordPerPage, ret);
                    }
                    catch
                    {
                        LogSave.Save(User.Identity.IsAuthenticated ? User.Identity.GetUserId() : "",
                            Request.UserHostAddress, Request.UserAgent, "RecordPerPage catch No USER");
                    }
                }
                return ret == (int) Defaults.RecordPerPageBase.Indexes.All
                    ? Defaults.RecordPerPageBase.Values.All
                    : Convert.ToInt32(Defaults.RecordPerPage.GetById(ret));
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
                var lang = ReadUserSideStorage(UserSideStorageKeys.Language, (int)Defaults.LanguagesBase.Indexes.English);
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
                        LogSave.Save(User.Identity.IsAuthenticated ? User.Identity.GetUserId() : "", Request.UserHostAddress, Request.UserAgent, "catch No USER");
                    }
                }
                else
                {
                    var userLanguages = Request.UserLanguages;
                    if (userLanguages != null && userLanguages.Any())
                    {
                        //LogSave.Save(User.Identity.IsAuthenticated ? User.Identity.GetUserId() : "", Request.UserHostAddress, Request.UserAgent, "languages", Request.UserLanguages.Aggregate(String.Empty, (current, s) => current + (s + "|")));
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
                        CultureInfoTool.SetCulture(MetaBase.IsBotRus(Request.UserAgent)
                            ? CultureInfoTool.Cultures.Russian
                            : CultureInfoTool.Cultures.English);
                    }
                }
            }
        }

        private void AutoLogin()
        {
            var logger = new NLogLogger();
            const string methodName = "private void AutoLogin()";
            try
            {
                if (!User.Identity.IsAuthenticated)
                {
                    var privateKey = ReadUserSideStorage(UserSideStorageKeys.UserCreditsForAutologin, string.Empty);
                    if (!string.IsNullOrEmpty(privateKey))
                    {
                        var logic = new UserCreditsLogic();
                        if (logic.Verify(privateKey, Request.UserAgent))
                        {
                            var userManager = HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
                            var signInManager = HttpContext.GetOwinContext().Get<ApplicationSignInManager>();
                            var user = userManager.FindByName(logic.UserName);

                            if (user != null)
                            {
                                signInManager.SignIn(user, true, true);

                                var identity = userManager.CreateIdentity(user, DefaultAuthenticationTypes.ApplicationCookie);
                                var myPrincipal = new GenericPrincipal(identity, new string[] {});
                                ControllerContext.HttpContext.User = myPrincipal;
                                HttpContext.User = myPrincipal;
                                Thread.CurrentPrincipal = myPrincipal;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                logger.Error(methodName, ex);
            }
        }
        protected override void ExecuteCore()
        {
            var logger = new NLogLogger();
            const string methodName = "protected override void ExecuteCore()";
            try
            {
                AutoLogin();
                LogSave.Save(User.Identity.IsAuthenticated ? User.Identity.GetUserId() : "", Request.UserHostAddress, Request.UserAgent, Request.Path);
                SetLang();
            }
            catch (Exception ex)
            {
                logger.Error(methodName, ex);
            }
            base.ExecuteCore();
        }
        protected override IAsyncResult BeginExecuteCore(AsyncCallback callback, object state)
        {
            var logger = new NLogLogger();
            const string methodName = "protected override IAsyncResult BeginExecuteCore(AsyncCallback callback, object state)";
            try
            {
                AutoLogin();
                LogSave.Save(User.Identity.IsAuthenticated ? User.Identity.GetUserId() : "", Request.UserHostAddress, Request.UserAgent, Request.Path);
                SetLang();
            }
            catch (Exception ex)
            {
                logger.Error(methodName, ex);
            }
            return base.BeginExecuteCore(callback, state);
        }
    }
}