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
                    //"~/node_modules/jquery/dist/jquery.js",//13/06/16 bootstrap.js:15 Uncaught Error: Bootstrap's JavaScript requires jQuery version 1.9.1 or higher, but lower than version 3 
                    "~/node_modules/jquery-validation/dist/jquery.validate.js",
                    //angular
                    "~/node_modules/angular/angular.js",
                    "~/node_modules/angular-ui-router/release/angular-ui-router.js",
                    "~/node_modules/angular-animate/angular-animate.js",
                    "~/node_modules/angular-ui-bootstrap/dist/ui-bootstrap-tpls.js",
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
                    "~/node_modules/amcharts/dist/amcharts/amcharts.js",
                    "~/node_modules/amcharts/dist/amcharts/serial.js",
                    "~/node_modules/amcharts/dist/amcharts/themes/light.js",
                    //tools
                    "~/Scripts/modernizr-{version}.js",
                    //bootstrap
                    "~/node_modules/bootstrap/dist/js/bootstrap.js",
                    //"~/node_modules/respond/main.js",
                    //gmap3
                    "~/Scripts/gmap3.js",
                    "~/Scripts/myseen/gmap.js",
                    "~/Scripts/myseen/gmap.tools.js",
                    "~/Scripts/myseen/variables.js",
                    "~/Scripts/myseen/gmap3menu.js",
                    "~/Scripts/myseen/gmap.editor.js",
                    //tools
                    "~/node_modules/moment/min/moment-with-locales.js",
                    "~/node_modules/eonasdan-bootstrap-datetimepicker/build/js/bootstrap-datetimepicker.min.js",
                    "~/Scripts/myseen/jsNlog.js",
                    "~/Scripts/myseen/timer.js",
                    //test
                    "~/Content/Angular/templates/Tests/skillsdata.js",
                    "~/Content/Angular/templates/Tests/skill.js"
                    ));

                bundles.Add(new StyleBundle("~/css").Include(
                    "~/node_modules/eonasdan-bootstrap-datetimepicker/build/css/bootstrap-datetimepicker.css",
                    "~/node_modules/bootstrap/dist/css/bootstrap-theme.css",
                    "~/Content/myseen/Site.css",
                    "~/Content/myseen/navbar.css",
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
