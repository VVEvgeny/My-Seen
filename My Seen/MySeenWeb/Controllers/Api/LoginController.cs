using System;
using System.Web.Http;
using MySeenWeb.Add_Code.Services.Logging.NLog;
using static MySeenLib.MySeenWebApi;
using static MySeenLib.MySeenWebApi.SyncJsonAnswer;
using MySeenWeb.Models.OtherViewModels;
using System.Linq;
using System.Web;
using Microsoft.AspNet.Identity.Owin;
using MySeenWeb.Add_Code;

namespace MySeenWeb.Controllers.Api
{
    public class LoginController : BaseApiController
    {
        private ApplicationSignInManager SignInManager => HttpContext.Current.GetOwinContext().GetUserManager<ApplicationSignInManager>();

        public IHttpActionResult Get(string userKey, int mode, int apiVersion)
        {
            var logger = new NLogLogger();
            const string methodName = "public IHttpActionResult Get(string userKey, int mode, int apiVersion)";
            try
            {
                if (apiVersion != ApiVersion)
                {
                    return Ok(new SyncJsonAnswer { Value = Values.NoLongerSupportedVersion });
                }
                if ((SyncModesApiLogin)mode == SyncModesApiLogin.GetKey)
                {
                    var login = userKey.Split(';')[0];
                    var password = userKey.Split(';')[1];

                    if (SignInManager.PasswordSignIn(login.ToLower(), password, false, false) == SignInStatus.Success)
                    {
                        var ac = new ApplicationDbContext();
                        var user = ac.Users.First(u => u.UserName.ToLower() == login.ToLower());

                        return Ok(new SyncJsonAnswer { Value = Values.Ok, Data = Md5Tools.Get(user.Id) });
                    }
                    return Ok(new SyncJsonAnswer {Value = Values.UserNotExist});
                }
                return Ok(new SyncJsonAnswer { Value = Values.BadRequestMode });
            }
            catch (Exception ex)
            {
                logger.Error(methodName, ex);
            }
            return Ok(new SyncJsonAnswer { Value = Values.SomeErrorObtained });
        }
    }
}
