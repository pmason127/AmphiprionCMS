using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using System.Web.Mvc;

namespace AmphiprionCMS.Views
{
    public class ThemeRazorViewEngine : RazorViewEngine
    {
        public string _themeName = null;
        public ThemeRazorViewEngine()
        {
            _themeName = WebConfigurationManager.AppSettings["defaultThemeName"];

            ViewLocationFormats = new[]
            {
                "~/themes/"+_themeName +"/Views/{1}/{0}.cshtml",
                "~/themes/"+_themeName +"/Views/{1}/{0}.vbhtml",
                "~/themes/"+_themeName +"/Views/{0}.cshtml",
                "~/themes/"+_themeName +"/Views/{0}.vbhtml",
                "~/Views/{1}/{0}.cshtml",
                "~/Views/{1}/{0}.vbhtml",
                "~/Views/Shared/{0}.cshtml",
                "~/Views/Shared/{0}.vbhtml"
            };

            MasterLocationFormats = ViewLocationFormats;
            PartialViewLocationFormats = ViewLocationFormats;
        }
    }
}