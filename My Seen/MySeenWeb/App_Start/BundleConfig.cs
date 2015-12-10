using System.Web.Optimization;

namespace MySeenWeb
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {

            BundleTable.EnableOptimizations = false;// чтобы небыло жести типа <script src="/bundles/modernizr?v=inCVuEFe6J4Q07A0AcRsbJic_UE5MwpRMNGcOtk94TE1"></script>

            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryUi").Include(
                        "~/Scripts/jquery-ui-{version}.js"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.js",
                      "~/Scripts/respond.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/bootstrap.css",
                      "~/Content/myseen/site.css"));

            bundles.Add(new ScriptBundle("~/bundles/datetimepicker").Include(
                      "~/Scripts/moment-with-locales.js",
                      "~/Scripts/bootstrap-datetimepicker.js"));

            bundles.Add(new StyleBundle("~/Content/datetimepicker.css").Include(
                      "~/Content/bootstrap-datetimepicker.css"));

            bundles.Add(new ScriptBundle("~/bundles/gmap3").Include(
                        "~/Scripts/gmap3.js"));

            bundles.Add(new ScriptBundle("~/bundles/myseen.table").Include(
                        "~/Scripts/myseen/table.js"));

            bundles.Add(new ScriptBundle("~/bundles/myseen.gmap").Include(
                      "~/Scripts/myseen/gmap.js",
                      "~/Scripts/myseen/gmap.tools.js"
                      ));

            bundles.Add(new ScriptBundle("~/bundles/myseen.gmap.editor").Include(
                       "~/Scripts/myseen/gmap.editor.js",
                       "~/Scripts/myseen/gmap3menu.js",
                       "~/Scripts/myseen/gmap.tools.js"
                       ));


            bundles.Add(new StyleBundle("~/Content/css.gmap3menu").Include(
                       "~/Content/myseen/gmap3-menu.css"));

            bundles.Add(new ScriptBundle("~/bundles/knockout").Include(
                       "~/Scripts/knockout-{version}.js"
           ));

        }
    }
}
