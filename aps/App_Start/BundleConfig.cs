using System.Web;
using System.Web.Optimization;

namespace aps
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new StyleBundle("~/Content/css")
                .Include("~/Content/site.css")
            );

            bundles.Add(new ScriptBundle("~/bundles/angularjs")
                .Include("~/Scripts/angular.js")
                .Include("~/Scripts/angular-ui-router.js")
            );

            bundles.Add(new ScriptBundle("~/bundles/apsLogin")
                .Include("~/Scripts/account/login.js")
            );

            //bundles.Add(new ScriptBundle("~/bundles/aps").IncludeDirectory())

            // Set EnableOptimizations to false for debugging. For more information,
            // visit http://go.microsoft.com/fwlink/?LinkId=301862
            BundleTable.EnableOptimizations = true;
        }
    }
}
