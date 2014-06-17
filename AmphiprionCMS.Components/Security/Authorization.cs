using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using AmphiprionCMS.Components.Security;
using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security;

namespace AmphiprionCMS.Components.Authentication
{
    public enum Permission
    {
        CreatePage,EditPage,DeletePage,PublishPage,ManageSettings
    }
    public interface ICMSAuthorization
    {
        bool IsAuthenticated { get; }
        bool IsAdministrator { get; }
        bool RequestPermission(Permission permission);
    }

    public class DefaultCMSAuthorization : ICMSAuthorization
    {
        private ISecurityService _security;
        public DefaultCMSAuthorization(ISecurityService security)
        {
            _security = security;
        }
        public bool IsAuthenticated
        {
            get { return _security.CurrentUser != null && _security.CurrentUser.UserName != "anonymous" ; }
        }

        public bool IsAdministrator
        {
            get
            {
                if (!IsAuthenticated)
                    return false;
                return _security.IsInRoles(_security.CurrentUser.Id, "Administrators");
            }
        }

        public bool RequestPermission(Permission permission)
        {
            if (IsAdministrator) return true;
            string[] roles;
            if (permission == Permission.PublishPage)
                roles = new[] {"Publishers"};
            else
                roles = new [] {"Editors", "Publishers"};

            return _security.IsInAnyRole(_security.CurrentUser.Id, roles);
        }
    }
}
