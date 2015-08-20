using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(aps.Startup))]
namespace aps
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
