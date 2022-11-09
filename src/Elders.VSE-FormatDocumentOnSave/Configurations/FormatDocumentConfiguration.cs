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
            Microsoft.VisualStudio.Shell.ThreadHelper.ThrowIfNotOnUIThread();

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

        public bool IsEnable => configuration.IsEnable;

        public IEnumerable<string> Allowed => configuration.Allowed;

        public IEnumerable<string> Denied => configuration.Denied;

        public string Commands => configuration.Commands;

        public bool EnableInDebug => configuration.EnableInDebug;
    }
}
