using System.Web.Optimization;
using MySeenLib;

namespace MySeenWeb
{
    public class BundleConfig
    {
        public static void RegisterBundles(BundleCollection bundles)
        {
            BundleTable.EnableOptimizations = false;
            // чтобы небыло жести типа <script src="/bundles/modernizr?v=inCVuEFe6J4Q07A0AcRsbJic_UE5MwpRMNGcOtk94TE1"></script>
            // но т.к. надо min файлы напишу сам...

            //=> grunt
            bundles.Add(new ScriptBundle("~/js").Include("~/Content/prod/production" + (Admin.IsDebug ? "" : ".min") + ".js"));
            bundles.Add(new StyleBundle("~/css").Include("~/Content/prod/production" + (Admin.IsDebug ? "" : ".min") + ".css"));
        }
    }
}
