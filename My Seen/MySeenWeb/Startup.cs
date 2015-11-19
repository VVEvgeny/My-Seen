using Microsoft.Owin;
using MySeenWeb;
using Owin;

[assembly: OwinStartup(typeof (Startup))]

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