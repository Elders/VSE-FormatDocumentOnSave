using Microsoft.VisualStudio.Shell;
using System.Collections.Generic;
using System.ComponentModel;

namespace Elders.VSE_FormatDocumentOnSave.Configurations
{
    public class VisualStudioConfiguration : DialogPage, IConfiguration
    {
        [Category("Format Document On Save")]
        [DisplayName("Enable")]
        [Description("Enable document formating on save")]
        public bool IsEnable { get; set; } = true;

        [Category("Format Document On Save")]
        [DisplayName("Allowed extensions or filenames")]
        [Description("Space separated list. For example: [.cs .html .cshtml .vb .h .cpp]. The file extension (.cs) or filename (somefile.cs) needs to match an entry in the list to get formatted. The allow list is checked prior to the denie list.")]
        public string Allowed { get; set; } = ".*";

        [Category("Format Document On Save")]
        [DisplayName("Denied extensions or filenames")]
        [Description("Space separated list. For example: [resource.h]. The file extension (.cs) or filename (somefile.cs) needs to match an entry in the list to NOT get formatted")]
        public string Denied { get; set; } = "";

        [Category("Format Document On Save")]
        [DisplayName("Commands")]
        [Description("Space separated list. The Visual Studio command to execute. Defaults to VS command [Edit.FormatDocument]")]
        public string Commands { get; set; } = "Edit.FormatDocument";

        [Category("Format Document On Save")]
        [DisplayName("Enable in Debug")]
        [Description("By default the plugin is disabled in debug mode. You could explicitly configure to have the extension enabled while in a debug session")]
        public bool EnableInDebug { get; set; } = false;

        IEnumerable<string> IConfiguration.Allowed => Allowed.Split(' ');

        IEnumerable<string> IConfiguration.Denied => Denied.Split(' ');
    }
}
