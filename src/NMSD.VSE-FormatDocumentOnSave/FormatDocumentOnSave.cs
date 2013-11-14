using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using EnvDTE;

namespace NMSD.VSE_FormatDocumentOnSave
{
    internal class FormatDocumentOnSave
    {
        private CommandEvents commandEvents;

        private DTE dte;

        private static bool shouldRegisterFormatDocumentOnSave = true;

        public FormatDocumentOnSave(DTE dte)
        {
            this.dte = dte;
            if (shouldRegisterFormatDocumentOnSave)
            {
                commandEvents = dte.Events.CommandEvents;
                commandEvents.BeforeExecute += commandEvents_BeforeExecute;

                shouldRegisterFormatDocumentOnSave = false;
            }
        }

        void commandEvents_BeforeExecute(string Guid, int ID, object CustomIn, object CustomOut, ref bool CancelDefault)
        {
            FormatDocument(Guid, (uint)ID);
        }

        void FormatCurrentActiveDocument()
        {
            dynamic selection = (dynamic)dte.ActiveDocument.Selection;
            int line = selection.CurrentLine;
            int col = selection.CurrentColumn;

            if (dte.ActiveWindow.Kind == "Document")
                dte.ExecuteCommand("Edit.FormatDocument", string.Empty);

            if (!selection.IsEmpty)
                selection.Cancel();

            selection.MoveToLineAndOffset(line, col, false);
        }

        void FormatDocument(string pguidCmdGroup, uint nCmdID)
        {
            if (pguidCmdGroup == Microsoft.VisualStudio.VSConstants.CMDSETID.StandardCommandSet97_string)
            {
                switch (nCmdID)
                {
                    case (uint)Microsoft.VisualStudio.VSConstants.VSStd97CmdID.SaveProjectItem:
                        {
                            FormatCurrentActiveDocument();
                        }
                        break;
                    case (uint)Microsoft.VisualStudio.VSConstants.VSStd97CmdID.SaveSolution:
                        {
                            var currentDoc = dte.ActiveDocument;
                            foreach (var doc in GetNonSavedDocuments())
                            {
                                doc.Activate();
                                FormatCurrentActiveDocument();
                            }
                            currentDoc.Activate();
                        }
                        break;
                    default:
                        break;
                }
            }

        }

        IEnumerable<Document> GetNonSavedDocuments()
        {
            return Enumerable.Where<Document>(Enumerable.OfType<Document>((IEnumerable)dte.Documents), (Func<Document, bool>)(document => !document.Saved));
        }

    }
}
