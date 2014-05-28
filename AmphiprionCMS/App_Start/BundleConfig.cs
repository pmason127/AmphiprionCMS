using System.Web;
using System.Web.Optimization;

namespace AmphiprionCMS
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*"));

            bundles.Add(new ScriptBundle("~/bundles/amphiprion/bootstrap").Include(
                      "~/AmphiprionCMS/Scripts/bootstrap.js",
                      "~/AmphiprionCMS/Scripts/respond.js"));

            bundles.Add(new ScriptBundle("~/bundles/amphiprion/application").Include(
                     "~/AmphiprionCMS/Scripts/application.js"));

            bundles.Add(new StyleBundle("~/Content/amphiprion/css").Include(
                      "~/AmphiprionCMS/Content/bootstrap.css",
                      "~/AmphiprionCMS/Content/site.css"));


            bundles.Add(new StyleBundle("~/Content/amphiprion/admin/css").Include(
                      "~/AmphiprionCMS/Content/admin.css", "~/AmphiprionCMS/Content/iconmoon/style.css"));



            bundles.Add(new ScriptBundle("~/bundles/amphiprion/editor").Include("~/AmphiprionCMS/Scripts/ckeditor/ckeditor.js"));
            bundles.Add(new ScriptBundle("~/bundles/amphiprion/adapter").Include(
                     "~/AmphiprionCMS/Scripts/ckeditor/adapters/jquery.js"));

        }
    }
}
