using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Channels;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Caching;
using RazorEngine;
using RazorEngine.Templating;

namespace AmphiprionCMS.Components
{
    public interface IContentFragment
    {
        string Name { get; set; }
        string Description { get; set; }
        string Render(HttpContextBase context);
        IList<ConfigurationField> Fields { get; }
    }

    public class ConfigurationField
    {
        public string Name { get; set; }
        public string Label { get; set; }
        public string DefaultValue { get; set; }
        public string Value { get; set; }
        bool IsCacheable { get; set; }
        bool VaryCacheByUser { get; set; }
    }

    public abstract class RazorTemplateContentFragmentBase<T> : IContentFragment
    {
        protected RazorTemplateContentFragmentBase()
        {
            VaryCacheByUser = false;
            IsCacheable = true;
        }
        public abstract string Name
        {
            get;
            set;
        }
        public abstract string Description
        {
            get;
            set;
        }

        public abstract string TemplateFile { get; }
        public abstract T GetModel();
        public virtual bool IsCacheable { get; set; }
        public virtual bool VaryCacheByUser { get; set; }
        public virtual string Render(HttpContextBase context)
        {
            string template = System.IO.File.ReadAllText((context.Server.MapPath(this.TemplateFile)));
            if (string.IsNullOrEmpty(template))
                return string.Empty;

            var model = GetModel();
            string cacheName = null;

            if (IsCacheable)
            {
                cacheName = this.Name;
                if (VaryCacheByUser)
                    cacheName += "-user"; //TODO:Add user name
            }
            DynamicViewBag bag = new DynamicViewBag();
            bag.AddValue("FragmentName",this.Name);
            bag.AddValue("FragmentDescription", this.Description );
            if (Fields != null)
            {
                foreach (var prop in Fields)
                {
                    bag.AddValue(prop.Name, prop.Value);
                }
            }

            return Razor.Parse(template, model, bag, cacheName);

        }

        public virtual IList<ConfigurationField> Fields
        {
            get { return null; }
        }
    }

    public class SampleModel
    {
        public string Message { get; set; }
    }

    public class SampleRazorTemplate : RazorTemplateContentFragmentBase<SampleModel>
    {

        public override string Name
        {
            get
            {
               return "Sample Razor Template";
            }
            set
            {
              
            }
        }

        public override string Description
        {
            get
            {
                return "A sample template";
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public override string TemplateFile
        {
            get {return "~/Views/ContentFragments/sample.cshtml"; }
        }

        public override SampleModel GetModel()
        {
           return new SampleModel(){Message = "Hello World"};
        }
    }

}
