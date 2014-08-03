using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Configuration;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using AmphiprionCMS.Code;
using AmphiprionCMS.Components.Authentication;
using AmphiprionCMS.Components.Security;
using AmphiprionCMS.Components.SQL;
using FluentValidation.Mvc;
using Microsoft.Ajax.Utilities;
using Microsoft.Owin.Security;
using Microsoft.Practices.ServiceLocation;


namespace AmphiprionCMS.Components
{
    public interface IConfigurationSettings
    {
         ICMSAuthorization Authorizer { get; }
         ICMSAuthentication AuthenticationManager { get; }
         string ConnectionString { get; }
         string ConnectionStringName { get; }
         bool IsConfigured { get; }
        void RefreshFileSettings();
    }

    public interface IConfigurationSetupExpression
    {
        void SetAuthorization(Func<HttpContextBase, ICMSAuthorization> auth);
        void SetAuthentication(Func<HttpContextBase, ICMSAuthentication> manager);
    }
   
    internal class AmphiprionCMSConfiguration:IConfigurationSettings,IConfigurationSetupExpression
    {
        private Func<HttpContextBase, ICMSAuthorization> _authorizer = null;
        private Func<HttpContextBase, ICMSAuthentication> _authentication = null;
        private HttpContextBase _httpConext = null;
        private static dynamic _settings;
        internal  AmphiprionCMSConfiguration(HttpContextBase httpContext)
        {
            _httpConext = httpContext;
            RefreshFileSettingsInternal();
        }
        void IConfigurationSetupExpression.SetAuthorization(Func<HttpContextBase, ICMSAuthorization> auth)
        {
            _authorizer = auth;
        }

        void IConfigurationSetupExpression.SetAuthentication(Func<HttpContextBase, ICMSAuthentication> manager)
        {
            _authentication = manager;
        }
       
        ICMSAuthorization IConfigurationSettings.Authorizer
        {
            get
            {
                if (_authorizer == null)
                {
                    _authorizer = (c) => new SecurityService(c, ServiceLocator.Current.GetInstance<ICMSUserRepository>());
                }
                return _authorizer(ServiceLocator.Current.GetInstance<HttpContextBase>());
            }
        }

        ICMSAuthentication IConfigurationSettings.AuthenticationManager
        {
            get
            {
                if (_authentication == null)
                {
                    _authentication = (c) => new SecurityService(c,ServiceLocator.Current.GetInstance<ICMSUserRepository>());
                }
                return _authentication((ServiceLocator.Current.GetInstance<HttpContextBase>()));
            }

        }

        string IConfigurationSettings.ConnectionString
        {
            get
            {
                return ConnectionStringInternal;
            }
        }
        string IConfigurationSettings.ConnectionStringName
        {
            get
            {
                return ConnectionStringNameInternal;
            }
        }

        private string ConnectionStringNameInternal
        {
            get
            {
                if (_settings == null)
                    return null;

                return _settings.ConnectionStringName;
            }
        }
        private string ConnectionStringInternal
        {
            get
            {
                if (_settings == null)
                    return null;

                return _settings.ConnectionString;
            }
        }
        bool IConfigurationSettings.IsConfigured
        {
            get
            {
                if (_settings == null)
                    return false;
                if (_settings.IsConfigured == null)
                    return false;

                return _settings.IsConfigured.Value && (!string.IsNullOrEmpty(ConnectionStringInternal) || !string.IsNullOrEmpty(ConnectionStringNameInternal));

            }
        }

        private void RefreshFileSettingsInternal()
        {
            _settings = SettingsFile.ReadFile(_httpConext.Server.MapPath("~/App_Data"), "amp_settings.cfg");
        }
        void IConfigurationSettings.RefreshFileSettings()
        {
            RefreshFileSettingsInternal();
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
            routes.MapRoute("site_setup"
               , "amphiprioncms/setup"
               , new { controller = "AmpSiteSetup", action = "Setup", lang = "en" }
               ,null
               , new string[] { "AmphiprionCMS.Controllers" }
               );
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
