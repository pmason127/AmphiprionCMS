using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AmphiprionCMS.Code;

namespace AmphiprionCMS.Areas.AmpAdministration.Controllers
{
     [CMSAuthorize]
    public class HomeController : Controller
    {
        //
        // GET: /Administration/Home/
        public ActionResult Index()
        {
            return View();
        }
	}
}