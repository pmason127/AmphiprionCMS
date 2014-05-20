using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Routing;
using System.Web.Mvc;

namespace AmphiprionCMS.Code
{
    public class CMSIgnoreRouteContraint:IRouteConstraint
    {
        public bool Match(HttpContextBase httpContext, Route route, string parameterName, RouteValueDictionary values, RouteDirection routeDirection)
        {
            if (httpContext == null
                || httpContext.Request.IsAjaxRequest()
                || httpContext.Request.HttpMethod.ToUpper() != "GET" )
                return false;

            return true;
        }
    }
}