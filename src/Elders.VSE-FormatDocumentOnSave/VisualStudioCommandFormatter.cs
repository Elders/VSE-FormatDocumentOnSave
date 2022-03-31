using System;
using EnvDTE;
using Microsoft.VisualStudio.Shell;

namespace Elders.VSE_FormatDocumentOnSave
{
    public class VisualStudioCommandFormatter : IDocumentFormatter
    {
        const string defaultCommand = "Edit.FormatDocument";

        readonly DTE dte;

        public VisualStudioCommandFormatter(DTE dte)
        {
            this.dte = dte ?? throw new ArgumentNullException(nameof(dte));
        }

        public void Format(Document document, IDocumentFilter filter, string command)
        {
            ThreadHelper.ThrowIfNotOnUIThread();

            var currentDoc = dte.ActiveDocument;

            document.Activate();

            if (dte.ActiveWindow.Kind == "Document" && filter.IsAllowed(document))
            {
                if (string.IsNullOrWhiteSpace(command))
                {
                    command = defaultCommand;
                }

                dte.ExecuteCommand(command, string.Empty);
            }

            currentDoc.Activate();
        }
    }
}
