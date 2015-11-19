using System.Web;
using System.Web.Optimization;

namespace AUPPRB.Web
{
    public class BundleConfig
    {
        // For more information on Bundling, visit http://go.microsoft.com/fwlink/?LinkId=254725
        public static void RegisterBundles(BundleCollection bundles)
        {

            #region Scripts

            bundles.Add(new ScriptBundle("~/bundles/jqueryJs").Include(
                        "~/Scripts/jquery-{version}.js",
                        "~/Scripts/date.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryUiJs").Include(
                         "~/Scripts/jquery-ui-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrapJs").Include(
                        "~/Scripts/bootstrap/bootstrap*"));

            bundles.Add(new ScriptBundle("~/bundles/mvcToolsJs").Include(
                         "~/Scripts/jquery.unobtrusive*",
                         "~/Scripts/jquery.validate*",
                         "~/Scripts/modernizr-*"));


            bundles.Add(new ScriptBundle("~/bundles/usefulToolsJs").Include(
                        "~/Scripts/choosen/chosen*",
                        "~/Scripts/alertify/alertify*",
                        "~/Scripts/greatCheckboxes/prettyCheckable.js",
                        "~/Scripts/ui.datepicker-ru.js",
                        "~/Scripts/jqueryFileDownload/jquery.fileDownload.js",
                        "~/Scripts/AUPPRB.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqGridJs").Include(
              "~/Scripts/i18n/grid.locale-ru.js",
              "~/Scripts/jquery.jqGrid.js",
              "~/Scripts/jqGrid/jqGridPicker.js",
              "~/Scripts/GridsLibrary.js"));


            #endregion

            #region Styles

            bundles.Add(new StyleBundle("~/Content/aupprbCss").Include(
               "~/Content/aupprb.css"));

            bundles.Add(new StyleBundle("~/Content/bootstrapCss").Include(
                "~/Content/bootstrap/bootstrap*"));


            bundles.Add(new StyleBundle("~/bundles/usefulCssTools").Include(
                "~/Content/choosen/chosen*",
                "~/Content/alertifyCss/alertify*",
                "~/Content/greatCheckboxes/prettyCheckable.css",
                "~/Content/Font-Awesome/css/font-awesome.css"));

            bundles.Add(new StyleBundle("~/Content/themes/custom-theme/customThemeCss").Include(
                "~/Content/jquery.jqGrid/ui.jqgrid.css",
                "~/Content/themes/custom-theme/jquery-ui-1.10.0.custom.css"));

            bundles.Add(new StyleBundle("~/Content/userInterface").Include(
                "~/Content/login.css",
                "~/Content/style.css",
                "~/Content/timetable.css"
                ));
            #endregion

            //bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
            //            "~/Scripts/jquery-{version}.js",
            //            "~/Scripts/date.js"));

            //bundles.Add(new ScriptBundle("~/bundles/jqueryui").Include(
            //                 "~/Scripts/jquery-ui-{version}.js",
            //            "~/Scripts/ui.datepicker-ru.js"));

            //bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
            //            "~/Scripts/jquery.unobtrusive*",
            //            "~/Scripts/jquery.validate*"));



            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.



            //bundles.Add(new ScriptBundle("~/bundles/choosenJS").Include("~/Scripts/choosen/chosen*"));
            //bundles.Add(new StyleBundle("~/bundles/choosenCSS").Include("~/Content/choosen/chosen*"));

            //bundles.Add(new ScriptBundle("~/bundles/jqGrid").Include("~/Scripts/jqSuite/i18n/grid.locale-ru.js"));
            //bundles.Add(new ScriptBundle("~/bundles/jqGridLibrary").Include("~/Scripts/GridsLibrary"));

            //bundles.Add(new ScriptBundle("~/bundles/alertifyJS").Include("~/Scripts/alertify*"));

            //bundles.Add(new StyleBundle("~/bundles/alertifyCSS").Include("~/Content/alertifyCss/alertify*"));






            //bundles.Add(new StyleBundle("~/Content/jqGrid/css").Include("~/Content/jquery.jqGrid/ui*", "~/Content/jquery.jqGrid/jquery*"));






            //bundles.Add(new StyleBundle("~/Content/css").Include("~/Content/site.css", "~/Content/bootstrap/bootstrap*", "~/Content/bootstrap*"));

            //bundles.Add(new StyleBundle("~/Content/themes/base/css").Include(
            //            "~/Content/themes/base/jquery.ui.core.css",
            //            "~/Content/themes/base/jquery.ui.resizable.css",
            //            "~/Content/themes/base/jquery.ui.selectable.css",
            //            "~/Content/themes/base/jquery.ui.accordion.css",
            //            "~/Content/themes/base/jquery.ui.autocomplete.css",
            //            "~/Content/themes/base/jquery.ui.button.css",
            //            "~/Content/themes/base/jquery.ui.dialog.css",
            //            "~/Content/themes/base/jquery.ui.slider.css",
            //            "~/Content/themes/base/jquery.ui.tabs.css",
            //            "~/Content/themes/base/jquery.ui.datepicker.css",
            //            "~/Content/themes/base/jquery.ui.progressbar.css",
            //            "~/Content/themes/base/jquery.ui.theme.css"));
        }
    }
}