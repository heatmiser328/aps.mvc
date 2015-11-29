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
                .Include("~/Content/bootstrap.min.css")
                .Include("~/Content/fontawesome.min.css")                
                .Include("~/Content/ui-grid.min.css")
                .Include("~/Content/Site.css")
            );

            bundles.Add(new ScriptBundle("~/bundles/angularjs")
                .Include("~/Scripts/angular.min.js")
                .Include("~/Scripts/angular-ui-router.min.js")
                .Include("~/Scripts/ui-grid.min.js")
                .Include("~/Scripts/angular-ui/ui-bootstrap-tpls.min.js")
            );

            bundles.Add(new ScriptBundle("~/bundles/vendorjs")
                .Include("~/Scripts/lodash.min.js")
                .Include("~/Scripts/moment.min.js")
            );

            bundles.Add(new ScriptBundle("~/bundles/apsLogin")                          
                .Include("~/Scripts/account/login.js")
            );

            bundles.Add(new ScriptBundle("~/bundles/aps")
                .IncludeDirectory("~/Scripts/app", "*.js", true)
            );

            // Set EnableOptimizations to false for debugging. For more information,
            // visit http://go.microsoft.com/fwlink/?LinkId=301862
            BundleTable.EnableOptimizations = true;
        }
    }
}
