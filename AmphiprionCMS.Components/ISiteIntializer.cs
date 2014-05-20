using System;
using System.Web.Security;
using Amphiprion.Data.Entities;
using AmphiprionCMS.Components.Security;
using AmphiprionCMS.Components.Services;
using Microsoft.AspNet.Identity;


namespace AmphiprionCMS.Components
{
    public interface ISiteInitializer
    {
        bool Setup();
       
    }

    public class DefaultSiteInitializer : ISiteInitializer
    {
        private IPageService _pages;
        private UserManager<CMSUser, Guid> _users;
        private RoleManager<CMSRole, string> _roles;
        private static readonly Guid AnonymousUserId = new Guid("AA852EFC-A593-423E-8F61-EFD100BB8420");
        private static readonly Guid AdminUserId = new Guid("8227E5B9-F837-4AD5-B216-1656928321F4");
        private static readonly Guid defaultPageId = new Guid("9E868D0F-A9E7-40CF-BDDC-6D354D167DA5");
        public DefaultSiteInitializer(IPageService pageService,IUserStore<CMSUser,Guid> userStore,IRoleStore<CMSRole,string> roleStore )
        {
            _pages = pageService;
            _users = new UserManager<CMSUser, Guid>(userStore);
            _roles = new RoleManager<CMSRole, string>(roleStore);
        }
        public bool Setup()
        {
           
            var admin = _users.FindByName("admin");
            if (admin == null)
            {
                admin = new CMSUser() {Id = AdminUserId, UserName = "admin", Email = "notset@localhost.com"};
                _users.Create(admin);
                _users.AddPassword(admin.Id, "p@ssw0rd");
         
            }

            if(!_users.IsInRole(AdminUserId,"administrators"))
                _users.AddToRole(AdminUserId, "administrators");
            

            var anonymous = _users.FindByName("anonymous");
            if (anonymous == null)
            {
                anonymous = new CMSUser() { Id = AnonymousUserId, UserName = "anonymous", Email = "notset@localhost.com" };
                _users.CreateAsync(anonymous);
                _users.AddPassword(anonymous.Id, "p@89535#%6=1sw8517y!k$098word");
            }

            if (!_users.IsInRole(AnonymousUserId, "everyone"))
                _users.AddToRole(AnonymousUserId, "everyone");

            var page = _pages.GetPage(defaultPageId);
            if (page == null)
            {
                page = new Page()
                {
                    Title = "Welcome to Amphiprion CMS",
                    Author = "admin",      
                    CreateDateUtc = DateTime.UtcNow,
                    LastUpdateDateUtc = DateTime.UtcNow,
                    Id = defaultPageId,
                    HtmlDescription = "<p>A Micro-CMS site based on ASP.NET MVC</p>",
                    IsActive = true,
                    IsApproved = true,
                   // IsHomePage=true
                };
                _pages.CreatePage(page);
            }

            return true;
        }

    
    }
}
