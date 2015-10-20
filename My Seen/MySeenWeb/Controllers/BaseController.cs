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
        private void SetLang()
        {
            if (User.Identity.IsAuthenticated)
            {
                ApplicationDbContext ac = new ApplicationDbContext();
                string user_id = User.Identity.GetUserId();
                try
                {
                    CultureInfoTool.SetCulture(ac.Users.Where(u => u.Id == user_id).First().Culture);
                }
                catch
                {
                    //когда пересоздаю БД 
                    HttpCookie cc = ControllerContext.HttpContext.Request.Cookies[".AspNet.ApplicationCookie"];
                    if (cc != null)
                    {
                        cc.Value = string.Empty;
                        cc.Expires = DateTime.Now.AddMilliseconds(-1);
                        ControllerContext.HttpContext.Response.Cookies.Add(cc);
                    }
                }
            }
        }
        protected override void ExecuteCore()
        {
            SetLang();
            base.ExecuteCore();
        }
        protected override IAsyncResult BeginExecuteCore(AsyncCallback callback, object state)
        {
            SetLang();
            return base.BeginExecuteCore(callback, state);
        }
    }
}