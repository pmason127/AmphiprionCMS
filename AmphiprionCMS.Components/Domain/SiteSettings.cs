using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;

namespace Amphiprion.Data.Entities
{
    public class SiteSettings
    {
        public int Id { get; set; }
        public string SiteName { get; set; }
        public string Description { get; set; }
        public string SiteUrl { get; set; }
        public string MetaKeywords { get; set; }
        public string RawHeader { get; set; }
        public string RawFooter { get; set; }
        public string Timezone { get; set; }

        public TimeZoneInfo TimeZoneInfo
        {
            get
            {
                if (string.IsNullOrEmpty(Timezone))
                    return null;

                return TimeZoneInfo.FindSystemTimeZoneById(Timezone);
            }
        }
       
    }
}
