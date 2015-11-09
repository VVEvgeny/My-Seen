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
    public class BaseController : Controller
    {
        private string CoockieSelectedKey = "RecordPerPage";
        public int RPP
        {
            get
            {
                HttpCookie cookie = ControllerContext.HttpContext.Request.Cookies[CoockieSelectedKey];
                string user_id = User.Identity.GetUserId();

                if (cookie == null)
                {
                    cookie = new HttpCookie(CoockieSelectedKey);
                    try
                    {
                        ApplicationDbContext ac = new ApplicationDbContext();
                        ApplicationUser au = ac.Users.Where(u => u.Id == user_id).First();
                        cookie.Value = au.RecordPerPage.ToString();
                    }
                    catch
                    {
                        LogSave.Save(User.Identity.IsAuthenticated ? User.Identity.GetUserId() : "", Request.UserHostAddress, Request.UserAgent, "RPP catch No USER", user_id);
                        cookie.Value = "0";
                    }
                    cookie.Expires = DateTime.Now.AddDays(1);
                    ControllerContext.HttpContext.Response.Cookies.Add(cookie);
                }
                else
                {
                    int ret = 0;
                    try
                    {
                        ret = Convert.ToInt32(cookie.Value);
                        if (string.IsNullOrEmpty(Defaults.RecordPerPage.GetById(ret))) throw new Exception();

                        //теперь приведем к норм виду 0= все
                        if (ret == Defaults.RecordPerPageBase.IndexAll)
                        {
                            ret = Defaults.RecordPerPageBase.ValAll;
                        }
                        else ret = Convert.ToInt32(Defaults.RecordPerPage.GetById(ret));
                    }
                    catch
                    {
                        ControllerContext.HttpContext.Request.Cookies.Remove(CoockieSelectedKey);
                        ret = Defaults.RecordPerPageBase.ValAll;
                    }
                    return ret;
                }
                return Defaults.RecordPerPageBase.ValAll;
            }
            set
            {
                HttpCookie cookie = ControllerContext.HttpContext.Request.Cookies[CoockieSelectedKey];
                if (cookie == null)
                {
                    cookie = new HttpCookie(CoockieSelectedKey);
                }
                cookie.Value = value.ToString();
                cookie.Expires = DateTime.Now.AddDays(1);
                ControllerContext.HttpContext.Response.Cookies.Add(cookie);
            }
        }
        private void SetLang()
        {
            if (User.Identity.IsAuthenticated)
            {
                ApplicationDbContext ac = new ApplicationDbContext();
                string user_id = User.Identity.GetUserId();
                ApplicationUser au = null;
                try
                {
                    au = ac.Users.Where(u => u.Id == user_id).First();
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
                    LogSave.Save(User.Identity.IsAuthenticated ? User.Identity.GetUserId() : "", Request.UserHostAddress, Request.UserAgent, "catch No USER", user_id);
                }
            }
            else
            {
                var userLanguages = Request.UserLanguages;
                if(userLanguages.Count()>0)
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