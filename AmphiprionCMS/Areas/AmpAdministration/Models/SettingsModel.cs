using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Policy;
using System.Web;
using System.Web.Mvc;
using Amphiprion.Data.Entities;
using FluentValidation;

namespace AmphiprionCMS.Areas.AmpAdministration.Models
{
    [FluentValidation.Attributes.Validator(typeof(SettingsModelValidator))]
    public class SettingsModel
    {
        public SettingsModel()
        {

        }
        public SettingsModel(SiteSettings internalObj)
        {
            SiteName = internalObj.SiteName;
            Description = internalObj.Description;
            SiteUrl = internalObj.SiteUrl;
            MetaKeywords = internalObj.MetaKeywords;
            RawHeader = internalObj.RawHeader;
            RawFooter = internalObj.RawFooter;
            Timezone = internalObj.Timezone;
        }
        [DisplayName("Site Name")]
        public string SiteName { get; set; }

        public string Description { get; set; }
        [DisplayName("Site Url")]
        public string SiteUrl { get; set; }
        [DisplayName("Default Keywords")]
        public string MetaKeywords { get; set; }
        [DisplayName("Raw Header")]
        public string RawHeader { get; set; }
        [DisplayName("Raw Footer")]
        public string RawFooter { get; set; }
        public string Timezone { get; set; }

        private IList<SelectListItem> _zones = null;
        public IList<SelectListItem> Timezones
        {
            get
            {
                if (_zones == null)
                {
                    var zones = System.TimeZoneInfo.GetSystemTimeZones();
                    _zones = zones.Select(z => new SelectListItem() { Text = z.DisplayName, Value = z.Id }).ToList();
                }
                return _zones;
            }
            set { }
        }
    }


    public class SettingsModelValidator : AbstractValidator<SettingsModel>
    {

        public SettingsModelValidator()
        {
            RuleFor(p => p.SiteName).NotEmpty();
            RuleFor(p => p.SiteUrl).NotEmpty();
            RuleFor(p => p.Timezone).NotEmpty();
        }
    }
}