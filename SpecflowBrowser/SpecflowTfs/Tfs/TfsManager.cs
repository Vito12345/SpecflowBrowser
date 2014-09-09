using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using Microsoft.TeamFoundation.Client;
using Microsoft.TeamFoundation.VersionControl.Client;

namespace BL.Tfs
{
    public class TfsManager
    {
        private TfsTeamProjectCollection TfsCollection;
        private VersionControlServer TfsVersion;

        public TfsManager(string url)
        {
            TfsCollection = TfsTeamProjectCollectionFactory.GetTeamProjectCollection(new Uri(url));
            TfsVersion = TfsCollection.GetService<VersionControlServer>();
        }

        public void DownloadFiles(string tfsProjectFolder, string fileExtension, string destinationFolder)
        {
            List<string> lst = new List<string>();
            ItemSet items = TfsVersion.GetItems(tfsProjectFolder);

            foreach (Item item in items.Items)
            {
                if (item.ItemType == ItemType.Folder)
                {
                    DownloadFiles(item.ServerItem + "/*", fileExtension, destinationFolder);
                }
                else if (item.ServerItem.EndsWith("." + fileExtension))
                {
                    item.DownloadFile(Path.Combine(destinationFolder, item.ServerItem.Split('/').Last()));
                }
            }
        }

        public void DownloadTestProject(string tfsProjectFolder, string destinationFolder)
        {
            ItemSet items = TfsVersion.GetItems(tfsProjectFolder);

            foreach (Item item in items.Items)
            {
                if (item.ItemType == ItemType.Folder)
                {
                    GetTestProjectPath(tfsProjectFolder + "/*");
                }
                else if (item.ServerItem.EndsWith(".csproj"))
                {
                    string xmlContent = string.Empty;
                    using (StreamReader sr = new StreamReader(item.DownloadFile()))
                    {
                        xmlContent = sr.ReadToEnd();
                    }

                    XDocument doc = XDocument.Parse(xmlContent);
                    XElement elPropertyGroup = doc.Root.Element("PropertyGroup");
                    XElement elGuids = elPropertyGroup.Element("ProjectTypeGuids");
                    if (elGuids.Value.Split(';').Contains("{3AC096D0-A1C2-E12C-1390-A8335801FDAB}"))
                    {
                        string path = Path.Combine(destinationFolder, item.ServerItem.Split('/').Last().Replace(".csproj", ""));
                        Directory.CreateDirectory(path);
                        
                    }
                }
            }
        }

        private void GetTestProjectPath(string tfsProjectFolder)
        {
            
        }
    }
}
