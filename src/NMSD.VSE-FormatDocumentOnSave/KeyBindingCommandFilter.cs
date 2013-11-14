using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using EnvDTE;
using Microsoft.VisualStudio.OLE.Interop;

namespace NMSD.VSE_FormatDocumentOnSave
{
    internal class KeyBindingCommandFilter : IOleCommandTarget
    {
        DTE dte;

        internal bool m_added;

        internal IOleCommandTarget m_nextTarget;

        public KeyBindingCommandFilter(DTE dte)
        {
            this.dte = dte;
        }

        void FormatCurrentActiveDocument()
        {
            if (dte.ActiveWindow.Kind == "Document")
                dte.ExecuteCommand("Edit.FormatDocument", string.Empty);
        }

        IEnumerable<Document> GetNonSavedDocuments()
        {
            return Enumerable.Where<Document>(Enumerable.OfType<Document>((IEnumerable)dte.Documents), (Func<Document, bool>)(document => !document.Saved));
        }

        int IOleCommandTarget.Exec(ref Guid pguidCmdGroup, uint nCmdID, uint nCmdexecopt, IntPtr pvaIn, IntPtr pvaOut)
        {
            if (pguidCmdGroup == Guid.Parse(Microsoft.VisualStudio.VSConstants.CMDSETID.StandardCommandSet97_string))
            {
                if (nCmdID == (uint)Microsoft.VisualStudio.VSConstants.VSStd97CmdID.SaveProjectItem)
                {
                    FormatCurrentActiveDocument();
                }
                else if (nCmdID == (uint)Microsoft.VisualStudio.VSConstants.VSStd97CmdID.SaveSolution)
                {
                    var currentDoc = dte.ActiveDocument;
                    foreach (var doc in GetNonSavedDocuments())
                    {
                        doc.Activate();
                        FormatCurrentActiveDocument();
                    }
                    currentDoc.Activate();
                }
            }
            return m_nextTarget.Exec(ref pguidCmdGroup, nCmdID, nCmdexecopt, pvaIn, pvaOut);
        }

        int IOleCommandTarget.QueryStatus(ref Guid pguidCmdGroup, uint cCmds, OLECMD[] prgCmds, IntPtr pCmdText)
        {
            return m_nextTarget.QueryStatus(ref pguidCmdGroup, cCmds, prgCmds, pCmdText);
        }

    }
}