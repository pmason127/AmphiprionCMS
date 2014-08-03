
using AmphiprionCMS.Components.Security;

using Microsoft.Practices.ServiceLocation;

namespace AmphiprionCMS.Components.Authentication
{

    public static class CMSPermissions
    {
        public  const  string CreatePage= "CMS.CreatePage";
        public const string EditPage = "CMS.EditPage"; 
        public const string DeletePage = "CMS.DeletePage"; 
        public const string PublishPage =  "CMS.PublishPage"; 
        public const string ViewPageManagement =  "CMS.PageManagement"; 
        public const string ManageSiteSettings = "CMS.ManageSiteSettings";
        public const string AccessAdminArea = "CMS.AccessAdministration";
    }

   
    public interface ICMSAuthorization
    {
        bool  RequestPermission(string permission);
    }

    public interface ICMSAuthentication
    {
        bool IsAuthenticated { get; }
        CMSUser CurrentUser { get; }
        void SignIn(string userName, string password, bool isPersistent);
        void SignOut();
        bool AssignCMSAdministrator(string username,string password,string emailAddress);

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
