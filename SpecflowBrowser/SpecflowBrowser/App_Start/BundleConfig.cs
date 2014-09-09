using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Optimization;
using BundleTransformer.Core.Transformers;

namespace SpecflowBrowser.App_Start
{
    public class BundleConfig
    {
        public static void RegisterBundles(BundleCollection bundles)
        {
            var jsBundle = new ScriptBundle("~/bundles/jquery")
                .Include("~/Scripts/jquery-1.11.1.min.js")
                .Include("~/Scripts/jquery.signalR-2.1.1.min.js")
                .Include("~/Scripts/bootstrap.min.js")
                .Include("~/Scripts/jquery.sparkline.min.js");
            jsBundle.Transforms.Add(new JsTransformer());
            bundles.Add(jsBundle);

            var jsBundleIe8 = new ScriptBundle("~/bundles/ie8")
                .Include("~/Scripts/Respond.js")
                .Include("~/Scripts/html5shiv.js");
            jsBundleIe8.Transforms.Add(new JsTransformer());
            bundles.Add(jsBundleIe8);

#if DEBUG
            var lessBundle = new Bundle("~/bundles/css")
                .Include("~/Content/bootstrap/bootstrap.less")
                .Include("~/Content/site.css");
            lessBundle.Transforms.Add(new CssTransformer());
            lessBundle.Transforms.Add(new CssMinify());
            bundles.Add(lessBundle);
#else
            var lessBundle = new Bundle("~/bundles/css")
                .Include("~/Content/bootstrap/bootstrap.css")
                .Include("~/Content/site.css");
            lessBundle.Transforms.Add(new CssTransformer());
            lessBundle.Transforms.Add(new CssMinify());
            bundles.Add(lessBundle);
#endif
        }
    }
}