using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http.Controllers;
using System.Web.Http.ModelBinding;

namespace SpecflowBrowser.Models
{
    public class ProjectModelBinder : IModelBinder
    {
        private const string SessionKey = "Project";

        #region IModelBinder Membres

        bool IModelBinder.BindModel(HttpActionContext actionContext, ModelBindingContext bindingContext)
        {
            if (System.Web.HttpContext.Current.Session[SessionKey] == null)
            {
                System.Web.HttpContext.Current.Session[SessionKey] = new ProjectModel();
            }

            bindingContext.Model = System.Web.HttpContext.Current.Session[SessionKey];

            return true;
        }

        #endregion
    }
}