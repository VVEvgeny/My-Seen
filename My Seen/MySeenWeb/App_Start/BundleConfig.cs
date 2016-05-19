using System.Web.Optimization;
using static MySeenLib.Admin;

namespace MySeenWeb
{
    public class BundleConfig
    {
        public static void RegisterBundles(BundleCollection bundles)
        {
            BundleTable.EnableOptimizations = false;
            // чтобы небыло жести типа <script src="/bundles/modernizr?v=inCVuEFe6J4Q07A0AcRsbJic_UE5MwpRMNGcOtk94TE1"></script>
            // но т.к. надо min файлы напишу сам...

            if (IsDebug)
            {
                bundles.Add(new ScriptBundle("~/js").Include(
                    //jquery
                    "~/Scripts/jquery-{version}.js",
                    "~/Scripts/jquery.validate.js",
                    "~/Scripts/jquery.validate.unobtrusive.js",
                    //angular
                    "~/Scripts/angular.js",
                    "~/Scripts/angular-ui-router.js",
                    "~/Scripts/angular-animate.js",
                    "~/Scripts/angular-ui/ui-bootstrap-tpls.js",
                    //my app
                    "~/Scripts/myseen/jQueryToAngular.js",
                    "~/Content/Angular/App/app.js",
                    "~/Content/Angular/controllers/*.js",
                    "~/Content/Angular/controllers/Admin/*.js",
                    "~/Content/Angular/controllers/MyMemory/*.js",
                    "~/Content/Angular/controllers/MyMemory/Shared/*.js",
                    "~/Content/Angular/controllers/Portal/*.js",
                    "~/Content/Angular/controllers/Tests/*.js",
                    //amcharts
                    "~/Content/amcharts/amcharts.js",
                    "~/Content/amcharts/serial.js",
                    "~/Content/amcharts/themes/light.js",
                    "~/Scripts/modernizr-{version}.js",
                    //bootstrap
                    "~/Scripts/bootstrap.js",
                    "~/Scripts/respond.js",
                    //gmap3
                    "~/Scripts/gmap3.js",
                    "~/Scripts/myseen/gmap.js",
                    "~/Scripts/myseen/gmap.tools.js",
                    "~/Scripts/myseen/variables.js",
                    "~/Scripts/myseen/gmap3menu.js",
                    "~/Scripts/myseen/gmap.editor.js",
                    //tools
                    "~/Scripts/moment-with-locales.js",
                    "~/Scripts/bootstrap-datetimepicker.js",
                    "~/Scripts/myseen/jsNlog.js",
                    "~/Scripts/myseen/timer.js",
                    //test
                    "~/Content/Angular/templates/Tests/skillsdata.js",
                    "~/Content/Angular/templates/Tests/skill.js"
                    ));

                bundles.Add(new StyleBundle("~/css").Include(
                    "~/Content/myseen/Site.css",
                    "~/Content/myseen/navbar.css",
                    "~/Content/bootstrap-datetimepicker.css",
                    "~/Content/bootstrap-theme.css",
                    "~/Content/animate.css",
                    "~/Content/font-awesome.css",
                    "~/Content/myseen/skill.css",
                    "~/Content/myseen/gmap3-menu.css"
                    ));
            }
            else
            {
                bundles.Add(new ScriptBundle("~/js").Include("~/Content/prod/production.min.js"));
                bundles.Add(new StyleBundle("~/css").Include("~/Content/prod/production.min.css"));
            }
        }
    }
}
