using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Amphiprion.Data.Entities;
using AmphiprionCMS.Code;
using AmphiprionCMS.Components.Services;
using FluentValidation;

namespace AmphiprionCMS.Areas.AmpAdministration.Models
{
    [FluentValidation.Attributes.Validator(typeof(PageModelValidator))]
    public class PageCreateEditModel
    {
        private IPageService  pageService = DependencyResolver.Current.GetService<IPageService>();
        public PageCreateEditModel()
        {
            //AccessDefinition = new List<AccessDefinitionModel>();
        }

        public PageCreateEditModel(Page page, HttpContextBase context)
        {
            this.Id = page.Id;
            this.HtmlDescription = page.HtmlDescription;
            this.Title = page.Title;
            this.IsApproved = page.IsApproved;
            this.MetaDescription = page.MetaDescription;
            this.MetaKeywords = page.MetaKeywords;
            if (page.PublishDateUtc.HasValue)
                this.PublishDateUtc = page.PublishDateUtc.Value.AddMinutes(-1 * Client.GetClientTimezoneOffsetInMinutes(context));

            this.Slug = page.Slug;
            this.ParentId = page.ParentId;

        }
        public virtual Guid? Id { get; set; }
        public virtual Guid? ParentId { get; set; }

        [DataType(DataType.Html)]
        [AllowHtml]
        public virtual string HtmlDescription { get; set; }
        [DataType(DataType.Text)]
        public virtual string Title { get; set; }

        [DataType(DataType.Text)]
        [DisplayName("Keywords")]
        public virtual string MetaKeywords { get; set; }
        [DataType(DataType.MultilineText)]
        [DisplayName("Meta Description")]
        public virtual string MetaDescription { get; set; }
        [DataType(DataType.DateTime)]
        [DisplayName("Publish On")]
        public virtual DateTime? PublishDateUtc { get; set; }
        [DataType(DataType.Text)]
        public virtual string Author { get; set; }


        [DefaultValue(false)]
        [DisplayName("Publish")]
        public virtual bool IsApproved { get; set; }
        [DataType(DataType.Text)]
        public virtual string Slug { get; set; }

        private IList<SelectListItem> _pages = null;
        public IList<SelectListItem> Pages
        {
            get
            {
                if (_pages == null)
                {
                    var pages = pageService.ListPages(null, false, true, false);
                    _pages = pages.Select(p => new SelectListItem() { Text = p.Title, Value = p.Id.ToString() }).ToList();
                }
                return _pages;
            }
        }

        //  public List<AccessDefinitionModel> AccessDefinition { get; set; } 
    }

    public class PageModelValidator : AbstractValidator<PageCreateEditModel>
    {

        public PageModelValidator()
        {
            RuleFor(p => p.Title).NotEmpty();
            RuleFor(p => p.HtmlDescription).NotEmpty();
        }
    }



    public class PageHierarchyModel
    {
        public PageHierarchyModel()
        {
            Children = new List<PageHierarchyModel>();
            Level = 0;
        }

        public PageHierarchyModel(Page page,HttpContextBase context)
            : this()
        {
            this.Id = page.Id.Value;
            Title = page.Title;
            Path = page.Path;
            Slug = page.Slug;
            IsApproved = page.IsApproved;
            IsActive = page.IsActive;
            this.PublishDateUtc = page.PublishDateUtc;

        }
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Path { get; set; }
        public string Slug { get; set; }
        public List<PageHierarchyModel> Children { get; set; }
        public PageHierarchyModel Parent { get; set; }
        public int Level { get; set; }
        public bool IsApproved { get; set; }
        public DateTime? PublishDateUtc { get; set; }
        public bool IsActive { get; set; }
        public bool IsHomepage { get { return this.Id == PageConstants.DefaultPageId; }}
    }
}