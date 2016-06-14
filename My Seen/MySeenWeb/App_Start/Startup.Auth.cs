using System;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security.Google;
using MySeenLib;
using MySeenWeb.Add_Code;
using Owin;
using MySeenWeb.Models.OtherViewModels;
using Owin.Security.Providers.Dropbox;
using Owin.Security.Providers.GitHub;
using Owin.Security.Providers.LinkedIn;
using Owin.Security.Providers.Steam;
using Owin.Security.Providers.VKontakte;
using Owin.Security.Providers.Yahoo;

namespace MySeenWeb
{
    public partial class Startup
    {
        // For more information on configuring authentication, please visit http://go.microsoft.com/fwlink/?LinkId=301864
        public void ConfigureAuth(IAppBuilder app)
        {
            // Configure the db context, user manager and signin manager to use a single instance per request
            app.CreatePerOwinContext(ApplicationDbContext.Create);
            app.CreatePerOwinContext<ApplicationUserManager>(ApplicationUserManager.Create);
            app.CreatePerOwinContext<ApplicationSignInManager>(ApplicationSignInManager.Create);

            // Enable the application to use a cookie to store information for the signed in user
            // and to use a cookie to temporarily store information about a user logging in with a third party login provider
            // Configure the sign in cookie
            app.UseCookieAuthentication(new CookieAuthenticationOptions
            {
                AuthenticationType = DefaultAuthenticationTypes.ApplicationCookie,
                LoginPath = new PathString("/Account/Login"),
                Provider = new CookieAuthenticationProvider
                {
                    // Enables the application to validate the security stamp when the user logs in.
                    // This is a security feature which is used when you change a password or add an external login to your account.  
                    OnValidateIdentity = SecurityStampValidator.OnValidateIdentity<ApplicationUserManager, ApplicationUser>(
                        //validateInterval: TimeSpan.FromMinutes(30),
                        validateInterval: TimeSpan.FromDays(7),
                        regenerateIdentity: (manager, user) => user.GenerateUserIdentityAsync(manager))
                }
            });
            app.UseExternalSignInCookie(DefaultAuthenticationTypes.ExternalCookie);

            // Enables the application to temporarily store user information when they are verifying the second factor in the two-factor authentication process.
            //app.UseTwoFactorSignInCookie(DefaultAuthenticationTypes.TwoFactorCookie, TimeSpan.FromMinutes(5));
            app.UseTwoFactorSignInCookie(DefaultAuthenticationTypes.TwoFactorCookie, TimeSpan.FromDays(7));

            // Enables the application to remember the second login verification factor such as phone or email.
            // Once you check this option, your second step of verification during the login process will be remembered on the device where you logged in from.
            // This is similar to the RememberMe option when you log in.
            app.UseTwoFactorRememberBrowserCookie(DefaultAuthenticationTypes.TwoFactorRememberBrowserCookie);

            var mySeenAuths = new Auths("MySeen", AppDomain.CurrentDomain.BaseDirectory + "bin");

            //Dropbox
            if (Admin.IsDebug && mySeenAuths.Have("Dropbox"))
            {
                //не поддерживает HTTP (только для локального), только HTTPS
                app.UseDropboxAuthentication(mySeenAuths.Id("Dropbox"), mySeenAuths.Secret("Dropbox"));
            }
            //Twitter
            if (mySeenAuths.Have("Twitter"))
            {
                app.UseTwitterAuthentication(mySeenAuths.Id("Twitter"), mySeenAuths.Secret("Twitter"));
            }
            //Yahoo
            if (mySeenAuths.Have("Yahoo"))
            {
                app.UseYahooAuthentication(mySeenAuths.Id("Yahoo"), mySeenAuths.Secret("Yahoo"));
            }
            //Steam
            if (mySeenAuths.Have("Steam"))
            {
                app.UseSteamAuthentication(mySeenAuths.Id("Steam"));
            }
            //GitHub
            if (mySeenAuths.Have("GitHub"))
            {
                app.UseGitHubAuthentication(mySeenAuths.Id("GitHub"), mySeenAuths.Secret("GitHub"));
            }
            //LinkedIn
            if (mySeenAuths.Have("LinkedIn"))
            {
                app.UseLinkedInAuthentication(mySeenAuths.Id("LinkedIn"), mySeenAuths.Secret("LinkedIn"));
            }
            //Microsoft
            if (mySeenAuths.Have("Microsoft"))
            {
                app.UseMicrosoftAccountAuthentication(mySeenAuths.Id("Microsoft"), mySeenAuths.Secret("Microsoft"));
            }
            //Facebook
            if (mySeenAuths.Have("Facebook"))
            {
                app.UseFacebookAuthentication(mySeenAuths.Id("Facebook"), mySeenAuths.Secret("Facebook"));
            }
            //Vkontakte
            if (mySeenAuths.Have("Vkontakte"))
            {
                app.UseVKontakteAuthentication(new VKontakteAuthenticationOptions
                {
                    ClientId = mySeenAuths.Id("Vkontakte"),
                    ClientSecret = mySeenAuths.Secret("Vkontakte")
                });
                //Ушел от duke  app.UseVkontakteAuthentication(mySeenAuths.Id("Vkontakte"), mySeenAuths.Secret("Vkontakte"), mySeenAuths.Options("Vkontakte"));
            }
            //Google
            if (mySeenAuths.Have("Google"))
            {
                app.UseGoogleAuthentication(new GoogleOAuth2AuthenticationOptions() { ClientId = mySeenAuths.Id("Google"), ClientSecret = mySeenAuths.Secret("Google") });
            }
        }
    }
}