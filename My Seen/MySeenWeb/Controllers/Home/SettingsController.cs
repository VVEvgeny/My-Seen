using System;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using MySeenLib;
using MySeenWeb.ActionFilters;
using MySeenWeb.Add_Code;
using MySeenWeb.Add_Code.Services.Logging.NLog;
using MySeenWeb.Controllers._Base;
using MySeenWeb.Models.OtherViewModels;
using MySeenWeb.Models.Tools;
using Nemiro.OAuth;

namespace MySeenWeb.Controllers.Home
{
    public class SettingsController : BaseController
    {
        private ApplicationUserManager UserManager
        {
            get { return HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>(); }
        }

        private ApplicationSignInManager SignInManager
        {
            get { return HttpContext.GetOwinContext().Get<ApplicationSignInManager>(); }
        }
        private IAuthenticationManager AuthenticationManager
        {
            get { return HttpContext.GetOwinContext().Authentication; }
        }

        [Compress]
        //[Authorize]
        [HttpPost]
        public JsonResult SetLanguage(int val)
        {
            var logger = new NLogLogger();
            const string methodName = "public JsonResult SetLanguage(int language)";
            try
            {
                var userId = User.Identity.GetUserId();
                if (!string.IsNullOrEmpty(userId))
                {
                    var ac = new ApplicationDbContext();
                    ac.Users.First(u => u.Id == userId).Culture = Defaults.Languages.GetValDb(val);
                    ac.SaveChanges();
                }
                CultureInfoTool.SetCulture(Defaults.Languages.GetValDb(val));
                WriteUserSideStorage(UserSideStorageKeys.Language, val);
                Defaults.ReloadResources();
                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                logger.Error(methodName, ex);
            }
            return new JsonResult { Data = new { success = false, error = methodName } };
        }

        [Compress]
        //[Authorize]
        [HttpPost]
        public JsonResult SetTheme(int val)
        {
            var logger = new NLogLogger();
            const string methodName = "public JsonResult SetTheme(int language)";
            try
            {
                var userId = User.Identity.GetUserId();
                if (!string.IsNullOrEmpty(userId))
                {
                    var ac = new ApplicationDbContext();
                    ac.Users.First(u => u.Id == userId).Theme = val;
                    ac.SaveChanges();
                }
                Theme = val;
                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                logger.Error(methodName, ex);
            }
            return new JsonResult { Data = new { success = false, error = methodName } };
        }

        [Compress]
        //[Authorize]
        [HttpPost]
        public JsonResult SetRpp(int val)
        {
            var logger = new NLogLogger();
            const string methodName = "public JsonResult SetRpp(int rpp)";
            try
            {
                var userId = User.Identity.GetUserId();
                if (!string.IsNullOrEmpty(userId))
                {
                    var ac = new ApplicationDbContext();
                    ac.Users.First(u => u.Id == userId).RecordPerPage = val;
                    ac.SaveChanges();
                }
                Rpp = val;
                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                logger.Error(methodName, ex);
            }
            return new JsonResult { Data = new { success = false, error = methodName } };
        }

        [Compress]
        [Authorize]
        [HttpPost]
        public JsonResult SetMor(int val)
        {
            var logger = new NLogLogger();
            const string methodName = "public JsonResult SetMor(int mor)";
            try
            {
                var ac = new ApplicationDbContext();
                var userId = User.Identity.GetUserId();
                ac.Users.First(u => u.Id == userId).MarkersOnRoads = val;
                ac.SaveChanges();
                MarkersOnRoads = val;
                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                logger.Error(methodName, ex);
            }
            return new JsonResult { Data = new { success = false, error = methodName } };
        }

        [Compress]
        [Authorize]
        [HttpPost]
        public JsonResult SetPassword(string password, string newPassword)
        {
            var logger = new NLogLogger();
            const string methodName = "public JsonResult SetPassword(string password, string newPassword, string passwordConfirm)";
            try
            {
                if (string.IsNullOrEmpty(password)) //create new
                {
                    var result = UserManager.AddPassword(User.Identity.GetUserId(), newPassword);
                    if (result.Succeeded)
                    {
                        var user = UserManager.FindById(User.Identity.GetUserId());
                        if (user != null)
                        {
                            SignInManager.SignIn(user, isPersistent: false, rememberBrowser: false);
                        }
                        return Json(new { success = true });
                    }
                    logger.Info(result.Errors.First());
                    return new JsonResult { Data = new { success = false, error = result.Errors.First() } };
                }
                else //Change current
                {
                    var result = UserManager.ChangePassword(User.Identity.GetUserId(), password, newPassword);
                    if (result.Succeeded)
                    {
                        var user = UserManager.FindById(User.Identity.GetUserId());
                        if (user != null)
                        {
                            SignInManager.SignIn(user, isPersistent: false, rememberBrowser: false);
                        }
                        return Json(new { success = true });
                    }
                    logger.Info(result.Errors.First());
                    return new JsonResult { Data = new { success = false, error = result.Errors.First() } };
                }
            }
            catch (Exception ex)
            {
                logger.Error(methodName, ex);
            }
            return new JsonResult { Data = new { success = false, error = methodName } };
        }

