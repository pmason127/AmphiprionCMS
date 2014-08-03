using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Routing;
using System.Web.Mvc;

namespace AmphiprionCMS.Code
{
    public class ServerAccessOnlyRouteConstraint : IRouteConstraint
    {
        public bool Match(HttpContextBase httpContext, Route route, string parameterName, RouteValueDictionary values, RouteDirection routeDirection)
        {
            if (httpContext == null || !httpContext.Request.IsLocal )
                return false;

            return true;
        }
    }
}