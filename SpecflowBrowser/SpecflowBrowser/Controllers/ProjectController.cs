namespace SpecflowBrowser.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Configuration;
    using System.IO;
    using System.Linq;
    using System.Web.Mvc;
    using BL;
    using DTO;
    using Newtonsoft.Json;
    using SpecflowBrowser.Models;

    public class ProjectController : Controller
    {
        private SpecflowEntities Entities;
        public ProjectController(SpecflowEntities entities)
        {
            Entities = entities;
        }

        public ActionResult Index(string nomProjet, string versionProjet, ProjectModel project)
        {
            Projet projet = Entities.Projets.FirstOrDefault(p=>p.Nom == nomProjet && p.Version == versionProjet);
            project.Result = projet;

            return View(projet);
        }

        public ActionResult DetailFonctionnalite(string featureId, string scenarioId)
        {
            Guid guidId = Guid.Parse(featureId);

            Fonctionnalite feature = Entities.Fonctionnalites.FirstOrDefault(s => s.Id == guidId);
            if (feature == null)
            {
                return new HttpNotFoundResult();
            }
            ViewBag.ScenarioId = scenarioId;

            return View(feature);
        }

        public ActionResult DetailScenario(string scenarioId)
        {
            Guid guidId = Guid.Parse(scenarioId);

            Scenario scenario = Entities.Scenarios.FirstOrDefault(s => s.Id == guidId);
            if (scenario == null)
            {
                return new HttpNotFoundResult();
            }

            return View(scenario);
        }
    }
}
