using Microsoft.AspNet.Identity;
using Microsoft.Owin;
using Microsoft.Owin.Security.Cookies;
using Owin;

[assembly: OwinStartup("AmphiprionCMS", typeof(AmphiprionCMS.Startup))]
namespace AmphiprionCMS
{
    public  class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            var appSettings = System.Web.Configuration.WebConfigurationManager.AppSettings;
            var cookieName = appSettings["amp:auth:cookieName"] ?? "ampAuth";
            var cookieDomain = appSettings["amp:auth:cookieDomain"] ?? null;

            app.UseCookieAuthentication(new CookieAuthenticationOptions
            {
                AuthenticationType = DefaultAuthenticationTypes.ApplicationCookie,
                LoginPath = new PathString("/amp-admin/account/login"),
                CookieDomain = cookieDomain,
                CookieName = cookieName
            });
        }

    }
}
