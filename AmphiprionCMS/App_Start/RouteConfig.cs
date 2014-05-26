using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using AmphiprionCMS.Code;


namespace AmphiprionCMS
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            //routes.MapRoute(
            //    name: "Page",
            //    url: "{lang}/{*path}",
            //    defaults: new { controller = "Page", action = "Page", lang = UrlParameter.Optional }
                
            //);
            //var dataTokens = new RouteValueDictionary();
            //var ns = new string[] { "AmphiprionCMS.Controllers" };
            //dataTokens["Namespaces"] = ns;
            //routes.Add("content_page", new Route("{*path}"
            //        , new RouteValueDictionary(new { controller = "Page", action = "Page", lang = "en" })
            //        ,null
            //        ,dataTokens 
            //        , new ContentPageRouteHandler()
            //    ));


         
            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}",
                defaults:null
            );

            routes.MapRoute("content_page"
                , "{*path}"
                , new { controller = "Page", action = "Page", lang = "en",path="" }
                , new {page = new CMSIgnoreRouteContraint()}
              
                );

        }
    }
}
