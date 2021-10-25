using EditorConfig.Core;
using System.Collections.Generic;

namespace Elders.VSE_FormatDocumentOnSave.Configurations
{
    public sealed class EditorConfigConfiguration : IConfiguration
    {
        private readonly bool enable = true;
        private readonly string allowed = ".*";
        private readonly string denied = "";
        private readonly string command = "Edit.FormatDocument";
        private readonly bool enableInDebug = false;

        public EditorConfigConfiguration(string formatConfigFile)
        {
            var parser = new EditorConfigParser(formatConfigFile);
            FileConfiguration configFile = parser.Parse(formatConfigFile);

            if (configFile.Properties.TryGetValue("enable", out string enableAsString) && bool.TryParse(enableAsString, out bool enableParsed))
                enable = enableParsed;

            if (configFile.Properties.ContainsKey("allowed_extensions"))
                configFile.Properties.TryGetValue("allowed_extensions", out allowed);

            if (configFile.Properties.ContainsKey("denied_extensions"))
                configFile.Properties.TryGetValue("denied_extensions", out denied);

            if (configFile.Properties.ContainsKey("command"))
                configFile.Properties.TryGetValue("command", out command);

            if (configFile.Properties.TryGetValue("enable_in_debug", out string enableInDebugAsString) && bool.TryParse(enableInDebugAsString, out bool enableInDebugParsed))
                enableInDebug = enableInDebugParsed;
        }

        public bool IsEnable => enable;

        IEnumerable<string> IConfiguration.Allowed => allowed.Split(' ');

        IEnumerable<string> IConfiguration.Denied => denied.Split(' ');

        public string Commands => command;

        public bool EnableInDebug => enableInDebug;
    }
}
