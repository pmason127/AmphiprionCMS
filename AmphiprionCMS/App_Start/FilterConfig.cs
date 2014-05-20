using System.Web;
using System.Web.Mvc;
using AmphiprionCMS.Code;

namespace AmphiprionCMS
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
            filters.Add(new System.Web.Mvc.AuthorizeAttribute());

        }
    }
}
