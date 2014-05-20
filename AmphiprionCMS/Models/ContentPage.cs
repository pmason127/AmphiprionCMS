using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Amphiprion.Data.Entities;

namespace AmphiprionCMS.Models
{
    public class ContentPage
    {
        private Page _page;
        public ContentPage(string language,Page page)
        {
            _page = page;
            Language = language;
        }

        public Page Page
        {
            get { return _page; }
        }
        public string Language { get; private  set; }
        public bool IsRenderingInPreviewMode { get; set; }
    }
}