﻿using System.Web.Mvc;

namespace AmphiprionCMS.Areas.Administration
{
    public class AdministrationAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "Administration";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {

            context.MapRoute(
               "Administration_PageAdd",
               "amp-admin/page/add",
               new { controller = "PageAdmin", action = "Add"},
               new string[] { "AmphiprionCMS.Areas.Administration.Controllers" }
           );
            context.MapRoute(
              "Administration_PageEdit",
              "amp-admin/page/edit/{id}",
              new { controller = "PageAdmin", action = "Edit" }
          );
            context.MapRoute(
             "Administration_PageList",
             "amp-admin/page/list",
             new { controller = "PageAdmin", action = "List" }
         );
            context.MapRoute(
                "Administration_default",
                "amp-admin/{controller}/{action}/{id}",
                new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}