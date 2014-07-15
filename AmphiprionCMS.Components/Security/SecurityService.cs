using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Configuration;
using Amphiprion.Data.Entities;
using AmphiprionCMS.Components.Authentication;
using AmphiprionCMS.Components.SQL;
using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security;

namespace AmphiprionCMS.Components.Security
{
    public enum AccessType{Read,Edit,Delete,Publish}

    public class InvalidUserException : ApplicationException
    {
        public InvalidUserException():base("User not found or credentials not valid")
        {
                
        }
    }
  
    public class SecurityService : ICMSAuthentication,ICMSAuthorization
    {
        private static readonly AccessDefinition AdminDefinitionTemplate = new AccessDefinition() { CanDelete = true, CanEdit = true, CanPublish = true, CanRead = true };
        private static readonly AccessDefinition EditorDefinitionTemplate = new AccessDefinition() { CanDelete = true, CanEdit = true, CanPublish = false, CanRead = true };
        private static readonly AccessDefinition PublisherDefinitionTemplate = new AccessDefinition() { CanDelete = false, CanEdit = false, CanPublish = true, CanRead = true };
        private static readonly AccessDefinition DefaultDefinitionTemplate = new AccessDefinition() { CanDelete = false, CanEdit = false, CanPublish = false, CanRead = true };
        private UserManager<CMSUser, Guid> _userManager = null;
        private RoleManager<CMSRole, string> _roleManager = null;
        private HttpContextBase _context = null;
        
        private CMSUser anonymous;
        public SecurityService(HttpContextBase httpContext
          ,ICMSUserRepository userRepo
           )
        {
            _userManager = new UserManager<CMSUser, Guid>(new CMSUserStore(userRepo));
            _roleManager = new RoleManager<CMSRole, string>(new CMSUserStore(userRepo));
            _context = httpContext;
            anonymous = _userManager.FindByName("anonymous");
        }

        public bool IsInRoles(Guid id, params string[] rolesToCheck)
        {
            var roles = _userManager.GetRoles(id);

            return rolesToCheck.Intersect(roles).Count() == rolesToCheck.Length;

        }
        public bool IsInAnyRole(Guid id, params string[] rolesToCheck)
        {
            var roles = _userManager.GetRoles(id);

            return rolesToCheck.Intersect(roles).Any();

        }
        public void RemoveUser(Guid id)
        {
            _userManager.Delete(GetUser(id));
        }

        public CMSUser GetUser(Guid id)
        {
            return _userManager.FindById(id);
        }
        public CMSUser GetUser(string username)
        {
            return _userManager.FindByName(username);
        }
        public CMSUser GetUserByEmail(string email)
        {
            return _userManager.FindByEmail(email);
        }
        public void CreateUser(CMSUser user)
        {
            _userManager.Create(user);
        }

        public IList<CMSUser> GetUsers()
        {
            return _userManager.Users.ToList();
        } 
        public CMSUser CurrentUser
        {
            get
            {
               
                var user = Authentication.User;
                if (user == null || !user.Identity.IsAuthenticated)
                    return anonymous;

                var loggedIn = _userManager.FindById(Guid.Parse(user.Identity.GetUserId()));
                return loggedIn;
            }
        }

       
     


        public IList<string> GetRoles()
        {
            return _roleManager.Roles.Select(r => r.Id).ToList();
        }

        public bool IsAuthenticated
        {
            get
            {
                var user = Authentication.User;
                if (user == null)
                    return false;

                return user.Identity.IsAuthenticated;
            }
        }

        public void SignIn(string userName, string password, bool isPersistent)
        {
            var user =  _userManager.Find(userName, password);
            if (user == null)
                throw new InvalidUserException();

            var identity =  _userManager.CreateIdentity(user, DefaultAuthenticationTypes.ApplicationCookie);
            Authentication.SignIn(new AuthenticationProperties() { IsPersistent = isPersistent }, identity);
        }

        public void SignOut()
        {
            Authentication.SignOut();
        }

        public bool RequestPermission(Permission permission)
        {
            if (IsAdministrator) return true;
            string[] roles;
            if (permission == Permission.PublishPage)
                roles = new[] { "Publishers" };
            else
                roles = new[] { "Editors", "Publishers" };

            return IsInAnyRole(CurrentUser.Id, roles);
        }

        private IAuthenticationManager Authentication
        {
            get
            {
                return _context.GetOwinContext().Authentication;
            }
        }
        private bool IsAdministrator
        {
            get
            {
                if (!IsAuthenticated)
                    return false;
                return IsInRoles(CurrentUser.Id, "administrators");
            }
        }
    }
}
