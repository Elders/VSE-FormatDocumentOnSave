using EditorConfig.Core;
using System.Collections.Generic;
using System.Linq;

namespace Elders.VSE_FormatDocumentOnSave.Configurations
{
    public sealed class EditorConfigConfiguration : IConfiguration
    {
        private readonly string allowed = ".*";
        private readonly string denied = "";
        private readonly string command = "Edit.FormatDocument";
        private readonly bool enableInDebug = false;

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

            string enableInDebugAsString;
            if (configFile.Properties.ContainsKey("enable_in_debug"))
            {
                configFile.Properties.TryGetValue("enable_in_debug", out enableInDebugAsString);
                bool.TryParse(enableInDebugAsString, out enableInDebug);
            }
        }

        IEnumerable<string> IConfiguration.Allowed => allowed.Split(' ');

        IEnumerable<string> IConfiguration.Denied => denied.Split(' ');

        public string Commands => command;

        public bool EnableInDebug => enableInDebug;
    }
}
