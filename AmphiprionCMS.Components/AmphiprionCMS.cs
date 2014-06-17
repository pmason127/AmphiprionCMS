using System;
using System.Configuration;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using AmphiprionCMS.Code;
using AmphiprionCMS.Components.Authentication;
using FluentValidation.Mvc;
using Microsoft.Owin.Security;


namespace AmphiprionCMS.Components
{
    public interface IConfigurationSettings
    {
        ICMSAuthorization Authorizer { get; }
        Func<HttpContextBase,IAuthenticationManager> AuthenticationManager { get; }
    }

    public interface IConfigurationSetupExpression
    {
        void SetAuthorization(ICMSAuthorization authorizer);
        void SetAuthentication(Func<HttpContextBase, IAuthenticationManager> manager);
    }
   
    internal class AmphiprionCMSConfiguration:IConfigurationSettings,IConfigurationSetupExpression
    {
        private ICMSAuthorization _authorizer = null;
        private Func<HttpContextBase, IAuthenticationManager> _authentication = null;
        private HttpContextBase _httpConext = null;
        internal  AmphiprionCMSConfiguration(HttpContextBase httpContext)
        {
            _httpConext = httpContext;
        }
        void IConfigurationSetupExpression.SetAuthorization(ICMSAuthorization authorizer)
        {
            _authorizer = authorizer;
        }

        void IConfigurationSetupExpression.SetAuthentication(Func<HttpContextBase, IAuthenticationManager> manager)
        {
            _authentication = manager;
        }

        ICMSAuthorization IConfigurationSettings.Authorizer
        {
            get
            {
                return _authorizer;
            }
        }

        Func<HttpContextBase,IAuthenticationManager> IConfigurationSettings.AuthenticationManager
        {
            get
            {
                if(_authentication != null)
                   return _authentication;

                return (c) =>
                {
                    return c.GetOwinContext().Authentication;
                };
            }

        }
    }
    public static class AmphiprionCMSInitializer
    {
        private static  IConfigurationSetupExpression _options = null;
        private static readonly object _locker = new object();
       
        public static void Initialize(HttpContextBase httpContextBase,Action<IConfigurationSetupExpression,HttpContextBase> setup = null)
        {

            lock (_locker)
            {
                _options = new AmphiprionCMSConfiguration(httpContextBase);
                if(setup != null)
                    setup(_options,httpContextBase);

                Initialize();
            }

        }
        private static void Initialize()
        {
         //   StructuremapMvc.Start();
            RegisterRoutes(RouteTable.Routes);
            RegisterGlobalFilters(GlobalFilters.Filters);
            RegisterBundles(BundleTable.Bundles);

            FluentValidationModelValidatorProvider.Configure();

            ModelBinders.Binders[typeof(DateTime?)] = new DateTimeModelBinder();

            //var siteInitializer = DependencyResolver.Current.GetService<ISiteInitializer>();
            //if (siteInitializer != null)
            //    siteInitializer.Setup();
        }

        private static void RegisterBundles(BundleCollection bundles )
        {
               bundles.Add(new ScriptBundle("~/bundles/amphiprion/bootstrap").Include(
                      "~/AmphiprionCMS/Scripts/bootstrap.js",
                      "~/AmphiprionCMS/Scripts/respond.js"));

            bundles.Add(new ScriptBundle("~/bundles/amphiprion/application").Include(
                     "~/AmphiprionCMS/Scripts/application.js"));

            bundles.Add(new StyleBundle("~/Content/amphiprion/css").Include(
                      "~/AmphiprionCMS/Content/bootstrap.css",
                      "~/AmphiprionCMS/Content/site.css"));


            bundles.Add(new StyleBundle("~/Content/amphiprion/admin/css").Include(
                      "~/AmphiprionCMS/Content/admin.css", "~/AmphiprionCMS/Content/iconmoon/style.css"));



            bundles.Add(new ScriptBundle("~/bundles/amphiprion/editor").Include("~/AmphiprionCMS/Scripts/ckeditor/ckeditor.js"));
            bundles.Add(new ScriptBundle("~/bundles/amphiprion/adapter").Include(
                     "~/AmphiprionCMS/Scripts/ckeditor/adapters/jquery.js"));
        }
        private static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {

        }
        private static void RegisterRoutes(RouteCollection routes)
        {

            routes.MapRoute("content_page"
                , "{*path}"
                , new { controller = "AmpPage", action = "Page", lang = "en" }
                , new { page = new CMSIgnoreRouteContraint() }
                , new string[] { "AmphiprionCMS.Controllers" }
                );

        }

        public static IConfigurationSettings Configuration
        {
            get
            {
                return (IConfigurationSettings)_options;
            }
        }
    }
}
