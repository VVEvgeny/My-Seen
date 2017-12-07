using System.Web.Helpers;
using System.Web.Mvc;
using System.Web.Routing;

namespace MySeenWeb
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            //routes.MapMvcAttributeRoutes();

            routes.MapRoute(
                name: "Json/GetPage",
                url: "Json/GetPage",
                defaults: new {controller = "Json", action = "GetPage"}
                );

            routes.MapRoute(
                name: "Json/GetTranslation",
                url: "Json/GetTranslation",
                defaults: new {controller = "Json", action = "GetTranslation"}
                );
            routes.MapRoute(
                name: "Json/GetPrepared",
                url: "Json/GetPrepared",
                defaults: new {controller = "Json", action = "GetPrepared"}
                );
            routes.MapRoute(
                name: "Json/AddData",
                url: "Json/AddData",
                defaults: new {controller = "Json", action = "AddData"}
                );
            routes.MapRoute(
                name: "Json/UpdateData",
                url: "Json/UpdateData",
                defaults: new {controller = "Json", action = "UpdateData"}
                );
            routes.MapRoute(
                name: "Json/DeleteData",
                url: "Json/DeleteData",
                defaults: new {controller = "Json", action = "DeleteData"}
                );
            routes.MapRoute(
                name: "Json/AddSeries",
                url: "Json/AddSeries",
                defaults: new {controller = "Json", action = "AddSeries"}
                );
            routes.MapRoute(
                name: "Json/SkipEvent",
                url: "Json/SkipEvent",
                defaults: new { controller = "Json", action = "SkipEvent" }
            );
            routes.MapRoute(
                name: "Json/EndImprovement",
                url: "Json/EndImprovement",
                defaults: new {controller = "Json", action = "EndImprovement"}
                );
            //Shares
            routes.MapRoute(
                name: "Json/GetShare",
                url: "Json/GetShare",
                defaults: new {controller = "Json", action = "GetShare"}
                );
            routes.MapRoute(
                name: "Json/GenerateShare",
                url: "Json/GenerateShare",
                defaults: new {controller = "Json", action = "GenerateShare"}
                );
            routes.MapRoute(
                name: "Json/DeleteShare",
                url: "Json/DeleteShare",
                defaults: new {controller = "Json", action = "DeleteShare"}
                );
            routes.MapRoute(
                name: "Json/RemoveAllError",
                url: "Json/RemoveAllError",
                defaults: new { controller = "Json", action = "RemoveAllError" }
                );
            routes.MapRoute(
                name: "Json/UpdateUser",
                url: "Json/UpdateUser",
                defaults: new { controller = "Json", action = "UpdateUser" }
                );
            
            //portal
            routes.MapRoute(
                name: "Portal/RateMem",
                url: "Portal/RateMem",
                defaults: new { controller = "Portal", action = "RateMem" }
                );

            //Settings
            routes.MapRoute(
                name: "Settings/SetLanguage",
                url: "Settings/SetLanguage",
                defaults: new {controller = "Settings", action = "SetLanguage"}
                );
            routes.MapRoute(
                name: "Settings/SetTheme",
                url: "Settings/SetTheme",
                defaults: new {controller = "Settings", action = "SetTheme"}
                );
            routes.MapRoute(
                name: "Settings/SetRpp",
                url: "Settings/SetRpp",
                defaults: new {controller = "Settings", action = "SetRpp"}
                );
            routes.MapRoute(
                name: "Settings/SetMor",
                url: "Settings/SetMor",
                defaults: new {controller = "Settings", action = "SetMor"}
                );
            routes.MapRoute(
                name: "Settings/SetEnableAnimation",
                url: "Settings/SetEnableAnimation",
                defaults: new { controller = "Settings", action = "SetEnableAnimation" }
            );
            routes.MapRoute(
                name: "Settings/SetPassword",
                url: "Settings/SetPassword",
                defaults: new {controller = "Settings", action = "SetPassword"}
                );
            routes.MapRoute(
                name: "Settings/GetLogins",
                url: "Settings/GetLogins",
                defaults: new {controller = "Settings", action = "GetLogins"}
                );
            routes.MapRoute(
                name: "Settings/RemoveLogin",
                url: "Settings/RemoveLogin",
                defaults: new {controller = "Settings", action = "RemoveLogin"}
                );
            routes.MapRoute(
                name: "Settings/AddLogin",
                url: "Settings/AddLogin",
                defaults: new {controller = "Settings", action = "AddLogin"}
                );
            routes.MapRoute(
                name: "Settings/LinkLoginCallback",
                url: "Settings/LinkLoginCallback",
                defaults: new {controller = "Settings", action = "LinkLoginCallback"}
                );

            //Account
            routes.MapRoute(
                name: "Account/LoginMain",
                url: "Account/LoginMain",
                defaults: new {controller = "Account", action = "LoginMain"}
                );
            routes.MapRoute(
                name: "Account/Register",
                url: "Account/Register",
                defaults: new {controller = "Account", action = "Register"}
                );
            routes.MapRoute(
                name: "Account/ExternalLogin",
                url: "Account/ExternalLogin",
                defaults: new {controller = "Account", action = "ExternalLogin"}
                );
            routes.MapRoute(
                name: "Account/ExternalLoginCallback",
                url: "Account/ExternalLoginCallback",
                defaults: new {controller = "Account", action = "ExternalLoginCallback"}
                );
            routes.MapRoute(
                name: "Account/ExternalLoginConfirmation",
                url: "Account/ExternalLoginConfirmation",
                defaults: new {controller = "Account", action = "ExternalLoginConfirmation"}
                );
            routes.MapRoute(
                name: "Account/ExternalLoginFailure",
                url: "Account/ExternalLoginFailure",
                defaults: new {controller = "Account", action = "ExternalLoginFailure"}
                );
            routes.MapRoute(
                name: "Account/LogOut",
                url: "Account/LogOut",
                defaults: new {controller = "Account", action = "LogOut"}
                );

            //html5 
            routes.MapRoute(
                name: "Default",
                url: "{*anything}",
                defaults: new {controller = "Json", action = "Index"}
                );


            //routes.AppendTrailingSlash = true;
        }
    }
}
