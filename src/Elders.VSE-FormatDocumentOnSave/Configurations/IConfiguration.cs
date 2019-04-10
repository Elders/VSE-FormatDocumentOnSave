using System.Collections.Generic;

namespace Elders.VSE_FormatDocumentOnSave.Configurations
{
    public interface IConfiguration
    {
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
    }
}
