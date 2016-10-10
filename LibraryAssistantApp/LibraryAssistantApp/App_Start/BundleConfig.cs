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
                        "~/Scripts/bxSlider/jquery.bxslider.min.js",
                        "~/Scripts/DataTable/datatables.min.js",
                        "~/Scripts/jquery-{version}.js",
                        "~/Scripts/jquery.timepicker.min.js",
                        "~/Scripts/notify.min.js",
                        "~/Scripts/jquery.base64.js",
                        "~/Scripts/tableExport.js",
                        "~/Scripts/base64.js",
                        "~/Scripts/sprintf.js",
                        "~/Scripts/jspdf.js",
                        "~/Script/bootbox.js",
                        "~/Scripts/jquery.ambiance.js",
                        "~/Scripts/DataTables/jquery.dataTables.min.js",
                        "~/Scripts/DataTables/dataTables.bootstrap.min.js"));

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
                        "~/Content/jquery.ambiance.css",
                        "~/Content/jquery.bxslider.css",
                        //"~/Content/DataTables/css/jquery.dataTables.min.css",
                        "~/Content/DataTables/css/dataTables.bootstrap.min.css"));

            bundles.Add(new ScriptBundle("~/bundles/headScripts").Include(
                "~/Scripts/Scheduler/dhtmlxscheduler.js",
                "~/Scripts/Scheduler/dhtmlxscheduler_agenda_view.js",
                "~/Scripts/moment.min.js"));

            //checkbox scripts
            bundles.Add(new ScriptBundle("~/bundles/checkboxjs").Include(
                "~/Scripts/checkbox.js"));
            //select rows script
            bundles.Add(new ScriptBundle("~/bundles/selectrowjs").Include(
                "~/Scripts/selectrow.js"));
            //modal script
            bundles.Add(new ScriptBundle("~/bundles/bootbox").Include(
                "~/Scripts/bootbox.js"));
            //notification 'noty' scripts
            bundles.Add(new ScriptBundle("~/bundles/noty").Include(
                "~/Scripts/noty/jquery.noty.js",
                "~/Scripts/noty/layouts/bottom.js",
                "~/Scripts/noty/layouts/bottomCenter.js",
                "~/Scripts/noty/layouts/bottomLeft.js",
                "~/Scripts/noty/layouts/bottomRight.js",
                "~/Scripts/noty/layouts/center.js",
                "~/Scripts/noty/layouts/centerLeft.js",
                "~/Scripts/noty/layouts/centerRight.js",
                "~/Scripts/noty/layouts/inline.js",
                "~/Scripts/noty/layouts/top.js",
                "~/Scripts/noty/layouts/topCenter.js",
                "~/Scripts/noty/layouts/topLeft.js",
                "~/Scripts/noty/layouts/topRight.js",
                "~/Scripts/noty/themes/default.js",
                "~/Scripts/noty/themes/relax.js"
                ));
        }
    }
}