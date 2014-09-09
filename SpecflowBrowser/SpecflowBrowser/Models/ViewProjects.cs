using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SpecflowBrowser.Models
{
    public class ViewProjects
    {
        public ViewProjects()
        {
            LstTestResults = new List<string>();
        }

        public string NomProjet { get; set; }
        public string VersionProjet { get; set; }
        public List<string> LstTestResults { get; set; }
    }
}