using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Amphiprion.Data.Entities;
using AmphiprionCMS.Areas.Administration.Models;
using AmphiprionCMS.Components.Services;

namespace AmphiprionCMS.Controllers.API
{
    public class PageHierarchyController : ApiController
    {
         private IPageService _pageService;
         public PageHierarchyController(IPageService pageService)
        {
            _pageService = pageService;
        }
        // GET api/<controller>
        public HttpResponseMessage Get()
        {
            var pages = _pageService.ListPages(null, false, true, true);
            var hierarchy = GetHierarchy(pages);
            var nodes = new PageHierarchyModel[] {hierarchy};
            return Request.CreateResponse(HttpStatusCode.OK, nodes);
        }

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
                    root = new PageHierarchyModel(p);
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
                        part = new PageHierarchyModel(p);
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
    }
}