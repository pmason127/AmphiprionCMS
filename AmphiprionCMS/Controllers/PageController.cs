using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Amphiprion.Data.Entities;
using AmphiprionCMS.Components.Security;
using AmphiprionCMS.Components.Services;
using AmphiprionCMS.Models;


namespace AmphiprionCMS.Controllers
{
    public class PageController : Controller
    {
        private IPageService _pageService;
        private ISecurityService _securityService;
        public PageController(IPageService pageService,ISecurityService securityService)
        {
            _pageService = pageService;
            _securityService = securityService;
        }
        //
        // GET: /Page/
        [AllowAnonymous]
        public ActionResult Page(string lang,string path)
        {
            if (string.IsNullOrEmpty(lang))
            {
                if (this.HttpContext.Request.UserLanguages != null && this.HttpContext.Request.UserLanguages.Any())
                {
                   lang = this.HttpContext.Request.UserLanguages[0];
                }
            }
            Amphiprion.Data.Entities.Page page = null;
            if (string.IsNullOrEmpty(path))
            {
                page = _pageService.GetHomePage();
            }
            else
            {
                 page = _pageService.GetPage(path);
            }

            if (page == null 
                     || !page.IsActive
                     || !page.IsApproved
                     || (page.PublishDateUtc.HasValue && page.PublishDateUtc.Value > DateTime.UtcNow))
            {
                throw new HttpException(404,"Page not found");
            }
            
            var cp = new ContentPage(lang, page);
          //  cp.IsRenderingInPreviewMode = (!permissions.CanEdit || !permissions.CanPublish);
            return View(cp);

        }

      
    }
}