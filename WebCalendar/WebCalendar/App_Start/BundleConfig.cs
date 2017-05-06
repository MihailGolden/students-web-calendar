using System.Web.Optimization;

namespace WebCalendar
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryui").Include(
            "~/Scripts/jquery-ui.min.js"));


            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                       "~/Scripts/jquery.validate.min.js",
                       "~/Scripts/jquery.validate.unobtrusive.min.js"));
            bundles.Add(new ScriptBundle("~/bundles/globalize").Include(
                 "~/Scripts/globalize.js", "~/Scripts/globalize/globalize-cultures/globalize.culture.uk-UA.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryglobal").Include(
    "~/Scripts/jquery.validate.globalize.min.js"));
            bundles.Add(new ScriptBundle("~/bundles/moment").Include(
                        "~/Scripts/moment.js"));
            bundles.Add(new ScriptBundle("~/bundles/jscolor").Include(
            "~/Scripts/jscolor.js"));
            bundles.Add(new ScriptBundle("~/bundles/colorpicker").Include(
            "~/Scripts/bootstrap-colorpicker.js"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.js",
                      "~/Scripts/respond.js",
                      "~/Scripts/bootstrap-datetimepicker.js", "~/Scripts/angular.min.js",
                      "~/Scripts/app.js, ~/Script/calendar-event.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include("~/Content/Site.css",
                      "~/Content/bootstrap-datetimepicker.css", "~/Content/bootstrap-colorpicker.css"));
        }
    }
}
