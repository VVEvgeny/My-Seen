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
using MySeenWeb.Add_Code.Services.Logging.NLog;
using MySeenWeb.Controllers.Home;
using MySeenWeb.Controllers._Base;
using MySeenWeb.Models.OtherViewModels;
using MySeenWeb.Models.TablesLogic;
using MySeenWeb.Models.Tools;
using static MySeenLib.CultureInfoTool;
using static MySeenWeb.Add_Code.Md5Tools;

namespace MySeenWeb.Controllers.Account
{
    [Authorize]
    public class AccountController : BaseController
    {
        private ApplicationSignInManager _signInManager;
        private ApplicationUserManager _userManager;
        public ApplicationSignInManager SignInManager
        {
            get { return _signInManager ?? HttpContext.GetOwinContext().Get<ApplicationSignInManager>(); }
            private set { _signInManager = value; }
        }

        public ApplicationUserManager UserManager
        {
            get { return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>(); }
            private set { _userManager = value; }
        }

        public AccountController()
        {
            // for ninject
        }

        public AccountController(ApplicationUserManager userManager, ApplicationSignInManager signInManager)
        {
            UserManager = userManager;
            SignInManager = signInManager;
        }

        [Compress]
        [HttpPost]
        [AllowAnonymous]
        public async Task<JsonResult> LoginMain(string userName, string password, string remember)
        {
            var logger = new NLogLogger();
            const string methodName = "public async Task<JsonResult> LoginMain(string userName, string password, string remember)";
            try
            {
                var errorMessage = string.Empty;
                if (await SignInManager.PasswordSignInAsync(userName.ToLower(), password, bool.Parse(remember), shouldLockout: false) != SignInStatus.Success)
                {
                    errorMessage = Resource.EmailIncorrect;
                }
                else
                {
                    var logic = new UserCreditsLogic();
                    WriteUserSideStorage(UserSideStorageKeys.UserCreditsForAutologin,
                        logic.GetNew(userName, Request.UserAgent));
                }
                return !string.IsNullOrEmpty(errorMessage) ? new JsonResult { Data = new { success = false, error = errorMessage } } : Json(new { success = true });
            }
            catch (Exception ex)
            {
                logger.Error(methodName, ex);
            }
            return new JsonResult { Data = new { success = false, error = methodName } };
        }

        [Compress]
        [HttpPost]
        [AllowAnonymous]
        public async Task<JsonResult> Register(string userName, string password, string repeatPassword)
        {
            var logger = new NLogLogger();
            const string methodName = "public async Task<ActionResult> Register(string userName, string password, string repeatPassword)";
            try
            {
                if (password != repeatPassword)
                {
                    return new JsonResult { Data = new { success = false, error = "password <> repeat password" } };
                }
                var user = CreateUser(userName);
                var result = await UserManager.CreateAsync(user, password);
                if (result.Succeeded)
                {
                    await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
                    var logic = new UserCreditsLogic();
                    WriteUserSideStorage(UserSideStorageKeys.UserCreditsForAutologin,
                        logic.GetNew(userName, Request.UserAgent));
                    return new JsonResult { Data = new { success = true } };
                }
                return new JsonResult {Data = new {success = false, error = result.Errors.First()}};
            }
            catch (Exception ex)
            {
                logger.Error(methodName, ex);
            }
            return new JsonResult {Data = new {success = false, error = methodName}};
        }
        
        [Compress]
        [HttpPost]
        [AllowAnonymous]
        public ActionResult ExternalLogin(string provider, string returnUrl)
        {
            var logger = new NLogLogger();
            const string methodName = "public ActionResult ExternalLogin(string provider, string returnUrl)";

            try
            {
                // Request a redirect to the external login provider
                return new SettingsController.ChallengeResult(provider, Url.Action("ExternalLoginCallback", "Account", new { ReturnUrl = returnUrl }));
            }
            catch (Exception ex)
            {
                logger.Error(methodName, ex);
            }
            return RedirectToAction("Index", "Json");
        }

