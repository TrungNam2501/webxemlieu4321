using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Optimization;
using System.Web.UI;

namespace KendaWeb
{
    public class BundleConfig
    {
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                "~/Scripts/jquery-3.4.1.js*"));
            bundles.Add(new ScriptBundle("~/bundles/jquery_ui").Include(
            "~/Scripts/jquery-ui.js*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                "~/Scripts/bootstrap.min.js*"));

            bundles.Add(new ScriptBundle("~/bundles/main").Include(
                "~/Scripts/main.js*"));
        }
    }
}