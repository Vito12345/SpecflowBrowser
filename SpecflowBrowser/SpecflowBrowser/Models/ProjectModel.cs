using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http.ModelBinding;
using BL;

namespace SpecflowBrowser.Models
{
    [ModelBinder]
    public class ProjectModel
    {
        private const string SessionKey = "Project";
        private Projet SessionStateLessPanier = null;
        public Projet Result
        {
            get
            {
                if (System.Web.HttpContext.Current == null || System.Web.HttpContext.Current.Session == null)
                {
                    // il s'agit certainement d'un test unitaire
                    return SessionStateLessPanier;
                }

                return System.Web.HttpContext.Current.Session[SessionKey] as Projet;
            }
            set
            {
                if (System.Web.HttpContext.Current == null || System.Web.HttpContext.Current.Session == null)
                {
                    // il s'agit certainement d'un test unitaire
                    SessionStateLessPanier = value;
                }
                else
                {
                    System.Web.HttpContext.Current.Session[SessionKey] = value;
                }
            }
        }
    }
}