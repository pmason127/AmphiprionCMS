using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using AmphiprionCMS.Areas.AmpAdministration.Models;
using AmphiprionCMS.Components.Security;
using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security;

namespace AmphiprionCMS.Areas.AmpAdministration.Controllers
{
   
    public class AccountController : Controller
    {
        private UserManager<CMSUser,Guid> _userManager;
        private IAuthenticationManager _authenticationManager;
        public AccountController(IAuthenticationManager authenticationManager, IUserStore<CMSUser,Guid> userStore )
        {
            _authenticationManager = authenticationManager;
            _userManager = new UserManager<CMSUser, Guid>(userStore);
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
                var user = await _userManager.FindAsync(model.UserName, model.Password);
                if (user != null)
                {
                    await SignInAsync(user, model.RememberMe);
                    return RedirectToLocal(returnUrl);
                }
                else
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
            return RedirectToAction("Login", "Account", new { area = "Administration" });
        }
        private async Task SignInAsync(CMSUser  user, bool isPersistent)
        {
            var identity = await _userManager.CreateIdentityAsync(user, DefaultAuthenticationTypes.ApplicationCookie);
            _authenticationManager.SignIn(new AuthenticationProperties() { IsPersistent = isPersistent }, identity);
        }
        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            else
            {
                return RedirectToAction("Index","Home",new {area="Administration"});
            }
        }
	}
}