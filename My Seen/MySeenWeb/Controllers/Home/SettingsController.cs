using System;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using MySeenWeb.ActionFilters;
using MySeenWeb.Add_Code;
using MySeenWeb.Controllers._Base;
using MySeenWeb.Models.OtherViewModels;
using MySeenWeb.Models.Tools;
using static MySeenLib.CultureInfoTool;
using static MySeenLib.Defaults;

namespace MySeenWeb.Controllers.Home
{
    public class SettingsController : BaseController
    {
        public SettingsController(ICacheService cache) : base(cache)
        {

        }
        private ApplicationUserManager UserManager => HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();

        private ApplicationSignInManager SignInManager => HttpContext.GetOwinContext().Get<ApplicationSignInManager>();

        private IAuthenticationManager AuthenticationManager => HttpContext.GetOwinContext().Authentication;

        [Compress]
        [HttpPost]
        public JsonResult SetLanguage(int val)
        {
            try
            {
                var userId = User.Identity.GetUserId();
                if (!string.IsNullOrEmpty(userId))
                {
                    var ac = new ApplicationDbContext();
                    ac.Users.First(u => u.Id == userId).Culture = Languages.GetValDb(val);
                    ac.SaveChanges();
                }
                SetCulture(Languages.GetValDb(val));
                WriteUserSideStorage(UserSideStorageKeys.Language, val);
                ReloadResources();
                return Json(new { success = true });
            }
            catch (Exception ex)
            {
            }
            return new JsonResult { Data = new { success = false, error = System.Reflection.MethodBase.GetCurrentMethod().Name } };
        }

        [Compress]
        [HttpPost]
        public JsonResult SetTheme(int val)
        {
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
            }
            return new JsonResult { Data = new { success = false, error = System.Reflection.MethodBase.GetCurrentMethod().Name } };
        }

        [Compress]
        [HttpPost]
        public JsonResult SetRpp(int val)
        {
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
            }
            return new JsonResult { Data = new { success = false, error = System.Reflection.MethodBase.GetCurrentMethod().Name } };
        }

        [Compress]
        [HttpPost]
        public JsonResult SetMor(int val)
        {
            try
            {
                var userId = User.Identity.GetUserId();
                if (!string.IsNullOrEmpty(userId))
                {
                    var ac = new ApplicationDbContext();
                    ac.Users.First(u => u.Id == userId).MarkersOnRoads = val;
                    ac.SaveChanges();
                }

                MarkersOnRoads = val;
                return Json(new { success = true });
            }
            catch (Exception ex)
            {
            }
            return new JsonResult { Data = new { success = false, error = System.Reflection.MethodBase.GetCurrentMethod().Name } };
        }
        [Compress]
        [HttpPost]
        public JsonResult SetEnableAnimation(int val)
        {
            try
            {
                var userId = User.Identity.GetUserId();
                if (!string.IsNullOrEmpty(userId))
                {
                    var ac = new ApplicationDbContext();
                    ac.Users.First(u => u.Id == userId).EnableAnimation = val;
                    ac.SaveChanges();
                }

                EnableAnimation = val;
                return Json(new { success = true });
            }
            catch (Exception ex)
            {
            }
            return new JsonResult { Data = new { success = false, error = System.Reflection.MethodBase.GetCurrentMethod().Name } };
        }

        [Compress]
        [Authorize]
        [HttpPost]
        public JsonResult SetPassword(string password, string newPassword)
        {
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
                    return new JsonResult { Data = new { success = false, error = result.Errors.First() } };
                }
            }
            catch (Exception ex)
            {
            }
            return new JsonResult { Data = new { success = false, error = System.Reflection.MethodBase.GetCurrentMethod().Name } };
        }

        [Compress]
        [Authorize]
        [HttpPost]
        public JsonResult GetLogins()
        {
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
            }
            return new JsonResult { Data = new { success = false, error = System.Reflection.MethodBase.GetCurrentMethod().Name } };
        }

        [Compress]
        [Authorize]
        [HttpPost]
        public JsonResult RemoveLogin(string loginProvider, string providerKey)
        {
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
                return new JsonResult { Data = new { success = false, error = result.Errors.First() } };
            }
            catch (Exception ex)
            {
            }
            return new JsonResult { Data = new { success = false, error = System.Reflection.MethodBase.GetCurrentMethod().Name } };
        }

        [Compress]
        [Authorize]
        [HttpPost]
        public ActionResult AddLogin(string provider)
        {
            try
            {
                // Request a redirect to the external login provider to link a login for the current user
                return new ChallengeResult(provider, Url.Action("LinkLoginCallback", "Settings"), User.Identity.GetUserId());
            }
            catch (Exception ex)
            {
            }
            return new JsonResult { Data = new { success = false, error = System.Reflection.MethodBase.GetCurrentMethod().Name } };
        }

        private const string XsrfKey = "XsrfId";
        public async Task<ActionResult> LinkLoginCallback()
        {
            try
            {
                var loginInfo = await AuthenticationManager.GetExternalLoginInfoAsync(XsrfKey, User.Identity.GetUserId());
                if (loginInfo == null)
                {
                    return RedirectToAction("Index", "Json");
                }
                var result = await UserManager.AddLoginAsync(User.Identity.GetUserId(), loginInfo.Login);
                if (!result.Succeeded)
                {
                    return RedirectToAction("Index", "Json");
                }
            }
            catch (Exception ex)
            {
            }
            return RedirectToAction("Index", "Json");
        }
        internal class ChallengeResult : HttpUnauthorizedResult
        {
            public ChallengeResult(string provider, string redirectUri, string userId = null)
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