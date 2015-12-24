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
using MySeenWeb.Models;
using MySeenWeb.Models.OtherViewModels;
using MySeenWeb.Models.Tools;
using Nemiro.OAuth;

namespace MySeenWeb.Controllers
{
    [BrowserActionFilter]
    [Authorize]
    public class ManageController : BaseController
    {
        private ApplicationSignInManager _signInManager;
        private ApplicationUserManager _userManager;

        public ManageController()
        {
        }

        public ManageController(ApplicationUserManager userManager, ApplicationSignInManager signInManager)
        {
            UserManager = userManager;
            SignInManager = signInManager;
        }

        public ApplicationSignInManager SignInManager
        {
            get
            {
                return _signInManager ?? HttpContext.GetOwinContext().Get<ApplicationSignInManager>();
            }
            private set
            {
                _signInManager = value;
            }
        }

        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }

        //
        // GET: /Manage/Index
        public async Task<ActionResult> Index(ManageMessageId? message)
        {
            var logger = new NLogLogger();
            const string methodName = "public async Task<ActionResult> Index(ManageMessageId? message)";
            try
            {
                ViewBag.StatusMessage =
                message == ManageMessageId.ChangePasswordSuccess ? "Your password has been changed."
                : message == ManageMessageId.SetPasswordSuccess ? "Your password has been set."
                : message == ManageMessageId.SetTwoFactorSuccess ? "Your two-factor authentication provider has been set."
                : message == ManageMessageId.Error ? "An error has occurred."
                : message == ManageMessageId.AddPhoneSuccess ? "Your phone number was added."
                : message == ManageMessageId.RemovePhoneSuccess ? "Your phone number was removed."
                : "";

                var userId = User.Identity.GetUserId();
                var model = new IndexViewModel
                {
                    HasPassword = HasPassword(),
                    PhoneNumber = await UserManager.GetPhoneNumberAsync(userId),
                    TwoFactor = await UserManager.GetTwoFactorEnabledAsync(userId),
                    Logins = await UserManager.GetLoginsAsync(userId),
                    BrowserRemembered = await AuthenticationManager.TwoFactorBrowserRememberedAsync(userId)
                };

                if (User.Identity.IsAuthenticated)
                {
                    var ac = new ApplicationDbContext();
                    var user = ac.Users.First(u => u.Id == userId);
                    model.Lang = Defaults.Languages.GetIdDb(user.Culture);
                    model.Rpp = user.RecordPerPage;
                    model.Markers = user.MarkersOnRoads;
                    model.VkServiceEnabled = user.VkServiceEnabled;
                    model.GoogleServiceEnabled = user.GoogleServiceEnabled;
                    model.FacebookServiceEnabled = user.FacebookServiceEnabled;

                    model.LoadSelectList();

                    model.HaveData = (ac.Films.Any(f => f.UserId == userId)
                        || ac.Serials.Any(f => f.UserId == userId)
                        || ac.Books.Any(f => f.UserId == userId)
                        || ac.Tracks.Any(f => f.UserId == userId)
                        );
                }
                return View(model);
            }
            catch (Exception ex)
            {
                logger.Error(methodName, ex);
            }
            return RedirectToAction("Index","Home");
        }

