using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(TestDropBoxApp.Startup))]
namespace TestDropBoxApp
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
