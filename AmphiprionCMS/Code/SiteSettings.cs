using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Configuration;

namespace AmphiprionCMS.Web
{
    public static  class SiteSettings
    {
        public static  string DefaultLanguage
        {
            get
            {
                return WebConfigurationManager.AppSettings["defaultLanguage"];
            }
        }
    }
}