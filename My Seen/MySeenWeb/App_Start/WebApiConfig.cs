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
                //routeTemplate: "api/{controller}/{apiVersion}/{act}/{userKey}",
                routeTemplate: "api/{controller}/{action}/{apiVersion}/{userKey}",
                defaults: new { apiVersion = RouteParameter.Optional, userKey = RouteParameter.Optional }
            );
        }
    }
}
