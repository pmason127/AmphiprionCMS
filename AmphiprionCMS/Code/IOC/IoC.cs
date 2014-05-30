

using System;
using System.Web;

using System.Web.Http;
using System.Web.Mvc;
using Amphiprion.Data;
using AmphiprionCMS.Components;
using AmphiprionCMS.Components.Search;
using AmphiprionCMS.Components.Security;
using AmphiprionCMS.Components.Services;
using AmphiprionCMS.Components.SQL;
using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security;
using Microsoft.Practices.ServiceLocation;

using StructureMap;

using StructureMap.Graph;


namespace AmphiprionCMS.DependencyInjection {
    public static class StructuremapMvc
    {
        public static void Start()
        {
            IContainer container = IoC.Initialize();
            var resolver = new StructureMapDependencyResolver(container);
            DependencyResolver.SetResolver(resolver);
            GlobalConfiguration.Configuration.DependencyResolver = resolver;
            ServiceLocator.SetLocatorProvider(() => resolver);
        }
        public static void DestroyScope()
        {
            ObjectFactory.ReleaseAndDisposeAllHttpScopedObjects();
        }
    }

    public static class IoC {
        public static IContainer Initialize() {
            ObjectFactory.Initialize(x =>
                        {

                            x.Scan(cfg =>
                            {
                                cfg.AssemblyContainingType(typeof (IDocumentMapper<>));
                                cfg.ConnectImplementationsToTypesClosing(typeof(IDocumentMapper<>));
                            });

                            x.For<HttpContextBase>().HybridHttpOrThreadLocalScoped().Use(()=> new HttpContextWrapper(HttpContext.Current));
                            x.For<ISiteInitializer>().Use<DefaultSiteInitializer>();
                            x.For<IUserStore<CMSUser,Guid>>().Singleton().Use<CMSUserStore>();
                            x.Forward<IUserStore<CMSUser, Guid>,IRoleStore<CMSRole,string>>();
                            x.For<ICMSUserRepository>().Singleton().Use<CMSUserRepository>();
                            x.For<IConnectionManager>().Use<MSSQLConnectionManager>();
                            x.For<IPageRepository>().Singleton().Use<PageRepository>();
                            x.For<IPageService>().Singleton().Use<PageService>();
                            x.For<IFormatting>().Singleton().Use<Formatting>();
                            x.For<IImagingService>().Singleton().Use<ImagingService>();
                            x.For<ISecurityService>().Use<SecurityService>();
                            x.For<IAuthenticationManager>().Use((c) =>
                            {
                               return c.GetInstance<HttpContextBase>().GetOwinContext().Authentication;
                            });
                            x.For(typeof (ISearchIndexProvider<>)).Use(typeof (SearchIndexProvider<>));
                            x.For<ISettingsRepository>().Singleton().Use<SettingsRepository>();
                        });


            return ObjectFactory.Container;
        }

    }
    //internal class EventModuleConvention : IRegistrationConvention
    //{


    //    public void Process(System.Type type, StructureMap.Configuration.DSL.Registry registry)
    //    {
    //        if (!type.IsAbstract && typeof(AdvancedFlightTracker.Api.v1.IEventModule).IsAssignableFrom(type))
    //        {
    //            registry.For(typeof(AdvancedFlightTracker.Api.v1.IEventModule)).Singleton().Use(type);
    //        }
    //    }
    //}
}