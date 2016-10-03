using System.Web;
using System.Web.Optimization;

namespace LibraryAssistantApp
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js",
                        "~/Scripts/jquery.timepicker.min.js",
                        "~/Scripts/notify.min.js",
                        "~/Scripts/jquery.base64.js",
                        "~/Scripts/tableExport.js",
                        "~/Scripts/base64.js",
                        "~/Scripts/sprintf.js",
                        "~/Scripts/jspdf.js",
                        "~/Script/bootbox.js",
                        "~/Scripts/jquery.ambiance.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*",
                        "~/Scripts/jquery.validate.unobtrusive.js",
                        "~/Scripts/dynamicUnobtrusive.js"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                        "~/Scripts/bootstrap.js",
                        "~/Scripts/respond.js"));

            bundles.Add(new ScriptBundle("~/bundles/metisMenu").Include(
                        "~/Scripts/metisMenu.js",
                        "~/Scripts/metisMenuScripts.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                        "~/Content/bootstrap.css",
                        "~/Content/site.css", 
                        "~/Content/navbar-style.css",
                        "~/Content/metisMenu.css", 
                        "~/Content/font-awesome.css",
                        "~/Content/jquery.timepicker.min.css",
                        "~/Content/dhtmlxscheduler.css",
                        "~/Content/jquery.ambiance.css"));

            bundles.Add(new ScriptBundle("~/bundles/headScripts").Include(
                "~/Scripts/Scheduler/dhtmlxscheduler.js",
                "~/Scripts/Scheduler/dhtmlxscheduler_agenda_view.js",
                "~/Scripts/moment.min.js"));

            //checkbox scripts
            bundles.Add(new ScriptBundle("~/bundles/checkboxjs").Include(
                "~/Scripts/checkbox.js"));
            //select rows script
            bundles.Add(new ScriptBundle("~/bundles/selectrowjs").Include(
                "~/Script/selectrow.js"));
            //model script
            bundles.Add(new ScriptBundle("~/bundles/bootbox").Include(
                "~/Script/bootbox.js"));
        }
    }
}
