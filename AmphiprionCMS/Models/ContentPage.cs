using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Web;
using System.Web.Routing;
using Amphiprion.Data.Entities;

namespace AmphiprionCMS.Models
{
    public class ContentPage
    {
        private Page _page;

        public ContentPage(Page page,string url):this(null,page,url)
        {
                
        }
        public ContentPage(string language,Page page,string url)
        {
            _page = page;
            Language = language;
            Url = url;
        }

        public Page Page
        {
            get { return _page; }
        }
        public string Language { get; private  set; }
        public bool IsRenderingInPreviewMode { get; set; }

        public string Url
        { 
            get; set;
        }
    }
}