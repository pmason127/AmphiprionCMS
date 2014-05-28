using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AmphiprionCMS.Models;

namespace AmphiprionCMS.Controllers
{
    public class ErrorController : Controller
    {
        //
        // GET: /Error/
        public ActionResult Error404()
        {
            HttpContext.Response.StatusCode = 404;
            return View(new Error());
        }
        public ActionResult Error(Error error)
        {
            return View(new Error());
        }

       
	}
}