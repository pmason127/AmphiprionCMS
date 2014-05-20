using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using AmphiprionCMS.Components.Services;

namespace AmphiprionCMS.Controllers.API
{
    public class PageController : ApiController
    {
        private IPageService _pageService;
        public PageController(IPageService pageService)
        {
            _pageService = pageService;
        }
        public HttpResponseMessage Get(Guid id)
        {
            var page = _pageService.GetPage(id);
           return Request.CreateResponse(HttpStatusCode.OK, page);
        }
       
    }
}
