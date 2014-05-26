using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Amphiprion.Data.Entities;
using AmphiprionCMS.Areas.Administration.Models;
using AmphiprionCMS.Code;
using AmphiprionCMS.Components;
using AmphiprionCMS.Components.Security;
using AmphiprionCMS.Components.Services;
using AmphiprionCMS.Models;


namespace AmphiprionCMS.Areas.Administration.Controllers
{
    public class PageAdminController : Controller
    {
        private IPageService _pageService;
        private ISecurityService _securityService;
        private IFormatting _formatter;
        public PageAdminController(IPageService pageService, ISecurityService securityService, IFormatting formatter)
        {
            _pageService = pageService;
            _securityService = securityService;
            _formatter = formatter;
        }
        public ActionResult List(Guid? parentId)
        {



            var pages = _pageService.ListPages(null, false, true, true);
            var hierarchy = new PageHierarchyModel[] { GetHierarchy(pages) };

            if (!Request.IsAjaxRequest())
                return View("List", hierarchy);
            return Json(hierarchy, JsonRequestBehavior.AllowGet);
        }
        public ActionResult Get(Guid id)
        {

            if (!Request.IsAjaxRequest())
                return null;
            var page = _pageService.GetPage(id);
            return Json(page, JsonRequestBehavior.AllowGet);
        }
        //
        // GET: /Administration/Page/
        public ActionResult Add(Guid? parentId)
        {
            var user = _securityService.CurrentUser;
            if (!_securityService.IsInAnyRole(user.Id, "editors", "administrators"))
                throw new HttpException(403, "Access Denied");

            var model = new PageCreateEditModel();
            model.ParentId = parentId ?? PageConstants.DefaultPageId;
            model.PublishDateUtc = Client.LocalNow(this.HttpContext);

            return View(model);
        }
        public ActionResult Edit(Guid id)
        {
            var page = _pageService.GetPage(id);
            if (page == null)
                throw new HttpException(404, "Page does not exist");

            var model = new PageCreateEditModel(page, this.HttpContext);
            //TODO: Security check
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(PageCreateEditModel model)
        {
            if (!model.Id.HasValue)
                ModelState.AddModelError("InvalidId", "The Id is not valid");

            if (ModelState.IsValid)
            {

                var p = MapPage(model);
              

                _pageService.UpdatePage(p);
                var url = Url.Content("~" + p.Path);
                var contentModel = new ContentPage(p, url);

                if (Request.IsAjaxRequest())
                    return Json(contentModel);
            }


            return View(model);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [HandleAjaxModelErrors]
        public ActionResult Add(PageCreateEditModel model)
        {
            if (ModelState.IsValid)
            {
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