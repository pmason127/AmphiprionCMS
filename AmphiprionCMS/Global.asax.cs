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
using AmphiprionCMS.Components.IOC;
using AmphiprionCMS.Components.Search;
using AmphiprionCMS.Views;
using FluentValidation.Mvc;
using Microsoft.Practices.ServiceLocation;
using Newtonsoft.Json.Converters;
using StructureMap;


namespace AmphiprionCMS
{
    public class MvcApplication : System.Web.HttpApplication
    {
        
        protected void Application_Start()
        {
            //StructuremapMvc.Start();
            AreaRegistration.RegisterAllAreas();
        
           // ViewEngines.Engines.Clear();
          //  ViewEngines.Engines.Add(new ThemeRazorViewEngine());

            GlobalConfiguration.Configure(WebApiConfig.Register);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            //Default CMS initializer
            AmphiprionCMS.Components.AmphiprionCMSInitializer.Initialize(new HttpContextWrapper(HttpContext.Current));

            //Use Structure Map
            StructureMapServiceRegistry registry = null;
            //Do whatever you want with structuremap
            ObjectFactory.Initialize(r =>
            {
               //Your bindings here
               
            });
            //Setup CMS to use structuremap
            registry = new StructureMapServiceRegistry(ObjectFactory.Container,AmphiprionCMS.Components.AmphiprionCMSInitializer.Configuration);
            var provider = registry.GetService(); //Gets the implemented ServiceLocator, if you use your own do it here
            ServiceLocator.SetLocatorProvider(() => provider);//set service locator
            DependencyResolver.SetResolver(provider); //set dependency resolver

        }

    }
}
