using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http.Controllers;
using System.Web.Mvc;
using System.Web.Routing;


namespace AmphiprionCMS.Components.MVC
{
    public class EnsureSiteSetupAttribute:ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (!AmphiprionCMSInitializer.Configuration.IsConfigured)
            {
                if(filterContext.HttpContext.Request.IsLocal)
                    filterContext.Result = new RedirectToRouteResult("site_setup", null);
                else
                    filterContext.Result = new HttpNotFoundResult();
                
            }
                 
        }
    }
}
