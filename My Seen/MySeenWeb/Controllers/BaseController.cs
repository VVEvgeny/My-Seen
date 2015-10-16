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
                CultureInfoTool.SetCulture(ac.Users.Where(u => u.Id == user_id).First().Culture);
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