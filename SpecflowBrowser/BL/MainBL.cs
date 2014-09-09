namespace BL
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Xml.Linq;
    using Bytel.Cora.Socle.Exception;

    public static class MainBL
    {
        public static void Analyser(string nomProjet, string versionProjet, string pathToFeatures, 
            string pathToDll, string pathToTestSetting, TextWriter output)
        {
            if (!Directory.Exists(pathToFeatures))
            {
                throw new ExceptionFonctionnelle("Le dossier '" + pathToFeatures + "' n'existe pas.");
            }

            if (!File.Exists(pathToDll))
            {
                throw new ExceptionFonctionnelle("Le fichier '" + pathToDll + "' n'existe pas.");
            }

            nomProjet = nomProjet.Trim();
            if (string.IsNullOrEmpty(nomProjet))
            {
                throw new ExceptionFonctionnelle("Le nom du projet est vide.");
            }

            using (SpecflowEntities entities = new SpecflowEntities())
            {
                Projet projet = entities.Projets.FirstOrDefault(p => p.Nom == nomProjet && p.Version == versionProjet);
                if (projet == null)
                {
                    projet = new Projet() { Id = Guid.NewGuid(), Nom = nomProjet, Version = versionProjet, PathToFeatures = pathToFeatures, PathToTestDll = pathToDll, PathToTestSettings = pathToTestSetting };
                    entities.Projets.Add(projet);
                }

                List<Fonctionnalite> fonctionnalites = GetResult(projet, output);
                
                if (fonctionnalites == null)
                {
                    output.WriteLine("- Echec ! :(");
                    return;
                }

                output.WriteLine("- Purge des fonctionnalités");
                foreach (Fonctionnalite feature in projet.Fonctionnalites.ToList())
                {
                    entities.Fonctionnalites.Remove(feature);
                }
                entities.SaveChanges();

                output.WriteLine("- Ajout des fonctionnalités");
                fonctionnalites.ForEach(f => projet.Fonctionnalites.Add(f));
                entities.SaveChanges();

                output.WriteLine("- Réussi :)");
            }
        }

        public static void Analyser(Guid projectId, TextWriter output)
        {
            using (SpecflowEntities entities = new SpecflowEntities())
            {
                Projet projet = entities.Projets.FirstOrDefault(p => p.Id == projectId);
                if (projet == null)
                {
                    throw new ExceptionTechnique("Aucun projet n'a pour identifiant : " + projectId);
                }

                List<Fonctionnalite> fonctionnalites = GetResult(projet, output);

                if (fonctionnalites == null)
                {
                    output.WriteLine("- Echec ! :(");
                    return;
                }

                output.WriteLine("- Purge des fonctionnalités");
                foreach (Fonctionnalite feature in projet.Fonctionnalites.ToList())
                {
                    entities.Fonctionnalites.Remove(feature);
                }
                entities.SaveChanges();

                output.WriteLine("- Ajout des fonctionnalités");
                fonctionnalites.ForEach(f => projet.Fonctionnalites.Add(f));
                entities.SaveChanges();

                output.WriteLine("- Réussi :)");
            }
        }
        
        public static List<Fonctionnalite> GetResult(Projet projet, TextWriter output)
        {
            DirectoryInfo dirProjectTest = new DirectoryInfo(projet.PathToFeatures);

            output.WriteLine("- On parcourt les fichier *.feature et on extrait les lignes");
            List<Fonctionnalite> features = new List<Fonctionnalite>();
            foreach (FileInfo file in dirProjectTest.GetFiles("*.feature", SearchOption.AllDirectories))
            {
                features.Add(SpecflowParser.Parse(file.FullName));
            }

            string tempFile = Path.Combine(Path.GetTempPath(), projet.Nom + "_" + projet.Version + "_" + DateTime.Now.ToString("yyyyMMddhhmmss") + ".trx");

            output.WriteLine("- On exécute le test");
            MsTestRunnerBL.RunTests(projet.PathToTestDll, projet.PathToTestSettings, tempFile, output);

            output.WriteLine("- On extrait les résultats enregistrés sous " + tempFile);
            string trxContent = string.Empty;
            using (StreamReader sr = new StreamReader(tempFile))
            {
                trxContent = sr.ReadToEnd();
            }

            output.WriteLine("- On nettoie le fichier de résultats");
            File.Delete(tempFile);

            if (string.IsNullOrEmpty(trxContent))
            {
                output.WriteLine("- Le fichier de résultats est vide !");
                return null;
            }

            output.WriteLine("- On parse le fichier des résultats et on associe chaque test au scénario");
            MsTestResultParser msTest = new MsTestResultParser(XDocument.Parse(trxContent));
            foreach (Fonctionnalite feature in features)
            {
                foreach (Scenario scenario in feature.Scenarios)
                {
                    scenario.TestInfos = new List<TestInfo> { msTest.GetScenarioResult(scenario) };
                }
                feature.TestResult = MsTestResultParser.Merge(feature.Scenarios.SelectMany(s => s.TestInfos)).ToString();
            }

            return features;
        }
    }
}
