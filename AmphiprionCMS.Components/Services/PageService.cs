using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Amphiprion.Data;
using Amphiprion.Data.Entities;


namespace AmphiprionCMS.Components.Services
{
    public interface IPageService
    {
        Page GetPage(string path);
        Page GetPage(Guid id);
        Page GetHomePage();
        void CreatePage(Page page);
        void DeletePage(Guid id);
        void UpdatePage(Page page);
        IList<Page> ListPages(Guid? parentId = null,bool includeParentInResults = false,bool incudeUnpublished = false, bool includeInactive = false);
        string CreateAndValidateSlug(Page page,bool autoCorrect = false);
    }
   public class PageService:IPageService
   {
       private IPageRepository _repo;
       private IFormatting _formatter;
       public PageService(IPageRepository repository,IFormatting formatter )
       {
           _repo = repository;
           _formatter = formatter;
       }
       public Page GetPage(string path)
        {
            var page = _repo.Get(path);
       
           return page;
        }
   
        public void CreatePage(Page page)
        {
            if (!page.Id.HasValue)
                page.Id = Guid.NewGuid();

            ValidatePageAndSetDefaults(page);

            page.LastUpdateDateUtc = page.CreateDateUtc;

           _repo.Create(page);
            _repo.SetAccessDefinition(page.Id.Value,page.AccessDefinition);
        }


        public Page GetHomePage()
        {
         
            return GetPage(PageConstants.DefaultPageId);
        }


        public Page GetPage(Guid id)
        {
            var page = _repo.Get(id);
            return page;
        }


        public void DeletePage(Guid id)
        {
            var page = _repo.Get(id);
            if (page == null)
                return;

            if (page.Id == PageConstants.DefaultPageId)
                throw new InvalidOperationException("Cannot delete homepage, promote another page to be homepage first");
            _repo.Delete(id);
        }



       public void UpdatePage(Page page)
       {
           if (!page.Id.HasValue)
               throw new ArgumentException("Id is required", "Id");

           var exPage = GetPage(page.Id.Value);
           if(exPage == null)
               throw new ApplicationException("Invalid page for edit");

          

           ValidatePageAndSetDefaults(page);

           page.LastUpdateDateUtc = DateTime.UtcNow;

           _repo.Update(page);

       }

       public string CreateAndValidateSlug(Page page,bool autoCorrect = false)
       {

           Func<string, string, string> buildPath = (slug, parentPath) =>
           {
               return string.Concat(parentPath ?? "","/",slug);
           };

           var pageSlug = page.Slug;
           if (string.IsNullOrEmpty(pageSlug))
               pageSlug = _formatter.CreateSlug(page.Title, 45);

           Page parent = null;
           if(page.ParentId.HasValue)
               parent = _repo.Get(page.ParentId.Value);
           var path = string.Empty;
           if (parent != null)
               path = parent.Path;

          var pagePath = buildPath(pageSlug,path);

           Page chkPage = GetPage(pagePath);
           if (chkPage != null)
           {
               if (!autoCorrect)
               {
                   return null;
               }
                int i = 0;
               var slug =pageSlug;
               var newslug = slug;
               var newPath =pagePath;
               while (chkPage != null)
               {
                   i++;
                   newslug = slug + i;
                   newPath = buildPath(newslug, path);
                   chkPage = GetPage(newPath);
               }
               pageSlug  = newslug;
           }
           return pageSlug;
       }
    
       private void ValidatePageAndSetDefaults(Page page)
       {
           if (string.IsNullOrWhiteSpace(page.Title))
               throw new ArgumentException("Title is required", "Title");

           if (string.IsNullOrWhiteSpace(page.HtmlDescription))
               throw new ArgumentException("HtmlDescription is required", "HtmlDescription");

           var now = DateTime.UtcNow;
           if (!page.CreateDateUtc.HasValue)
               page.CreateDateUtc = now;

           //if (page.IsApproved && !page.PublishDateUtc.HasValue)
           //    page.PublishDateUtc = now;

           //if (page.PublishDateUtc.HasValue && !page.IsApproved)
           //    page.PublishDateUtc = null;

           if (string.IsNullOrEmpty(page.Slug))
           {
               page.Slug = CreateAndValidateSlug(page, true);
           }
       }


       public IList<Page> ListPages(Guid? parentId = null, bool includeParentInResults = false, bool incudeUnpublished = false, bool includeInactive = false)
       {
           return _repo.List(parentId,includeParentInResults,incudeUnpublished, includeInactive);
       }
   }
}
