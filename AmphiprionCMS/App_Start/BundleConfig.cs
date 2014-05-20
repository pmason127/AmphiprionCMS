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

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.js",
                      "~/Scripts/respond.js"));

            bundles.Add(new ScriptBundle("~/bundles/application").Include(
                     "~/Scripts/application.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/bootstrap.css",
                      "~/Content/site.css"));


            bundles.Add(new StyleBundle("~/Content/admin/css").Include(
                      "~/Content/admin.css", "~/Content/iconmoon/style.css"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrapdate").Include(
                      "~/Scripts/moment.js",
                      "~/Scripts/bootstrap-datetimepicker.js"));

            bundles.Add(new ScriptBundle("~/bundles/backbone").Include(
                      "~/Scripts/underscore.js",
                      "~/Scripts/backbone.js",
                      "~/Scripts/backbone-validation.js",
                       "~/Scripts/backbone.marionette.js"));


            bundles.Add(new ScriptBundle("~/bundles/pageapp").Include(
                  "~/Scripts/app/page/app.js"
                  , "~/Scripts/app/page/pagelist_controller.js"
                   , "~/Scripts/app/page/page_controller.js"
                 ,"~/Scripts/app/page/views.js"
                , "~/Scripts/app/page/models.js"
                , "~/Scripts/app/page/controller.js"
                   ));

            bundles.Add(new StyleBundle("~/Content/datetimepickercss").Include(
                     "~/Content/bootstrap-datetimepicker.css"));

            bundles.Add(new ScriptBundle("~/bundles/editor").Include( "~/Scripts/ckeditor/ckeditor.js"));
            bundles.Add(new ScriptBundle("~/bundles/adapter").Include(
                     "~/Scripts/ckeditor/adapters/jquery.js"));

            bundles.Add(new ScriptBundle("~/bundles/administration").Include(
                    "~/Scripts/tree.js"));

            bundles.Add(new StyleBundle("~/Content/icons").Include(
                     "~/Content/iconmoon/style.css"));

        }
    }
}
