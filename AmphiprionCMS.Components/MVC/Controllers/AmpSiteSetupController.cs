using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Amphiprion.Data;
using AmphiprionCMS.Components;
using AmphiprionCMS.Components.Authentication;
using AmphiprionCMS.Models;
using RedirectResult = System.Web.Http.Results.RedirectResult;

namespace AmphiprionCMS.Controllers
{
    [AllowAnonymous]
    public class AmpSiteSetupController : Controller
    {
        private ICMSAuthentication _auth;
        private ICMSAuthorization _authorization;
        private IInstaller _installer;
        public AmpSiteSetupController(ICMSAuthentication authentication,ICMSAuthorization authorization,IInstaller installer)
        {
            _auth = authentication;
            _authorization = authorization;
            _installer = installer;
        }
        public ActionResult Setup()
        {
            if (!HttpContext.Request.IsLocal)
                return new HttpNotFoundResult();

            if (AmphiprionCMSInitializer.Configuration.IsConfigured)
            {
                if (!_auth.IsAuthenticated)
                    return new HttpNotFoundResult();

                if (!_authorization.RequestPermission(CMSPermissions.ManageSiteSettings))
                    return new HttpUnauthorizedResult();
            }

            return View(new SetupModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Setup(SetupModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    SettingsFile.WriteToFile(this.HttpContext.Server.MapPath("~/App_Data"), "amp_settings.cfg", new { ConnectionString = model.ConnectionString, ConnectionStringName = model.ConnectionStringName,IsConfigured=true });
                    AmphiprionCMSInitializer.Configuration.RefreshFileSettings();

                    if(model.ExecuteSql)
                        _installer.Install();

                    _auth.AssignCMSAdministrator(model.Username, model.Password, model.EmailAddress);

                    return Redirect(VirtualPathUtility.ToAbsolute("~/"));
                }
                catch (Exception ex)
                {
                    
                    ModelState.AddModelError("",ex);
                }
               
            }

            return View(model);
        }

        public ActionResult DownloadSQL()
        {
            var bytes = System.Text.Encoding.UTF8.GetBytes(_installer.GetInstallScript());
            var result = new FileContentResult(bytes, "text/plain");
            result.FileDownloadName = "install.sql";
            return result;
        }
    }
}