        [Compress]
        [Authorize]
        [HttpPost]
        public JsonResult GetLogins()
        {
            var logger = new NLogLogger();
            const string methodName = "public JsonResult GetLogins()";
            try
            {
                var userLogins = UserManager.GetLogins(User.Identity.GetUserId());
                var otherLogins = AuthenticationManager.GetExternalAuthenticationTypes().Where(auth => userLogins.All(ul => auth.AuthenticationType != ul.LoginProvider)).ToList();
                //Не поддерживает сериализацию var otherLoginsNemiro = OAuthManager.RegisteredClients;

                return new JsonResult { Data = new { UserLogins = userLogins, OtherLogins = otherLogins
                //    , OtherLoginsNemiro = otherLoginsNemiro 
                } };
            }
            catch (Exception ex)
            {
                logger.Error(methodName, ex);
            }
            return new JsonResult { Data = new { success = false, error = methodName } };
        }

        [Compress]
        [Authorize]
        [HttpPost]
        public JsonResult RemoveLogin(string loginProvider, string providerKey)
        {
            var logger = new NLogLogger();
            const string methodName = "public async JsonResult RemoveLogin(string loginProvider, string providerKey)";
            try
            {
                var result = UserManager.RemoveLogin(User.Identity.GetUserId(), new UserLoginInfo(loginProvider, providerKey));
                if (result.Succeeded)
                {
                    var user = UserManager.FindById(User.Identity.GetUserId());
                    if (user != null)
                    {
                        SignInManager.SignIn(user, isPersistent: false, rememberBrowser: false);
                    }
                    return Json(new { success = true });
                }
                logger.Info(result.Errors.First());
                return new JsonResult { Data = new { success = false, error = result.Errors.First() } };
            }
            catch (Exception ex)
            {
                logger.Error(methodName, ex);
            }
            return new JsonResult { Data = new { success = false, error = methodName } };
        }

        [Compress]
        [Authorize]
        [HttpPost]
        public ActionResult AddLogin(string provider)
        {
            var logger = new NLogLogger();
            const string methodName = "public ActionResult AddLogin(string provider)";
            try
            {
                if (provider == ExternalNotOwinProviders.Yandex
                    || provider == ExternalNotOwinProviders.MailRu
                    )
                {
                    //logger.Info("provider YANDEX");
                    var returnUrl = MySeenWebApi.ApiHost + "/Account/ExternalLoginCallback";
                    return Redirect(OAuthWeb.GetAuthorizationUrl(provider, returnUrl));
                }
                // Request a redirect to the external login provider to link a login for the current user
                return new ChallengeResult(provider, Url.Action("LinkLoginCallback", "Settings"), User.Identity.GetUserId());
            }
            catch (Exception ex)
            {
                logger.Error(methodName, ex);
            }
            return new JsonResult { Data = new { success = false, error = methodName } };
        }
        // Used for XSRF protection when adding external logins
        private const string XsrfKey = "XsrfId";
        public async Task<ActionResult> LinkLoginCallback()
        {
            var logger = new NLogLogger();
            const string methodName = "public async Task<ActionResult> LinkLoginCallback()";
            try
            {
                var loginInfo = await AuthenticationManager.GetExternalLoginInfoAsync(XsrfKey, User.Identity.GetUserId());
                if (loginInfo == null)
                {
                    logger.Info("error GetExternalLoginInfoAsync LinkLoginCallback User=" + User.Identity.GetUserName());
                    return RedirectToAction("Index", "Json");
                }
                var result = await UserManager.AddLoginAsync(User.Identity.GetUserId(), loginInfo.Login);
                if (!result.Succeeded)
                {
                    logger.Info("error AddLoginAsync LinkLoginCallback User=" + User.Identity.GetUserName());
                    return RedirectToAction("Index", "Json");
                }
            }
            catch (Exception ex)
            {
                logger.Error(methodName, ex);
            }
            return RedirectToAction("Index", "Json");
        }
        internal class ChallengeResult : HttpUnauthorizedResult
        {
            public ChallengeResult(string provider, string redirectUri)
                : this(provider, redirectUri, null)
            {
            }

            public ChallengeResult(string provider, string redirectUri, string userId)
            {
                LoginProvider = provider;
                RedirectUri = redirectUri;
                UserId = userId;
            }

            public string LoginProvider { get; set; }
            public string RedirectUri { get; set; }
            public string UserId { get; set; }

            public override void ExecuteResult(ControllerContext context)
            {
                var properties = new AuthenticationProperties { RedirectUri = RedirectUri };
                if (UserId != null)
                {
                    properties.Dictionary[XsrfKey] = UserId;
                }
                context.HttpContext.GetOwinContext().Authentication.Challenge(properties, LoginProvider);
            }
        }
    }
}