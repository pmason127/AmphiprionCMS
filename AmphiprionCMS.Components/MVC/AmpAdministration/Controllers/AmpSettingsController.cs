using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Configuration;
using System.Web;
using System.Web.Mvc;
using AmphiprionCMS.Areas.AmpAdministration.Models;
using AmphiprionCMS.Code;
using AmphiprionCMS.Components;
using AmphiprionCMS.Components.Authentication;


namespace AmphiprionCMS.Areas.AmpAdministration.Controllers
{
     [CMSAuthorize]
    public class AmpSettingsController : Controller
     {
         private ICMSAuthorization _cmsAuthorization = null;
         public AmpSettingsController(ICMSAuthorization auth)
         {
             _cmsAuthorization = auth;
         }
        [CMSAuthorize(CMSPermissions.ManageSiteSettings)]
        public ActionResult Settings()
        {
            if (!_cmsAuthorization.RequestPermission(CMSPermissions.ManageSiteSettings))
                throw new HttpException(403, "Access Denied");

            var settings = SiteSettings.Current;
            return View(new SettingsModel(settings));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [HandleAjaxModelErrors]
        [CMSAuthorize(CMSPermissions.ManageSiteSettings)]
        public ActionResult Settings(SettingsModel model)
        {
            if (ModelState.IsValid)
            {
                SiteSettings.Current.SiteName = model.SiteName;
                SiteSettings.Current.SiteUrl = model.SiteUrl;
                SiteSettings.Current.Description = model.Description;
                SiteSettings.Current.RawFooter = model.RawFooter;
                SiteSettings.Current.RawHeader = model.RawHeader;
                SiteSettings.Current.MetaKeywords = model.MetaKeywords;
                SiteSettings.Current.Timezone = model.Timezone;
                SiteSettings.Save();
            }
            var newModel = new SettingsModel(SiteSettings.Current);
            if (Request.IsAjaxRequest())
                return Json(newModel);

            return View(newModel);
        }
	}
}