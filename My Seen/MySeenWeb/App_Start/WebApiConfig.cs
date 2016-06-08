using System.Web.Http;

namespace MySeenWeb
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Web API configuration and services

            // Web API routes
            config.MapHttpAttributeRoutes();
            
            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{userKey}/{mode}/{apiVersion}",
                defaults: new { user_key = RouteParameter.Optional, mode = RouteParameter.Optional, apiVersion = RouteParameter.Optional }
            );
        }
    }
}
