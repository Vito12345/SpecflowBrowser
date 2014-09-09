namespace SpecflowTfs
{
    using System;
    using System.Collections.Generic;
    using System.Configuration;
    using System.IO;
    using System.Linq;
    using System.Text;
    using BL;
    using BL.Tfs;

    public class Traitement
    {
        private string NomProjet { get; set; }
        private string VersionProjet { get; set; }
        private string TfsFolder { get; set; }
        private string TfsUrl { get; set; }

        public Traitement(string nomProjet, string versionProjet, string tfsFolder)
        {
            NomProjet = nomProjet;
            VersionProjet = versionProjet;
            TfsFolder = tfsFolder;
            TfsUrl = ConfigurationManager.AppSettings["TfsUrl"];
        }

        public void Traiter(TextWriter output)
        {
            output.WriteLine("- Connexion à TFS : ");
            TfsManager tfs = new TfsManager(TfsUrl);
            
            output.WriteLine("- Récupération des fichiers Specflow depuis TFS : ");
            string pathToFeatures = Path.Combine(Path.GetTempPath(), "SpecflowTfs_features_" + DateTime.Now.ToString("yyyyMMddhhmmss"));
            Directory.CreateDirectory(pathToFeatures);
            tfs.DownloadFiles(TfsFolder, "feature", pathToFeatures);

            string pathToTestRunConfig = Path.Combine(Path.GetTempPath(), "SpecflowTfs_testRunConfig_" + DateTime.Now.ToString("yyyyMMddhhmmss"));
            Directory.CreateDirectory(pathToTestRunConfig);
            tfs.DownloadFiles(TfsFolder, "testrunconfig", pathToTestRunConfig);

            string pathToTestProject = Path.Combine(Path.GetTempPath(), "SpecflowTfs_testProject_" + DateTime.Now.ToString("yyyyMMddhhmmss"));
            Directory.CreateDirectory(pathToTestProject);
            tfs.DownloadFiles(TfsFolder, "testrunconfig", pathToTestProject);

            MainBL.Analyser(NomProjet,
                VersionProjet,
                pathToFeatures,
                string.Empty,
                string.Empty,
                output);
        }
    }
}
