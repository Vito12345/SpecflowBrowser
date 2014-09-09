namespace BL
{
    using System;
    using System.Configuration;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;
    using System.Text;
    using System.IO;

    public class MsTestRunnerBL
    {
        public static void RunTests(string pathDll, string pathToTestSetting, string resultFileName, TextWriter output)
        {
            FileInfo dll = new FileInfo(pathDll);
            
            Process p = new Process();
            p.StartInfo.RedirectStandardError = true;
            p.StartInfo.RedirectStandardOutput = true;
            p.StartInfo.UseShellExecute = false;
            p.StartInfo.CreateNoWindow = true;
            p.StartInfo.WorkingDirectory = dll.Directory.FullName;
            p.StartInfo.FileName = ConfigurationManager.AppSettings["PathToMsTest"];
            p.StartInfo.Arguments = string.Format("/testcontainer:\"{0}\" /resultsfile:\"{1}\"", dll.Name, resultFileName);
            
            if (!string.IsNullOrEmpty(pathToTestSetting))
            {
                if (pathToTestSetting.EndsWith(".testsettings"))
                {
                    p.StartInfo.Arguments += " /testsettings:\"" + pathToTestSetting + "\"";
                }
                else if (pathToTestSetting.ToLower().EndsWith(".testrunconfig"))
                {
                    p.StartInfo.Arguments += " /runconfig:\"" + pathToTestSetting + "\"";
                }
            }

            p.OutputDataReceived += new DataReceivedEventHandler((s, e) => { output.WriteLine(e.Data); });
            p.ErrorDataReceived += new DataReceivedEventHandler((s, e) => { output.WriteLine(e.Data); });

            p.Start();
            p.BeginOutputReadLine();
            p.WaitForExit();
        }
    }
}
