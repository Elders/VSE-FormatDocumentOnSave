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
            this.dte = dte ?? throw new ArgumentNullException(nameof(dte));
        }

        public void Format(Document document, IDocumentFilter filter, string command)
        {
            var currentDoc = dte.ActiveDocument;

            document.Activate();

            if (dte.ActiveWindow.Kind == "Document" && filter.IsAllowed(document))
            {
                if (string.IsNullOrWhiteSpace(command))
                    command = defaultCommand;
                else
                    command = command.Trim();

                foreach (string splitCommand in command.Split(' '))
                {
                    if (string.IsNullOrWhiteSpace(splitCommand))
                        continue;
                    string commandName = splitCommand.Trim();
                    dte.ExecuteCommand(commandName, string.Empty);
                }
            }

            currentDoc.Activate();
        }
    }
}
