namespace SpecflowBrowser.App_Start
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Web;
    using System.Web.Optimization;
    using dotless.Core;
    using dotless.Core.Abstractions;
    using dotless.Core.Importers;
    using dotless.Core.Input;
    using dotless.Core.Loggers;
    using dotless.Core.Parser;

    public class LessTransform : IBundleTransform
    {
        public void Process(BundleContext context, BundleResponse bundle)
        {
            bundle.Content = dotless.Core.Less.Parse(bundle.Content);
            bundle.ContentType = "text/css";
        }
    }
}