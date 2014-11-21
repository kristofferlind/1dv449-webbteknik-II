using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Chatty.Startup))]
namespace Chatty
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
            app.MapSignalR();
        }
    }
}
