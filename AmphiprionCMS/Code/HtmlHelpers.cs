using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Amphiprion.Data.Entities;

namespace AmphiprionCMS
{
    public static  class HtmlHelpers
    {
        public static IHtmlString RawHeader(this HtmlHelper helper)
        {
            var str = AmphiprionCMS.Components.SiteSettings.Current.RawHeader ?? "";
            return helper.Raw(str);
        }
        public static IHtmlString RawFooter(this HtmlHelper helper)
        {
            var str = AmphiprionCMS.Components.SiteSettings.Current.RawFooter ?? "";
            return helper.Raw(str);
        }
      
    }
}