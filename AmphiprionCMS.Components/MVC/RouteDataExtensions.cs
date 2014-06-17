using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Web;
using System.Web.Routing;
using AmphiprionCMS.Code;

namespace AmphiprionCMS.Components
{
    public static class RouteDataExtensions
    {
       
        public static string GetString(this RouteData data,string key)
        {
            if (data == null || data.Values == null || data.Values[key] == null) return null;
            var value = data.Values[key];
            return value.ToString();
        }
        public static int? GetInt(this RouteData data, string key)
        {
            if (data == null || data.Values == null || data.Values[key] == null) return null;
            var value = data.Values[key];
            return Convert.ToInt32(value);
        }
        public static Guid? GetGuid(this RouteData data, string key)
        {
            if (data == null || data.Values == null || data.Values[key] == null) return null;
            var value = data.Values[key];
            return new Guid(value.ToString());
        }
        public static DateTime? GetDateTime(this RouteData data, string key)
        {
            if (data == null || data.Values == null || data.Values[key] == null) return null;
            var value = data.Values[key];
            return Convert.ToDateTime(value);
        }
    }
}