using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(AmphiprionCMS.Startup))]
namespace AmphiprionCMS
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
