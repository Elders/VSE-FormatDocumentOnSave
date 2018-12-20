using EditorConfig.Core;
using System.Collections.Generic;
using System.Linq;

namespace Elders.VSE_FormatDocumentOnSave.Configurations
{
    public sealed class EditorConfigConfiguration : IConfiguration
    {
        private string allowed = ".*";
        private string denied = "";
        private readonly string command = "Edit.FormatDocument";

        public EditorConfigConfiguration(string formatConfigFile)
        {
            var parser = new EditorConfig.Core.EditorConfigParser(formatConfigFile);
            FileConfiguration configFile = parser.Parse(formatConfigFile).First();

            if (configFile.Properties.ContainsKey("allowed_extensions"))
                configFile.Properties.TryGetValue("allowed_extensions", out allowed);

            if (configFile.Properties.ContainsKey("denied_extensions"))
                configFile.Properties.TryGetValue("denied_extensions", out denied);

            if (configFile.Properties.ContainsKey("command"))
                configFile.Properties.TryGetValue("command", out command);
        }

        IEnumerable<string> IConfiguration.Allowed => allowed.Split(' ');

        IEnumerable<string> IConfiguration.Denied => denied.Split(' ');

        public string Command => command;
    }
}
