using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Amphiprion.Data;
using AmphiprionCMS.Components.Authentication;
using AmphiprionCMS.Components.Search;
using AmphiprionCMS.Components.Security;
using AmphiprionCMS.Components.Services;
using AmphiprionCMS.Components.SQL;
using AmphiprionCMS.DependencyInjection;
using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security;
using Microsoft.Practices.ServiceLocation;
using StructureMap;


namespace AmphiprionCMS.Components.IOC
{
    public class StructureMapServiceRegistry
    {
        private IContainer _container;
        private static IServiceLocator _service = null;
        public IServiceLocator GetService()
        {
            return _service;
        }
        public StructureMapServiceRegistry(IContainer container,IConfigurationSettings settings)
        {
            _container = container;
           _container.Configure(x => {
                x.Scan(cfg =>
                            {
                                cfg.AssemblyContainingType(typeof (IDocumentMapper<>));
                                cfg.ConnectImplementationsToTypesClosing(typeof(IDocumentMapper<>));
                            });

                            x.For<HttpContextBase>().HybridHttpOrThreadLocalScoped().Use(()=> new HttpContextWrapper(HttpContext.Current));
                            x.For<ISiteInitializer>().Use<DefaultSiteInitializer>();
                           
                            x.For<ICMSUserRepository>().Singleton().Use<CMSUserRepository>();
                            x.For<IConnectionManager>().Use<MSSQLConnectionManager>();
                            x.For<IPageRepository>().Singleton().Use<PageRepository>();
                            x.For<IPageService>().Singleton().Use<PageService>();
                            x.For<IFormatting>().Singleton().Use<Formatting>();
                            x.For<IImagingService>().Singleton().Use<ImagingService>();

                            x.For<ICMSAuthentication>().HybridHttpOrThreadLocalScoped().Use((c) => settings.AuthenticationManager );
                            x.For(typeof (ISearchIndexProvider<>)).Use(typeof (SearchIndexProvider<>));
                            x.For<ISettingsRepository>().Singleton().Use<SettingsRepository>();
                            x.For<ICMSAuthorization>().HybridHttpOrThreadLocalScoped().Use((c) => settings.Authorizer);
                           
                        });

           _service = new StructureMapDependencyScope(container);
        }
    }
}
