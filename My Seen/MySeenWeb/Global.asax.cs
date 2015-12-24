using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using System.Web.Helpers;
using NLog.Config;

namespace MySeenWeb
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AntiForgeryConfig.SuppressIdentityHeuristicChecks = true;
            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            // Register custom NLog Layout renderers
            ConfigurationItemFactory.Default.LayoutRenderers.RegisterDefinition("web_variables", typeof(Add_Code.Services.Logging.NLog.WebVariablesRenderer));
            ConfigurationItemFactory.Default.LayoutRenderers.RegisterDefinition("utc_date", typeof(Add_Code.Services.Logging.NLog.UtcDateRenderer));
        }
    }
}
