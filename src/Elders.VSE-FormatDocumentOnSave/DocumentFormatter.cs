using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using EnvDTE;
using Microsoft.VisualStudio.TextManager.Interop;

namespace Elders.VSE_FormatDocumentOnSave
{
    internal class DocumentFormatter
    {
        private readonly IVsTextManager txtMngr;
        private readonly DTE dte;

        public DocumentFormatter(IVsTextManager txtMngr, DTE dte)
        {
            this.txtMngr = txtMngr;
            this.dte = dte;
        }

        public void FormatCurrentActiveDocument()
        {
            try
            {
                bool useManualSelection = dte.ActiveDocument.ProjectItem.Name.EndsWith(".cshtml", true, CultureInfo.InvariantCulture);

                if (dte.ActiveWindow.Kind == "Document")
                {
                    if (useManualSelection)
                        FormatCSHTML();
                    else
                        dte.ExecuteCommand("Edit.FormatDocument", string.Empty);
                }
            }
            catch (Exception) { }
        }

        private void FormatCSHTML()
        {
            IVsTextView textViewCurrent;
            txtMngr.GetActiveView(1, null, out textViewCurrent);    // Gets the TextView (TextEditor) for the current active document
            int a, b, c, verticalScrollPosition;
            textViewCurrent.GetScrollInfo(1, out a, out b, out c, out verticalScrollPosition);

            dynamic selection = dte.ActiveDocument.Selection;
            int line = selection.CurrentLine;
            int lineLength = selection.ActivePoint.LineLength;
            int col = selection.CurrentColumn;

            dte.ExecuteCommand("Edit.FormatDocument", string.Empty);

            if (!selection.IsEmpty)
                selection.Cancel();

            selection.GoToLine(line);
            int offset = col - (lineLength - selection.ActivePoint.LineLength);
            selection.MoveToLineAndOffset(line, offset, false);

            textViewCurrent.SetScrollPosition(1, verticalScrollPosition);
        }

        public void FormatDocuments(IEnumerable<Document> documents)
        {
            var currentDoc = dte.ActiveDocument;
            foreach (var doc in documents)
            {
                doc.Activate();
                FormatCurrentActiveDocument();
            }
            currentDoc.Activate();
        }
        public void FormatNonSavedDocuments()
        {
            FormatDocuments(GetNonSavedDocuments());
        }

        IEnumerable<Document> GetNonSavedDocuments()
        {
            return dte.Documents.OfType<Document>().Where(document => !document.Saved);
        }
    }
}