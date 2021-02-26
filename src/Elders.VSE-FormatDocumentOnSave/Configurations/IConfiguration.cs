using System.Collections.Generic;

namespace Elders.VSE_FormatDocumentOnSave.Configurations
{
    public interface IConfiguration
    {
        /// <summary>
        /// Enable or disable formating on save
        /// </summary>
        bool IsEnable { get; }

        /// <summary>
        /// Allowed extensions. For example: .cs .html .cshtml .vb
        /// </summary>
        IEnumerable<string> Allowed { get; }

        /// <summary>
        /// Denied filed extentions. For example: .cs .html .cshtml .vb
        /// </summary>
        IEnumerable<string> Denied { get; }

        /// <summary>
        /// The Visual Studio commands to execute. Defaults to format document (Edit.FormatDocument)
        /// </summary>
        string Commands { get; }

        /// <summary>
        /// By default the plugin is disabled in debug mode. You could explicitly configure to have the extension enabled while in a debug session.
        /// </summary>
        bool EnableInDebug { get; }
    }
}
