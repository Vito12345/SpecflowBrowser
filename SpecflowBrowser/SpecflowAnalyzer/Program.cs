namespace SpecflowAnalyzer
{
    using System;
    using System.Linq;
    using BL;
    using Bytel.Cora.Socle.Exception;
    using Bytel.Cora.Socle.Journalisation;
    //using Microsoft.Owin.Hosting;

    public class Program
    {
        //static void Main(string[] args)
        //{
        //    string url = "http://localhost:8089";
        //    using (WebApp.Start(url))
        //    {
        //        Console.WriteLine("Server running on {0}", url);

        //        Console.WriteLine("-- Press <Enter> to quit --");
        //        Console.ReadLine();
        //    }
        //}

        static void Main(string[] args)
        {
            if (!args.Any())
            {
                ShowHelp();
                return;
            }

            string argProjectName = args.FirstOrDefault(a => a.StartsWith("-projectName="));
            string argProjectVersion = args.FirstOrDefault(a => a.StartsWith("-projectVersion="));
            string argProjectFolder = args.FirstOrDefault(a => a.StartsWith("-projectFolder="));
            string argMsTestDll = args.FirstOrDefault(a => a.StartsWith("-testDll="));
            string argMsTestSetting = args.FirstOrDefault(a => a.StartsWith("-testSetting="));

            if (string.IsNullOrEmpty(argProjectName) ||
                string.IsNullOrEmpty(argProjectVersion) ||
                string.IsNullOrEmpty(argProjectFolder) ||
                string.IsNullOrEmpty(argMsTestDll))
            {
                ShowHelp();
                return;
            }
            string nomProjet = argProjectName.Split('=').Last();
            string versionProjet = argProjectVersion.Split('=').Last();
            string folderProjet = argProjectFolder.Split('=').Last();
            string dllTestFile = argMsTestDll.Split('=').Last();
            string testSetting = string.IsNullOrEmpty(argMsTestSetting) ? string.Empty : argMsTestSetting.Split('=').Last();

            Console.WriteLine("--------------------------------------------------------------");
            Console.WriteLine("-> Nom projet : " + nomProjet);
            Console.WriteLine("-> Version projet : " + versionProjet);
            Console.WriteLine("-> Path vers *.features : " + folderProjet);
            Console.WriteLine("-> Path vers fichier DLL : " + dllTestFile);
            Console.WriteLine("-> Path vers test settings : " + testSetting);
            Console.WriteLine("--------------------------------------------------------------\n");

            Console.WriteLine("Traitement en cours...");
            try
            {
                MainBL.Analyser(
                    nomProjet,
                    versionProjet,
                    folderProjet,
                    dllTestFile,
                    testSetting,
                    Console.Out);
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
