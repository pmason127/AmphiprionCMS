using System.Web.Mvc;

namespace AmphiprionCMS.Areas.AmpAdministration
{
    public class AmpAdministrationAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "AmpAdministration";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
             "Administration_menus",
             "amp-admin/menus",
             new { controller = "AmpMenu", action = "Index" }
             , new string[] { "AmphiprionCMS.Areas.AmpAdministration.Controllers" }
         );
            context.MapRoute(
              "Administration_settings",
              "amp-admin/settings",
              new { controller = "AmpSettings", action = "Settings" }
              , new string[] { "AmphiprionCMS.Areas.AmpAdministration.Controllers" }
          );
            context.MapRoute(
               "Administration_PageAdd",
               "amp-admin/page/add",
               new { controller = "PageAdmin", action = "Add"}
               , new string[] { "AmphiprionCMS.Areas.AmpAdministration.Controllers" }
           );
            context.MapRoute(
              "Administration_PageEdit",
              "amp-admin/page/edit/{id}",
              new { controller = "PageAdmin", action = "Edit" }
              , new string[] { "AmphiprionCMS.Areas.AmpAdministration.Controllers" }
          );
            context.MapRoute(
              "Administration_PageDelete",
              "amp-admin/page/delete",
              new { controller = "PageAdmin", action = "Delete" }
              , new string[] { "AmphiprionCMS.Areas.AmpAdministration.Controllers" }
          );
            context.MapRoute(
             "Administration_PageList",
             "amp-admin/page/list",
             new { controller = "PageAdmin", action = "List" }
             , new string[] { "AmphiprionCMS.Areas.AmpAdministration.Controllers" }
         );
            context.MapRoute(
                "Administration_default",
                "amp-admin/{controller}/{action}/{id}",
                new { controller = "Home", action = "Index", id = UrlParameter.Optional }
                , new string[] { "AmphiprionCMS.Areas.AmpAdministration.Controllers" }
            );
        }
    }
}