using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Configuration;
using Amphiprion.Data.Entities;
using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security;

namespace AmphiprionCMS.Components.Security
{
    public enum AccessType{Read,Edit,Delete,Publish}

    public interface ISecurityService
    {
        bool IsInRoles(Guid id, params string[] rolesToCheck);
        bool IsInAnyRole(Guid id, params string[] rolesToCheck);
        void RemoveUser(Guid id);
        CMSUser GetUser(Guid id);
        CMSUser GetUser(string username);
        CMSUser GetUserByEmail(string email);
        void CreateUser(CMSUser user);
        CMSUser CurrentUser { get; }
        AccessDefinition GetAccessDefinitionForUser(Page p, CMSUser user);
        IList<String> GetRoles();
        IList<CMSUser> GetUsers();
    }

    public class SecurityService : ISecurityService
    {
        private static readonly AccessDefinition AdminDefinitionTemplate = new AccessDefinition() { CanDelete = true, CanEdit = true, CanPublish = true, CanRead = true };
        private static readonly AccessDefinition EditorDefinitionTemplate = new AccessDefinition() { CanDelete = true, CanEdit = true, CanPublish = false, CanRead = true };
        private static readonly AccessDefinition PublisherDefinitionTemplate = new AccessDefinition() { CanDelete = false, CanEdit = false, CanPublish = true, CanRead = true };
        private static readonly AccessDefinition DefaultDefinitionTemplate = new AccessDefinition() { CanDelete = false, CanEdit = false, CanPublish = false, CanRead = true };
        private UserManager<CMSUser, Guid> _userManager = null;
        private RoleManager<CMSRole, string> _roleManager = null;
        private HttpContextBase _context = null;
        private IAuthenticationManager _auth;
        private CMSUser anonymous;
        public SecurityService(HttpContextBase httpContext
            ,IUserStore<CMSUser,Guid> userStore
            ,IRoleStore<CMSRole,string> roleStore
            ,IAuthenticationManager authManager)
        {
                _userManager = new UserManager<CMSUser, Guid>(userStore);
            _roleManager = new RoleManager<CMSRole, string>(roleStore);
            _context = httpContext;
            _auth = authManager;
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
               
                var user = _auth.User;
                if (user == null || !user.Identity.IsAuthenticated)
                    return anonymous;

                var loggedIn = _userManager.FindById(Guid.Parse(user.Identity.GetUserId()));
                return loggedIn;
            }
        }

       
        public AccessDefinition GetAccessDefinitionForUser(Page p,CMSUser user)
        {
            var roles = _userManager.GetRoles(user.Id);
            if(roles.Any(r => r == "administrators"))
                return AdminDefinitionTemplate;

            if ((p.AccessDefinition == null || !p.AccessDefinition.Any()))
            {
                if (roles.Any(r => r == "publishers"))
                    return PublisherDefinitionTemplate;

                if (roles.Any(r => r == "editors"))
                    return EditorDefinitionTemplate;

                return DefaultDefinitionTemplate;
            }



            var emptyDef = roles.Select(r => new AccessDefinition() { RoleId = r });

            var ins = p.AccessDefinition.Intersect(emptyDef, new AccessDefinitionComparer());

            var normalizedView = new AccessDefinition() { PageId =p.Id.Value, CanDelete = false, CanEdit = false, CanPublish = false, CanRead = false };
            foreach (var ad in ins)
            {
                if (ad.CanRead)
                    normalizedView.CanRead = true;
                if (ad.CanPublish)
                    normalizedView.CanPublish = true;
                if (ad.CanEdit)
                    normalizedView.CanEdit = true;
                if (ad.CanDelete)
                    normalizedView.CanDelete = true;
            }

            return normalizedView;
        }


        public IList<string> GetRoles()
        {
            return _roleManager.Roles.Select(r => r.Id).ToList();
        }
    }
}
