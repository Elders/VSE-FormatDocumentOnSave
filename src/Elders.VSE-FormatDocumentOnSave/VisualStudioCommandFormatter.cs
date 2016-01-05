using System;
using EnvDTE;

namespace Elders.VSE_FormatDocumentOnSave
{
    public class VisualStudioCommandFormatter : IDocumentFormatter
    {
        readonly VisualStudioCommand cmd;
        readonly DTE dte;

        public VisualStudioCommandFormatter(DTE dte, VisualStudioCommand cmd)
        {
            if (ReferenceEquals(null, dte)) throw new ArgumentNullException(nameof(dte));
            if (ReferenceEquals(null, cmd)) throw new ArgumentNullException(nameof(cmd));

            this.dte = dte;
            this.cmd = cmd;
        }

        public void Format(Document document, IDocumentFilter filter)
        {
            var currentDoc = dte.ActiveDocument;

            document.Activate();

            if (dte.ActiveWindow.Kind == "Document")
            {
                if (filter.IsAllowed(document))
                    dte.ExecuteCommand(cmd.Command, cmd.Arguments);
            }

            currentDoc.Activate();
        }
    }
}
