using System;
using System.Threading.Tasks;
using System.Web.Mvc;
using AmphiprionCMS.Areas.AmpAdministration.Models;
using AmphiprionCMS.Code;
using AmphiprionCMS.Components.Authentication;


namespace AmphiprionCMS.Areas.AmpAdministration.Controllers
{
   [CMSAuthorize]
    public class AccountController : Controller
    {
        private ICMSAuthentication  _authenticationManager;
        public AccountController(ICMSAuthentication authenticationManager)
        {
            _authenticationManager = authenticationManager;
         
        }
        [AllowAnonymous]
        public ActionResult Login()
        {
            return View();
        }
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Login(LoginViewModel model, string returnUrl)
        {
            if (ModelState.IsValid)
            {
               try
                {
                    await SignInAsync(model.UserName, model.Password, model.RememberMe);
                    return RedirectToLocal(returnUrl);
                }
                catch (Exception)
                {

                    ModelState.AddModelError("", "Invalid username or password.");
                }
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LogOff()
        {
            _authenticationManager.SignOut();
            return RedirectToAction("Login", "Account", new { area = "AmpAdministration" });
        }
        private async Task SignInAsync(string username,string password,bool isPersistent)
        {
            _authenticationManager.SignIn(username, password, isPersistent);
        }
        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            else
            {
                return RedirectToAction("Index","Home",new {area="AmpAdministration"});
            }
        }
	}
}