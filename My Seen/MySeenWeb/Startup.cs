using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(MySeenWeb.Startup))]
namespace MySeenWeb
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