        //
        // POST: /Manage/RemoveLogin
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> RemoveLogin(string loginProvider, string providerKey)
        {
            var logger = new NLogLogger();
            const string methodName = "public async Task<ActionResult> Index(ManageMessageId? message)";
            ManageMessageId? message = ManageMessageId.Error;
            try
            {
                var result = await UserManager.RemoveLoginAsync(User.Identity.GetUserId(), new UserLoginInfo(loginProvider, providerKey));
                if (result.Succeeded)
                {
                    var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());
                    if (user != null)
                    {
                        await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
                    }
                    message = ManageMessageId.RemoveLoginSuccess;
                }
            }
            catch (Exception ex)
            {
                logger.Error(methodName, ex);
            }
            return RedirectToAction("ManageLogins", new { Message = message });
        }
        [HttpPost]
        public JsonResult ChangeLanguage(string selected)
        {
            var logger = new NLogLogger();
            const string methodName = "public JsonResult ChangeLanguage(string selected)";
            try
            {
                var ac = new ApplicationDbContext();
                var userId = User.Identity.GetUserId();
                ac.Users.First(u => u.Id == userId).Culture = Defaults.Languages.GetValDb(Convert.ToInt32(selected));
                ac.SaveChanges();
                CultureInfoTool.SetCulture(Defaults.Languages.GetValDb(Convert.ToInt32(selected)));
                WriteUserSideStorage(UserSideStorageKeys.Language, selected);
                Defaults.ReloadResources();
                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                logger.Error(methodName, ex);
            }
            return new JsonResult { Data = new { success = false, error = methodName } };
        }
        [HttpPost]
        public JsonResult ChangeRpp(string selected)
        {
            var logger = new NLogLogger();
            const string methodName = "public JsonResult ChangeRpp(string selected)";
            try
            {
                var ac = new ApplicationDbContext();
                var userId = User.Identity.GetUserId();
                ac.Users.First(u => u.Id == userId).RecordPerPage = Convert.ToInt32(selected);
                ac.SaveChanges();
                Rpp = Convert.ToInt32(selected);
                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                logger.Error(methodName, ex);
            }
            return new JsonResult { Data = new { success = false, error = methodName } };
        }
        [HttpPost]
        public JsonResult ChangeMoR(string selected)
        {
            var logger = new NLogLogger();
            const string methodName = "public JsonResult ChangeMoR(string selected)";
            try
            {
                var ac = new ApplicationDbContext();
                var userId = User.Identity.GetUserId();
                ac.Users.First(u => u.Id == userId).MarkersOnRoads = Convert.ToInt32(selected);
                ac.SaveChanges();
                MarkersOnRoads = Convert.ToInt32(selected);
                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                logger.Error(methodName, ex);
            }
            return new JsonResult { Data = new { success = false, error = methodName } };
        }
        [HttpPost]
        public JsonResult ChangeVk(string selected)
        {
            var logger = new NLogLogger();
            const string methodName = "public JsonResult ChangeVk(string selected)";
            try
            {
                var ac = new ApplicationDbContext();
                var userId = User.Identity.GetUserId();
                ac.Users.First(u => u.Id == userId).VkServiceEnabled = Convert.ToInt32(selected) == Defaults.EnabledDisabledBase.Indexes.Enabled;
                ac.SaveChanges();
                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                logger.Error(methodName, ex);
            }
            return new JsonResult { Data = new { success = false, error = methodName } };
        }
        [HttpPost]
        public JsonResult ChangeGoogle(string selected)
        {
            var logger = new NLogLogger();
            const string methodName = "public JsonResult ChangeGoogle(string selected)";
            try
            {
                var ac = new ApplicationDbContext();
                var userId = User.Identity.GetUserId();
                ac.Users.First(u => u.Id == userId).GoogleServiceEnabled = Convert.ToInt32(selected) == Defaults.EnabledDisabledBase.Indexes.Enabled;
                ac.SaveChanges();
                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                logger.Error(methodName, ex);
            }
            return new JsonResult { Data = new { success = false, error = methodName } };
        }
        [HttpPost]
        public JsonResult ChangeFacebook(string selected)
        {
            var logger = new NLogLogger();
            const string methodName = "public JsonResult ChangeFacebook(string selected)";
            try
            {
                var ac = new ApplicationDbContext();
                var userId = User.Identity.GetUserId();
                ac.Users.First(u => u.Id == userId).FacebookServiceEnabled = Convert.ToInt32(selected) == Defaults.EnabledDisabledBase.Indexes.Enabled;
                ac.SaveChanges();
                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                logger.Error(methodName, ex);
            }
            return new JsonResult { Data = new { success = false, error = methodName } };
        }

        [HttpPost]
        public JsonResult DeleteData()
        {
            var logger = new NLogLogger();
            const string methodName = "public JsonResult DeleteData()";
            try
            {
                var ac = new ApplicationDbContext();
                var userId = User.Identity.GetUserId();
                ac.Films.RemoveRange(ac.Films.Where(f => f.UserId == userId));
                ac.Serials.RemoveRange(ac.Serials.Where(f => f.UserId == userId));
                ac.Books.RemoveRange(ac.Books.Where(f => f.UserId == userId));
                ac.Tracks.RemoveRange(ac.Tracks.Where(f => f.UserId == userId));
                ac.SaveChanges();
                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                logger.Error(methodName, ex);
            }
            return new JsonResult { Data = new { success = false, error = methodName } };
        }
        //
        // GET: /Manage/AddPhoneNumber
        public ActionResult AddPhoneNumber()
        {
            return View();
        }

        //
        // POST: /Manage/AddPhoneNumber
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> AddPhoneNumber(AddPhoneNumberViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            // Generate the token and send it
            var code = await UserManager.GenerateChangePhoneNumberTokenAsync(User.Identity.GetUserId(), model.Number);
            if (UserManager.SmsService != null)
            {
                var message = new IdentityMessage
                {
                    Destination = model.Number,
                    Body = "Your security code is: " + code
                };
                await UserManager.SmsService.SendAsync(message);
            }
            return RedirectToAction("VerifyPhoneNumber", new { PhoneNumber = model.Number });
        }

        //
        // POST: /Manage/EnableTwoFactorAuthentication
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> EnableTwoFactorAuthentication()
        {
            await UserManager.SetTwoFactorEnabledAsync(User.Identity.GetUserId(), true);
            var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());
            if (user != null)
            {
                await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
            }
            return RedirectToAction("Index", "Manage");
        }

        //
        // POST: /Manage/DisableTwoFactorAuthentication
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DisableTwoFactorAuthentication()
        {
            await UserManager.SetTwoFactorEnabledAsync(User.Identity.GetUserId(), false);
            var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());
            if (user != null)
            {
                await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
            }
            return RedirectToAction("Index", "Manage");
        }

        //
        // GET: /Manage/VerifyPhoneNumber
        public async Task<ActionResult> VerifyPhoneNumber(string phoneNumber)
        {
            //var code = 
                await UserManager.GenerateChangePhoneNumberTokenAsync(User.Identity.GetUserId(), phoneNumber);
            // Send an SMS through the SMS provider to verify the phone number
            return phoneNumber == null ? View("Error") : View(new VerifyPhoneNumberViewModel { PhoneNumber = phoneNumber });
        }

        //
        // POST: /Manage/VerifyPhoneNumber
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> VerifyPhoneNumber(VerifyPhoneNumberViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var result = await UserManager.ChangePhoneNumberAsync(User.Identity.GetUserId(), model.PhoneNumber, model.Code);
            if (result.Succeeded)
            {
                var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());
                if (user != null)
                {
                    await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
                }
                return RedirectToAction("Index", new { Message = ManageMessageId.AddPhoneSuccess });
            }
            // If we got this far, something failed, redisplay form
            ModelState.AddModelError("", "Failed to verify phone");
            return View(model);
        }

        //
        // GET: /Manage/RemovePhoneNumber
        public async Task<ActionResult> RemovePhoneNumber()
        {
            var result = await UserManager.SetPhoneNumberAsync(User.Identity.GetUserId(), null);
            if (!result.Succeeded)
            {
                return RedirectToAction("Index", new { Message = ManageMessageId.Error });
            }
            var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());
            if (user != null)
            {
                await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
            }
            return RedirectToAction("Index", new { Message = ManageMessageId.RemovePhoneSuccess });
        }

        //
        // GET: /Manage/ChangePassword
        public ActionResult ChangePassword()
        {
            return View();
        }

        //
        // POST: /Manage/ChangePassword
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ChangePassword(ChangePasswordViewModel model)
        {
            var logger = new NLogLogger();
            const string methodName = "public async Task<ActionResult> ChangePassword(ChangePasswordViewModel model)";
            try
            {
                if (!ModelState.IsValid)
                {
                    return View(model);
                }
                var result = await UserManager.ChangePasswordAsync(User.Identity.GetUserId(), model.OldPassword, model.NewPassword);
                if (result.Succeeded)
                {
                    var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());
                    if (user != null)
                    {
                        await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
                    }
                    return RedirectToAction("Index", new { Message = ManageMessageId.ChangePasswordSuccess });
                }
                AddErrors(result);
                return View(model);
            }
            catch (Exception ex)
            {
                logger.Error(methodName, ex);
            }
            return RedirectToAction("Index","Home");
        }

        //
        // GET: /Manage/SetPassword
        public ActionResult SetPassword()
        {
            return View();
        }

        //
        // POST: /Manage/SetPassword
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> SetPassword(SetPasswordViewModel model)
        {
            var logger = new NLogLogger();
            const string methodName = "public async Task<ActionResult> SetPassword(SetPasswordViewModel model)";
            try
            {
                if (ModelState.IsValid)
                {
                    var result = await UserManager.AddPasswordAsync(User.Identity.GetUserId(), model.NewPassword);
                    if (result.Succeeded)
                    {
                        var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());
                        if (user != null)
                        {
                            await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
                        }
                        return RedirectToAction("Index", new { Message = ManageMessageId.SetPasswordSuccess });
                    }
                    AddErrors(result);
                }

                // If we got this far, something failed, redisplay form
                return View(model);
            }
            catch (Exception ex)
            {
                logger.Error(methodName, ex);
            }
            return RedirectToAction("Index", "Home");
        }

        //
        // GET: /Manage/ManageLogins
        public async Task<ActionResult> ManageLogins(ManageMessageId? message)
        {
            var logger = new NLogLogger();
            const string methodName = "public async Task<ActionResult> ManageLogins(ManageMessageId? message)";
            try
            {
                ViewBag.StatusMessage =
                    message == ManageMessageId.RemoveLoginSuccess
                        ? Resource.TheExternalLoginWasRemoved
                        : message == ManageMessageId.Error
                            ? Resource.AnErrorHasOccurred
                            : "";
                var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());
                if (user == null)
                {
                    return View("Error");
                }
                var userLogins = await UserManager.GetLoginsAsync(User.Identity.GetUserId());
                var otherLogins =
                    AuthenticationManager.GetExternalAuthenticationTypes()
                        .Where(auth => userLogins.All(ul => auth.AuthenticationType != ul.LoginProvider))
                        .ToList();
                ViewBag.ShowRemoveButton = user.PasswordHash != null || userLogins.Count > 1;
                return View(new ManageLoginsViewModel
                {
                    CurrentLogins = userLogins,
                    OtherLogins = otherLogins
                });
            }
            catch (Exception ex)
            {
                logger.Error(methodName, ex);
            }
            return RedirectToAction("Index", "Home");
        }

        //
        // POST: /Manage/LinkLogin
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LinkLogin(string provider)
        {
            var logger = new NLogLogger();
            const string methodName = "public ActionResult LinkLogin(string provider)";
            try
            {
                if (provider == ExternalNotOwinProviders.Yandex)
                {
                    //logger.Info("provider YANDEX");
                    var returnUrl = MySeenWebApi.ApiHost + "/Account/ExternalLoginCallback";
                    return Redirect(OAuthWeb.GetAuthorizationUrl(provider, returnUrl));
                }
                // Request a redirect to the external login provider to link a login for the current user
                return new AccountController.ChallengeResult(provider, Url.Action("LinkLoginCallback", "Manage"), User.Identity.GetUserId());
            }
            catch (Exception ex)
            {
                logger.Error(methodName, ex);
            }
            return RedirectToAction("Index", "Home");
        }

        //
        // GET: /Manage/LinkLoginCallback
        public async Task<ActionResult> LinkLoginCallback()
        {
            var logger = new NLogLogger();
            const string methodName = "public ActionResult LinkLogin(string provider)";
            try
            {
                var loginInfo = await AuthenticationManager.GetExternalLoginInfoAsync(XsrfKey, User.Identity.GetUserId());
                if (loginInfo == null)
                {
                    return RedirectToAction("ManageLogins", new { Message = ManageMessageId.Error });
                }
                var result = await UserManager.AddLoginAsync(User.Identity.GetUserId(), loginInfo.Login);
                return result.Succeeded ? RedirectToAction("ManageLogins") : RedirectToAction("ManageLogins", new { Message = ManageMessageId.Error });
            }
            catch (Exception ex)
            {
                logger.Error(methodName, ex);
            }
            return RedirectToAction("Index", "Home");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && _userManager != null)
            {
                _userManager.Dispose();
                _userManager = null;
            }

            base.Dispose(disposing);
        }

        #region Helpers
        // Used for XSRF protection when adding external logins
        private const string XsrfKey = "XsrfId";

        private IAuthenticationManager AuthenticationManager
        {
            get
            {
                return HttpContext.GetOwinContext().Authentication;
            }
        }

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error);
            }
        }

        private bool HasPassword()
        {
            var user = UserManager.FindById(User.Identity.GetUserId());
            if (user != null)
            {
                return user.PasswordHash != null;
            }
            return false;
        }

/*
        private bool HasPhoneNumber()
        {
            var user = UserManager.FindById(User.Identity.GetUserId());
            if (user != null)
            {
                return user.PhoneNumber != null;
            }
            return false;
        }
*/

        public enum ManageMessageId
        {
            AddPhoneSuccess,
            ChangePasswordSuccess,
            SetTwoFactorSuccess,
            SetPasswordSuccess,
            RemoveLoginSuccess,
            RemovePhoneSuccess,
            Error
        }

        #endregion
    }
}