        private static ApplicationUser CreateUser(string email)
        {
            return new ApplicationUser
            {
                UserName = email,
                Email = email,
                UniqueKey = Get(email),
                ShareBooksKey = Generate(email, 1, 1),
                ShareEventsKey = Generate(email, 2, 2),
                ShareFilmsKey = Generate(email, 3, 3),
                ShareSerialsKey = Generate(email, 4, 4),
                Culture = Culture,
                RegisterDate = DateTime.Now
            };
        }

        [Compress]
        [AllowAnonymous]
        public async Task<ActionResult> ExternalLoginCallback(string returnUrl)
        {
            var logger = new NLogLogger();
            const string methodName = "public async Task<ActionResult> ExternalLoginCallback(string returnUrl)";

            try
            {
                var logic = new UserCreditsLogic();
                var loginInfo = await AuthenticationManager.GetExternalLoginInfoAsync();
                var userLogic = new UserLogic();
                if (loginInfo == null)
                {
                    return RedirectToAction("Index", "Json");
                }

                // Sign in the user with this external login provider if the user already has a login
                var result = await SignInManager.ExternalSignInAsync(loginInfo, isPersistent: false);
                switch (result)
                {
                    case SignInStatus.Success:

                        var email = loginInfo.Email;

                        if (string.IsNullOrEmpty(loginInfo.Email)) //для тех кто не отдает email
                        {
                            if (userLogic.IsExistInProvider(loginInfo.Login.LoginProvider, loginInfo.Login.ProviderKey))
                            {
                                //logger.Info("Создаю кредиты для автоавторизации");
                                email = userLogic.GetEmailByProvider(loginInfo.Login.LoginProvider, loginInfo.Login.ProviderKey);
                            }
                        }

                        if (!string.IsNullOrEmpty(email)) //Заполним если нашли чо-нить
                            WriteUserSideStorage(UserSideStorageKeys.UserCreditsForAutologin,
                                logic.GetNew(email, Request.UserAgent));

                        return RedirectToLocal(returnUrl);
                    default:

                        email = loginInfo.Email;
                        var provider = loginInfo.Login.LoginProvider;
                        //logger.Info("email=" + email + " provider=" + provider);
                        if (!string.IsNullOrEmpty(email) && !string.IsNullOrEmpty(provider))
                        {
                            if (userLogic.IsExist(loginInfo.Email) || userLogic.IsExistInProvider(provider, email))
                            {
                                //logger.Info("Создаю кредиты для авторизации");
                                if (userLogic.IsExistInProvider(provider, email))
                                    //если по провайдеру то его рейалынй эмейл будет другим
                                {
                                    email = userLogic.GetEmailByProvider(provider, email);
                                }
                                WriteUserSideStorage(UserSideStorageKeys.UserCreditsForAutologin,
                                    logic.GetNew(email, Request.UserAgent));
                                //Дальше разбереться UserCredits и авторизует его по имени
                                //Проверим, может это добавление нового сервиса
                                if (!userLogic.IsExistInProvider(provider, email))
                                {
                                    //logger.Info("Пользователь есть, добавим ему ещё авторизацию");
                                    var info = new ExternalLoginInfo
                                    {
                                        Login = new UserLoginInfo(provider, email)
                                    };
                                    var resultAddLoginAsync = UserManager.AddLogin(User.Identity.GetUserId(), info.Login);
                                    if (resultAddLoginAsync.Succeeded)
                                        return RedirectToAction("Index", "Json");
                                }
                            }
                            else //первичная регистрация, обойдусь без страницы подтверждения почты, она и так есть
                            {
                                var info = new ExternalLoginInfo {Login = new UserLoginInfo(provider, email)};
                                var user = CreateUser(email);
                                //logger.Info("Перед созданием пользователя");
                                var resultCreateAsync = await UserManager.CreateAsync(user);
                                //logger.Info("После созданием пользователя");
                                if (resultCreateAsync.Succeeded)
                                {
                                    //logger.Info("Успех созданием пользователя");
                                    resultCreateAsync = await UserManager.AddLoginAsync(user.Id, info.Login);
                                    //logger.Info("После авторизации пользователя");
                                    if (resultCreateAsync.Succeeded)
                                    {
                                        //logger.Info("Успех авторизации пользователя");
                                        await
                                            SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
                                        WriteUserSideStorage(UserSideStorageKeys.UserCreditsForAutologin,
                                            logic.GetNew(email, Request.UserAgent));
                                        return RedirectToLocal(returnUrl);
                                    }
                                }
                            }
                            return RedirectToLocal(returnUrl);
                        }
                        return View("ExternalLoginConfirmation", new ExternalLoginConfirmationViewModel { Email = loginInfo.Email });
                }
            }
            catch (Exception ex)
            {
                logger.Error(methodName, ex);
            }
            return RedirectToAction("Index", "Json");
        }

