using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using AmphiprionCMS.Components.Security;
using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security;
using Microsoft.Practices.ServiceLocation;

namespace AmphiprionCMS.Components.Authentication
{

    public enum Permission
    {
        CreatePage,EditPage,DeletePage,PublishPage,ManageSettings
    }
    public interface ICMSAuthorization
    {
        bool RequestPermission(Permission permission);
    }

    public interface ICMSAuthentication
    {
        bool IsAuthenticated { get; }
        CMSUser CurrentUser { get; }
        void SignIn(string userName, string password, bool isPersistent);
        void SignOut();
    }

   

    public static class Authorization
    {

        public static ICMSAuthorization Current
        {
            get { return  ServiceLocator.Current.GetInstance<ICMSAuthorization>(); }
        }

    }

    public static class Authentication
    {

        public static ICMSAuthentication Current
        {
            get { return ServiceLocator.Current.GetInstance<ICMSAuthentication>(); }
        }

    }
}
