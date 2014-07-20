using System.Linq;
using System.Web;
using System.Web.Mvc;
using AmphiprionCMS.Components.MVC;
using AmphiprionCMS.Components.Security;
using AmphiprionCMS.Components.Services;
using AmphiprionCMS.Models;


namespace AmphiprionCMS.Controllers
{
    public class AmpPageController : Controller
    {
        private IPageService _pageService;
        public AmpPageController(IPageService pageService)
        {
            _pageService = pageService;
          
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
                 page = _pageService.GetPage("/" + path);
                if (page.IsHomePage)
                    return new PermanentRedirect("~/");
            }

            if (page == null  || !page.IsPagePublished)
            {
               return new HttpNotFoundResult();
            }
            var url =   Url.Content("~" + page.Path);;
            var cp = new ContentPage(lang, page,url);
          //  cp.IsRenderingInPreviewMode = (!permissions.CanEdit || !permissions.CanPublish);
            return View(cp);

        }

      
    }
}