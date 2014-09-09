namespace SpecflowBrowser.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;
    using System.Web.Mvc;
    using System.IO;
    using System.Configuration;
    using BL;
    using SpecflowBrowser.Models;

    public class HomeController : Controller
    {
        private SpecflowEntities Entities;
        public HomeController(SpecflowEntities entities)
        {
            Entities = entities;
        }

        //
        // GET: /Home/

        public ActionResult Index()
        {
            return View(Entities.Projets.Select(projet =>
                new ViewProjects
                {
                    NomProjet = projet.Nom,
                    VersionProjet = projet.Version,
                    LstTestResults = Entities.TestInfoes
                        .Where(t => t.Scenario.Fonctionnalite.ProjetId == projet.Id)
                        .GroupBy(t => t.Scenario.Fonctionnalite)
                        .Select(a => a.Count(f => f.Result == "Failed").ToString() + ":" +
                                    a.Count(f => f.Result == "Inconclusive").ToString() + ":" +
                                    a.Count(t => t.Result == "Passed").ToString()
                                )
                        .ToList()
                }).ToList());
        }
    }
}
