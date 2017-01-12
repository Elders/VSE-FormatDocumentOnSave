using System;
using EnvDTE;

namespace Elders.VSE_FormatDocumentOnSave
{
    public class VisualStudioCommandFormatter : IDocumentFormatter
    {
        const string defaultCommand = "Edit.FormatDocument";

        readonly DTE dte;

        public VisualStudioCommandFormatter(DTE dte)
        {
            if (ReferenceEquals(null, dte)) throw new ArgumentNullException(nameof(dte));

            this.dte = dte;
        }

        public void Format(Document document, IDocumentFilter filter, string command)
        {
            var currentDoc = dte.ActiveDocument;

            document.Activate();

            if (dte.ActiveWindow.Kind == "Document")
            {
                if (string.IsNullOrEmpty(command))
                {
                    command = defaultCommand;
                }
                if (filter.IsAllowed(document))
                    dte.ExecuteCommand(command, string.Empty);
            }

            currentDoc.Activate();
        }
    }
}
