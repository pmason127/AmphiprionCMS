using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace AmphiprionCMS.Code
{
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