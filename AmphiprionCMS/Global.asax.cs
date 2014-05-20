using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using Amphiprion.Data.Entities;
using AmphiprionCMS.Code;
using AmphiprionCMS.Components;
using AmphiprionCMS.Components.Search;
using AmphiprionCMS.DependencyInjection;
using AmphiprionCMS.Views;
using FluentValidation.Mvc;
using Newtonsoft.Json.Converters;


namespace AmphiprionCMS
{
    public class MvcApplication : System.Web.HttpApplication
    {
        
        protected void Application_Start()
        {
            StructuremapMvc.Start();
            AreaRegistration.RegisterAllAreas();
        
           // ViewEngines.Engines.Clear();
          //  ViewEngines.Engines.Add(new ThemeRazorViewEngine());

            GlobalConfiguration.Configure(WebApiConfig.Register);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            FluentValidationModelValidatorProvider.Configure();

            ModelBinders.Binders[typeof(DateTime?)] =
   new DateTimeModelBinder();

            //var siteInitializer = DependencyResolver.Current.GetService<ISiteInitializer>();
            //if (siteInitializer != null)
            //    siteInitializer.Setup();

            var t = DependencyResolver.Current.GetService<ISearchIndexProvider<Page>>();

        }

    }
}