        [Compress]
        [HttpPost]
        [AllowAnonymous]
        public async Task<ActionResult> ExternalLoginConfirmation(ExternalLoginConfirmationViewModel model, string returnUrl)
        {
            var logger = new NLogLogger();
            const string methodName = "public async Task<ActionResult> ExternalLoginConfirmation(ExternalLoginConfirmationViewModel model, string returnUrl)";
            try
            {
                if (User.Identity.IsAuthenticated)
                {
                    return RedirectToAction("Index", "Json");
                }
                if (ModelState.IsValid)
                {
                    // Get the information about the user from the external login provider
                    var info = await AuthenticationManager.GetExternalLoginInfoAsync();
                    if (info == null)
                    {
                        logger.Info("ViewBag.LoginProvider =" + ViewBag.LoginProvider);
                        if (ViewBag.LoginProvider == "Yandex")
                        {
                            logger.Info("ViewBag.LoginProvider == Yandex");
                            info = new ExternalLoginInfo {Login = new UserLoginInfo("Yandex", model.Email)};
                        }
                        else
                        {
                            return View("ExternalLoginFailure");
                        }
                    }
                    var user = CreateUser(model.Email);
                    var result = await UserManager.CreateAsync(user);
                    if (result.Succeeded)
                    {
                        result = await UserManager.AddLoginAsync(user.Id, info.Login);
                        if (result.Succeeded)
                        {
                            await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
                            return RedirectToLocal(returnUrl);
                        }
                    }
                    AddErrors(result);
                }

                ViewBag.ReturnUrl = returnUrl;
                return View(model);
            }
            catch (Exception ex)
            {
                logger.Error(methodName, ex);
            }
            return RedirectToAction("Index", "Json");
        }

        [Compress]
        [AllowAnonymous]
        public ActionResult ExternalLoginFailure()
        {
            return View();
        }

        [Compress]
        [HttpPost]
        public JsonResult LogOut()
        {
            var logger = new NLogLogger();
            const string methodName = "public JsonResult LogOut()";
            try
            {
                AuthenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);

                var logic = new UserCreditsLogic();
                logic.Remove(User.Identity.GetUserId(), Request.UserAgent);
                WriteUserSideStorage(UserSideStorageKeys.UserCreditsForAutologin, string.Empty);
                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                logger.Error(methodName, ex);
            }
            return new JsonResult { Data = new { success = false, error = methodName } };
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (_userManager != null)
                {
                    _userManager.Dispose();
                    _userManager = null;
                }

                if (_signInManager != null)
                {
                    _signInManager.Dispose();
                    _signInManager = null;
                }
            }

            base.Dispose(disposing);
        }

        private IAuthenticationManager AuthenticationManager => HttpContext.GetOwinContext().Authentication;

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error);
            }
        }

        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            return RedirectToAction("Index", "Json");
        }
    }
}