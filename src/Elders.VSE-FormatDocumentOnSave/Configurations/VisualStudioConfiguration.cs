using Microsoft.VisualStudio.Shell;
using System.Collections.Generic;
using System.ComponentModel;

namespace Elders.VSE_FormatDocumentOnSave.Configurations
{
    public class VisualStudioConfiguration : DialogPage, IConfiguration
    {
        [Category("Format Document On Save")]
        [DisplayName("Allowed extensions")]
        [Description("Space separated list. For example: [.cs .html .cshtml .vb] Overrides extensions listed in Denied section. When you use [.*] the extensions listed in Denied section will be ignored. Empty value respects all extensions listed in Denied section.")]
        public string Allowed { get; set; } = ".*";

        [Category("Format Document On Save")]
        [DisplayName("Denied extensions")]
        [Description("Space separated list. For example: [.cs .html .cshtml .vb]")]
        public string Denied { get; set; } = "";

        [Category("Format Document On Save")]
        [DisplayName("Command")]
        [Description("Space separated list. The Visual Studio command to execute. Defaults to VS command [Edit.FormatDocument]")]
        public string Command { get; set; } = "Edit.FormatDocument";

        IEnumerable<string> IConfiguration.Allowed => Allowed.Split(' ');

        IEnumerable<string> IConfiguration.Denied => Denied.Split(' ');
    }
}
