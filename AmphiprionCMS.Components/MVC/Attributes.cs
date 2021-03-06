﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using AmphiprionCMS.Components.Authentication;
using Microsoft.Practices.ServiceLocation;

namespace AmphiprionCMS.Code
{
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class,
                AllowMultiple = false, Inherited = true)]
    public sealed class ValidateJsonAntiForgeryTokenAttribute
                                : FilterAttribute, IAuthorizationFilter
    {
        public void OnAuthorization(AuthorizationContext filterContext)
        {
            if (filterContext == null)
            {
                throw new ArgumentNullException("filterContext");
            }

            var httpContext = filterContext.HttpContext;
            var cookie = httpContext.Request.Cookies[AntiForgeryConfig.CookieName];
            AntiForgery.Validate(cookie != null ? cookie.Value : null,
                                 httpContext.Request.Headers["__RequestVerificationToken"]);
        }
    }

    public class CMSAuthorizeAttribute:AuthorizeAttribute
    {
        public CMSAuthorizeAttribute()
        {
                
        }
        public CMSAuthorizeAttribute(string permission)
        {
            _permission = permission;
        }

        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            var authenticaton = ServiceLocator.Current.GetInstance<ICMSAuthentication>();
            var authorizer = ServiceLocator.Current.GetInstance<ICMSAuthorization>();

            if (!authenticaton.IsAuthenticated)
                return false;

           if(!string.IsNullOrEmpty(Permission))
               if (!authorizer.RequestPermission(Permission))
                return false;

            return true;
        }

        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            var authenticaton = ServiceLocator.Current.GetInstance<ICMSAuthentication>();
            if (!authenticaton.IsAuthenticated)
                filterContext.Result = new HttpUnauthorizedResult();
            else
                filterContext.Result = new HttpStatusCodeResult(HttpStatusCode.Forbidden);
        }
        private string _permission;
 
        public string Permission
        {
            get { return _permission ?? String.Empty; }
            set
            {
                _permission = value;
              
            }
        }
        
    }

    public class HandleAjaxModelErrors:ActionFilterAttribute
    {
        public override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            if (!filterContext.HttpContext.Request.IsAjaxRequest())
                return;

             var modelState = filterContext.Controller.ViewData.ModelState;
            if (!modelState.IsValid)
            {
                var errorModel =
                   from x in modelState.Keys
                   where modelState[x].Errors.Count > 0
                   select new
                   {
                       key = x,
                       errors = modelState[x].Errors.
                                              Select(y => y.ErrorMessage).
                                              ToArray()
                   };
                filterContext.Result = new JsonResult()
                {
                    Data = errorModel
                };
                filterContext.HttpContext.Response.StatusCode =
                                                      (int)HttpStatusCode.BadRequest;
            }
        }
    }
    public class ErrorHandlerAttribute : HandleErrorAttribute
    {
         
        public override void OnException(ExceptionContext filterContext)
    {
        if (filterContext == null)
        {
            throw new ArgumentNullException("filterContext");
        }
        if (!filterContext.ExceptionHandled && filterContext.HttpContext.IsCustomErrorEnabled)
        {
            Exception innerException = filterContext.Exception;
            if ((new HttpException(null, innerException).GetHttpCode() == 500) && this.ExceptionType.IsInstanceOfType(innerException))
            {
                string controllerName = (string) filterContext.RouteData.Values["controller"];
                string actionName = (string) filterContext.RouteData.Values["action"];
                // Preserve old ViewData here
                var viewData = new ViewDataDictionary<HandleErrorInfo>(filterContext.Controller.ViewData);
                // Set the Exception information model here
                viewData.Model = new HandleErrorInfo(filterContext.Exception, controllerName, actionName);
                filterContext.Result = new ViewResult { ViewName = this.View, MasterName = this.Master, ViewData = viewData, TempData = filterContext.Controller.TempData };
                filterContext.ExceptionHandled = true;
                filterContext.HttpContext.Response.Clear();
                filterContext.HttpContext.Response.StatusCode = 500;
                filterContext.HttpContext.Response.TrySkipIisCustomErrors = true;
            }
        }
    }
    }
}