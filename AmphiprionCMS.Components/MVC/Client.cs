using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AmphiprionCMS.Code
{
    public static  class Client
    {
        private static readonly string tzKey = "tzOffset";

        public static DateTime LocalNow(HttpContextBase context)
        {
            return LocalizeDateTime(DateTime.UtcNow, context);
        }
        public static DateTime LocalizeDateTime(DateTime inDateTime,HttpContextBase context)
        {
            int offset = GetClientTimezoneOffsetInMinutes(context);
            return inDateTime.AddMinutes(-1 * offset);
        }
        public static int GetClientTimezoneOffsetInMinutes(HttpContextBase context)
        {
            int offset = 0;
            bool isParsed = false;
            var httpMethod = context.Request.HttpMethod.ToUpper();
            if(httpMethod == "POST" || httpMethod == "PUT")
            {
                if(context.Request.Form != null && context.Request.Form[tzKey] != null)
                {
                    isParsed = int.TryParse(context.Request.Form[tzKey], out offset);
                    if (isParsed)
                        return offset;
                }
            }
            else
            { 
                if(context.Request.QueryString != null && context.Request.QueryString[tzKey] != null)
                {
                    isParsed = int.TryParse(context.Request.QueryString[tzKey], out offset);
                    if (isParsed)
                        return offset;
                }
            }


            var tzCookie = context.Request.Cookies[tzKey];
            if (tzCookie != null)
            {
                var tzStr = tzCookie.Value;
                int.TryParse(tzStr, out offset);
            }

            return offset;
        }

        public static  string GetTimeZoneDisplay(HttpContextBase context)
        {
            var tzOffset = (GetClientTimezoneOffsetInMinutes(context) * -1) /60;
            return ("UTC " + (tzOffset > 0 ? "+" : "") + tzOffset.ToString()).TrimEnd();
        }
    }
}