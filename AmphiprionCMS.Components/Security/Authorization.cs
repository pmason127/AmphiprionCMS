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

    public static class CMSPermissions
    {
        public static  string CreatePage { get { return "CMS.CreatePage"; } }
        public static string EditPage { get { return "CMS.EditPage"; } }
        public static string DeletePage { get { return "CMS.DeletePage"; } }
        public static string PublishPage { get { return "CMS.PublishPage"; } }
        public static string ManageSiteSettings { get { return "CMS.ManageSiteSettings"; } }
    }
    public static class CMSSections
    {
        public static string PageManagement { get { return "CMSSection.PageManagement"; } }
        public static string ManageSettings { get { return "CMSSection.SiteSettings"; } }
      
    }
    public interface ICMSAuthorization
    {
        bool RequestPermission(string permission);
        bool CanAccessSection(string section);

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
