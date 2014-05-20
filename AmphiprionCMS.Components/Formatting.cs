using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web.UI;
using HtmlAgilityPack;

namespace AmphiprionCMS.Components
{
    public interface IFormatting
    {
        string RemoveScript(string text);
        string RemoveHtml(string text);
        string CreateSlug(string text,int maxLength);
    }

    public class Formatting : IFormatting
    {
        private static readonly Regex scripts = new Regex("(\\<script(.+?)\\</script\\>)",RegexOptions.Compiled|RegexOptions.IgnoreCase|RegexOptions.Multiline);
        public string RemoveScript(string text)
        {
            HtmlDocument doc = new HtmlDocument();
            doc.LoadHtml(text);

            //Remove potentially harmful elements
            HtmlNodeCollection nc = doc.DocumentNode.SelectNodes("//script|//link|//iframe|//frameset|//frame|//applet|//object");
            if (nc != null)
            {
                foreach (HtmlNode node in nc)
                {
                    node.ParentNode.RemoveChild(node, false);

                }
            }

            //remove hrefs to java/j/vbscript URLs
            nc = doc.DocumentNode.SelectNodes("//a[starts-with(@href, 'javascript')]|//a[starts-with(@href, 'jscript')]|//a[starts-with(@href, 'vbscript')]");
            if (nc != null)
            {

                foreach (HtmlNode node in nc)
                {
                    node.SetAttributeValue("href", "protected");
                }
            }



            //remove img with refs to java/j/vbscript URLs
            nc = doc.DocumentNode.SelectNodes("//img[starts-with(@src, 'javascript')]|//img[starts-with(@src, 'jscript')]|//img[starts-with(@src, 'vbscript')]");
            if (nc != null)
            {
                foreach (HtmlNode node in nc)
                {
                    node.SetAttributeValue("src", "protected");
                }
            }

            //remove on<Event> handlers from all tags
            nc = doc.DocumentNode.SelectNodes("//*[@onclick or @onmouseover or @onfocus or @onblur or @onmouseout or @ondoubleclick or @onload or @onunload]");
            if (nc != null)
            {
                foreach (HtmlNode node in nc)
                {
                    node.Attributes.Remove("onFocus");
                    node.Attributes.Remove("onBlur");
                    node.Attributes.Remove("onClick");
                    node.Attributes.Remove("onMouseOver");
                    node.Attributes.Remove("onMouseOut");
                    node.Attributes.Remove("onDoubleClick");
                    node.Attributes.Remove("onLoad");
                    node.Attributes.Remove("onUnload");
                }
            }


            return doc.DocumentNode.WriteTo();
        }
        public string RemoveHtml(string text)
        {
            return null;
        }
        public string CreateSlug(string text,int maxLength)
        {
            string str = RemoveAccent(text).ToLower();
            // invalid chars           
            str = Regex.Replace(str, @"[^a-z0-9\s-]", "");
            // convert multiple spaces into one space   
            str = Regex.Replace(str, @"\s+", " ").Trim();
            // cut and trim 
            str = str.Substring(0, str.Length <= maxLength ? str.Length : maxLength).Trim();
            str = Regex.Replace(str, @"\s", "-"); // hyphens   
            return str;
        }
       
        private string RemoveAccent(string txt)
        {
            byte[] bytes = System.Text.Encoding.GetEncoding("Cyrillic").GetBytes(txt);
            return System.Text.Encoding.ASCII.GetString(bytes);
        }
    }
}
