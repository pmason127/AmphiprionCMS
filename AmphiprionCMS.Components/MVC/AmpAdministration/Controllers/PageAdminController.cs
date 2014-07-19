using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Amphiprion.Data.Entities;
using AmphiprionCMS.Areas.AmpAdministration.Models;
using AmphiprionCMS.Code;
using AmphiprionCMS.Components;
using AmphiprionCMS.Components.Authentication;
using AmphiprionCMS.Components.Security;
using AmphiprionCMS.Components.Services;
using AmphiprionCMS.Models;


namespace AmphiprionCMS.Areas.AmpAdministration.Controllers
{

    public class PageAdminController : Controller
    {
        private IPageService _pageService;
        private ICMSAuthorization  _cmsAuthorization;
        private IFormatting _formatter;
        public PageAdminController(IPageService pageService, ICMSAuthorization cmsAuth, IFormatting formatter)
        {
            _pageService = pageService;
            _cmsAuthorization = cmsAuth;
            _formatter = formatter;
        }
        [CMSAuthorize(CMSPermissions.ViewPageManagement)]
        public ActionResult List(Guid? parentId)
        {
            var pages = _pageService.ListPages(null,false, true, true);
            var hierarchy = new PageHierarchyModel[] { GetHierarchy(pages) };

            if (!Request.IsAjaxRequest())
                return View("List", hierarchy);
            return Json(hierarchy, JsonRequestBehavior.AllowGet);
        }
        [CMSAuthorize(CMSPermissions.ViewPageManagement)]
        public ActionResult Get(Guid id)
        {

            if (!Request.IsAjaxRequest())
                return null;
            var page = _pageService.GetPage(id);
            return Json(page, JsonRequestBehavior.AllowGet);
        }
        //
        // GET: /Administration/Page/
        [CMSAuthorize(CMSPermissions.CreatePage)]
        public ActionResult Add(Guid? parentId)
        {
           
            var canPublish = _cmsAuthorization.RequestPermission(CMSPermissions.PublishPage);

            var model = new PageCreateEditModel();
            model.ParentId = parentId ?? PageConstants.DefaultPageId;
            model.IsApproved = canPublish;
            model.PublishDateUtc = Client.LocalNow(this.HttpContext);

            return View("AddEdit",model);
        }
        public ActionResult Edit(Guid id)
        {
            if (!_cmsAuthorization.RequestPermission(CMSPermissions.EditPage))
                throw new HttpException(403, "Access Denied");

            var canPublish = _cmsAuthorization.RequestPermission(CMSPermissions.PublishPage);

            var page = _pageService.GetPage(id);
            if (page == null)
                throw new HttpException(404, "Page does not exist");

            var model = new PageCreateEditModel(page, this.HttpContext);
            model.IsApproved = canPublish;
            return View("AddEdit",model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [HandleAjaxModelErrors]
        [CMSAuthorize(CMSPermissions.EditPage)]
        public ActionResult Edit(PageCreateEditModel model)
        {
           

            var canPublish = _cmsAuthorization.RequestPermission(CMSPermissions.PublishPage);

            if (!model.Id.HasValue)
                ModelState.AddModelError("InvalidId", "The Id is not valid");

            if (ModelState.IsValid)
            {
                var slug = model.Slug;
                if (model.IsApproved)
                    model.IsApproved = canPublish;

                var p = MapPage(model);
                if(p.Slug != slug)
                     slug = _pageService.CreateAndValidateSlug(p, false);
                if (!string.IsNullOrEmpty(slug))
                {
                    _pageService.UpdatePage(p);
                    var url = Url.Content("~" + p.Path);
                    var contentModel = new ContentPage(p, url);

                    if (Request.IsAjaxRequest())
                        return Json(contentModel);
                }
                else
                {
                    ModelState.AddModelError("", "The title specified would result in a duplicate page.  Choose another parent page or modify the slug property.");
                }
               
            }


            return View(model);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [HandleAjaxModelErrors]
        [CMSAuthorize(CMSPermissions.CreatePage)]
        public ActionResult Add(PageCreateEditModel model)
        {
           
            var canPublish = _cmsAuthorization.RequestPermission(CMSPermissions.PublishPage);

            if (ModelState.IsValid)
            {
                if (model.IsApproved)
                    model.IsApproved = canPublish;

                var p = MapPage(model);
                var slug = _pageService.CreateAndValidateSlug(p, false);
                if (!string.IsNullOrEmpty(slug))
                {
                    _pageService.CreatePage(p);  
                    var url = Url.Content("~" + p.Path);
                    var contentModel = new ContentPage(p,url);
                    if (Request.IsAjaxRequest())
                        return Json(contentModel);
                }
                else
                {
                    ModelState.AddModelError("","The title specified would result in a duplicate page.  Choose another parent page or modify the slug property.");
                }
            }


            return View(model);
        }
        [HttpPost]
        [ValidateJsonAntiForgeryToken]
        [HandleAjaxModelErrors]
        [CMSAuthorize(CMSPermissions.DeletePage)]
        public ActionResult Delete(Guid id)
        {
            if (!Request.IsAjaxRequest())
                return null;

            if (!_cmsAuthorization.RequestPermission(CMSPermissions.DeletePage))
                throw new HttpException(403, "Access Denied");

            bool deleted = false;
            if(id == PageConstants.DefaultPageId)
                ModelState.AddModelError("","Cannot delete home page");
            if (ModelState.IsValid)
            {
                try
                {
                    _pageService.DeletePage(id);
                    deleted = true;
                }
                catch (Exception ex)
                {
                   ModelState.AddModelError("",ex.Message);
                }
              
            }

            return Json(new{id=id,deleted=deleted});
            
        }

        private Page MapPage(PageCreateEditModel model)
        {
            Page p;
            if (model.Id.HasValue)
                p = _pageService.GetPage(model.Id.Value);
            else
            {
                p = new Page();
                p.Id = Guid.NewGuid();
            }
            p.ParentId = model.ParentId;
            p.Title = HttpUtility.HtmlEncode(model.Title);
            p.HtmlDescription = model.HtmlDescription;
            p.CreateDateUtc = DateTime.UtcNow;
            p.IsHomePage = model.IsHomePage;
            if (model.PublishDateUtc.HasValue)
            {
                var offset = Client.GetClientTimezoneOffsetInMinutes(this.HttpContext);
                p.PublishDateUtc = model.PublishDateUtc.Value.AddMinutes(offset);
              //  p.PublishDateUtc = model.PublishDateUtc;
            }
            else
            {
                p.PublishDateUtc = DateTime.UtcNow;
            }

            p.MetaDescription = model.MetaDescription;
            p.MetaKeywords = model.MetaKeywords;
            p.IsApproved = model.IsApproved;
            p.IsActive = true;

            if (!model.Id.HasValue)
            {

                if (!string.IsNullOrEmpty(model.Slug))
                    p.Slug = model.Slug;
                else
                {
                    p.Slug = _formatter.CreateSlug(model.Title, 45);
                }
            }
            else
            {
                if (!string.IsNullOrEmpty(model.Slug))
                    p.Slug = model.Slug.Trim();
            }

            p.LastUpdateDateUtc = p.CreateDateUtc;

            return p;
        }

        #region Tree

        private PageHierarchyModel GetHierarchy(IEnumerable<Page> model)
        {
            PageHierarchyModel root = new PageHierarchyModel();

            PageHierarchyModel current = null;
            int level = 0;
            root.Level = level;
            foreach (Page p in model)
            {
                if (p.Path == null)
                {
                    root = new PageHierarchyModel(p,this.HttpContext);
                    current = root;
                    continue;
                }

                var splits = p.Path.Split(new char[] { '/' });
                current = FindFirstModelWithValue(root, splits.First());
                if (current == null)
                    current = root;
                else
                {
                    current = current.Parent;

                }


                var parts = p.Path.Split(new char[] { '/' }, StringSplitOptions.RemoveEmptyEntries);


                foreach (var str in parts)
                {
                    var part = current.Children.FirstOrDefault(s => s.Slug == str);
                    if (part == null)
                    {
                        part = new PageHierarchyModel(p,HttpContext);
                        part.Level = parts.Length;
                        current.Children.Add(part);
                    }
                    current = part;
                }



            }
            return root;
        }
        private PageHierarchyModel FindFirstModelWithValue(PageHierarchyModel root, string val)
        {
            if (root.Slug == val)
                return root;
            foreach (var child in root.Children)
            {
                var res = FindFirstModelWithValue(child, val);
                if (res != null)
                    return res;
            }
            return null;
        }
        #endregion
    }
}