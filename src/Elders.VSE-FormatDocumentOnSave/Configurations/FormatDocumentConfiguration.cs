using EnvDTE;
using System.Collections.Generic;
using System.IO;

namespace Elders.VSE_FormatDocumentOnSave.Configurations
{
    public class FormatDocumentConfiguration : IConfiguration
    {
        private readonly IConfiguration configuration;

        public FormatDocumentConfiguration(Document doc, IConfiguration defaultCfg)
        {
            configuration = defaultCfg;

            FileInfo cfgFile = new FileInfo(Path.Combine(doc.Path, ".formatconfig"));
            var dir = new DirectoryInfo(doc.Path);
            while (dir.Parent != null)
            {
                if (cfgFile.Exists) break;

                var configs = dir.GetFiles(".formatconfig");
                if (configs.Length > 0)
                {
                    cfgFile = configs[0];
                    break;
                }
                else
                {
                    dir = dir.Parent;
                }
            }

            if (cfgFile.Exists)
                configuration = new EditorConfigConfiguration(cfgFile.FullName);
        }

        public IEnumerable<string> Allowed => configuration.Allowed;

        public IEnumerable<string> Denied => configuration.Denied;

        public string Command => configuration.Command;
    }
}
