namespace SpecflowTfs
{
    using System;
    using System.Linq;
    using Bytel.Cora.Socle.Exception;
    using Bytel.Cora.Socle.Journalisation;

    class Program
    {
        static void Main(string[] args)
        {
            if (!args.Any())
            {
                ShowHelp();
                return;
            }
            
            string argProjectName = args.FirstOrDefault(a => a.StartsWith("-projectName="));
            string argProjectVersion = args.FirstOrDefault(a => a.StartsWith("-projectVersion="));
            string argTfsFolder = args.FirstOrDefault(a => a.StartsWith("-tfsFolder="));
            
            if (string.IsNullOrEmpty(argProjectName) ||
                string.IsNullOrEmpty(argProjectVersion) ||
                string.IsNullOrEmpty(argTfsFolder))
            {
                ShowHelp();
                return;
            }

            string nomProjet = argProjectName.Split('=').Last();
            string versionProjet = argProjectVersion.Split('=').Last();
            string tfsFolder = argTfsFolder.Split('=').Last();

            Console.WriteLine("--------------------------------------------------------------");
            Console.WriteLine("-> Nom projet : " + nomProjet);
            Console.WriteLine("-> Version projet : " + versionProjet);
            Console.WriteLine("-> Path vers dossier TFS : " + tfsFolder);
            Console.WriteLine("--------------------------------------------------------------\n");

            Console.WriteLine("Traitement en cours...");
            try
            {
                Traitement traitement = new Traitement(nomProjet, versionProjet, tfsFolder);
                traitement.Traiter(Console.Out);
            }
            catch (ExceptionFonctionnelle ex)
            {
                Console.WriteLine(ex.Message);
                Journal.EcrireTraceErreur(ex.Message, ex);
            }
            catch (ExceptionTechnique ex)
            {
                Console.WriteLine(ex.Message);
                Journal.EcrireTraceCritique(ex.Message, ex);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Journal.EcrireTraceCritique(ex.Message, ex);
            }

            Console.WriteLine("\n\nAppuyer sur ENTREE pour fermer le programme.");
            Console.ReadLine();
        }

        static void ShowHelp()
        {
            Console.WriteLine("SpecflowAnalyzer\n");
            Console.WriteLine("Les 4 premiers arguments sont obligatoires :");
            Console.WriteLine("-projectName => Nom du projet (s'il n'existe pas, il sera créé)");
            Console.WriteLine("-projectVersion => Version du projet (s'il n'existe pas, il sera créé)");
            Console.WriteLine("-projectFolder => Chemin absolu ou relatif du dossier contenant les features");
            Console.WriteLine("-testDll => Chemin vers le fichier *.dll qui sera testé");
            Console.WriteLine("-testSetting => Chemin vers le fichier *.testsetting qui sera utilisé pour les tests");

            Console.WriteLine("\n\nAppuyer sur ENTREE pour fermer le programme.");
            Console.ReadLine();
        }
    }
}
