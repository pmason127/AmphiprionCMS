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

            routes.MapRoute("content_page"
                , "{*path}"
                , new { controller = "AmpPage", action = "Page", lang = "en" }
                , new {page = new CMSIgnoreRouteContraint()}
                );

        }
    }
}
