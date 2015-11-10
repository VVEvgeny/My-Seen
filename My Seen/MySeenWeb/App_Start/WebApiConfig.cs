using System;
using System.Collections.Generic;
using System.Linq;
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
                routeTemplate: "api/{controller}/{user_key}/{mode}/{apiVersion}",
                defaults: new { user_key = RouteParameter.Optional, mode = RouteParameter.Optional, apiVersion = RouteParameter.Optional }
            );
        }
    }
}
