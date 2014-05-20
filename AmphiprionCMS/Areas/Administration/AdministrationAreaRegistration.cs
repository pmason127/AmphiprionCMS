using System.Web.Mvc;

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
               "Administration/page/add",
               new { controller = "Page", action = "Add"},
               new string[] { "AmphiprionCMS.Areas.Administration.Controllers" }
           );
            context.MapRoute(
              "Administration_PageEdit",
              "Administration/page/edit",
              new { controller = "Page", action = "Edit" },
              new string[] { "AmphiprionCMS.Areas.Administration.Controllers" }
          );

            context.MapRoute(
                "Administration_default",
                "Administration/{controller}/{action}/{id}",
                new { controller = "Home", action = "Index", id = UrlParameter.Optional },
                new string[] { "AmphiprionCMS.Areas.Administration.Controllers" }
            );
        }
    }
}