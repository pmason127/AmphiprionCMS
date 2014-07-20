using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace AmphiprionCMS.Components.MVC
{
   public class PermanentRedirect:ActionResult
    {
       public string Url { get; set; }
       public PermanentRedirect(string url)
       {
           
           if (url.StartsWith("~"))
               Url = VirtualPathUtility.ToAbsolute(url);
           else
               Url = url;
       }
        public override void ExecuteResult(ControllerContext context)
        {
            context.HttpContext.Response.StatusCode = 301;
            context.HttpContext.Response.RedirectLocation = Url;
            context.HttpContext.Response.End();
        }
    }
}
