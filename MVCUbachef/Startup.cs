using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(MVCUbachef.Startup))]
namespace MVCUbachef
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
