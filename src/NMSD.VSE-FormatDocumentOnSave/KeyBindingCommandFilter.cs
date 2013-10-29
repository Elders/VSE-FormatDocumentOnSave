using System;
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

        int IOleCommandTarget.Exec(ref Guid pguidCmdGroup, uint nCmdID, uint nCmdexecopt, IntPtr pvaIn, IntPtr pvaOut)
        {
            if (pguidCmdGroup == Guid.Parse(Microsoft.VisualStudio.VSConstants.CMDSETID.StandardCommandSet97_string) && nCmdID == (uint)Microsoft.VisualStudio.VSConstants.VSStd97CmdID.SaveProjectItem)
            {
                dte.ExecuteCommand("Edit.FormatDocument", string.Empty);
            }
            return m_nextTarget.Exec(ref pguidCmdGroup, nCmdID, nCmdexecopt, pvaIn, pvaOut);
        }

        int IOleCommandTarget.QueryStatus(ref Guid pguidCmdGroup, uint cCmds, OLECMD[] prgCmds, IntPtr pCmdText)
        {
            return m_nextTarget.QueryStatus(ref pguidCmdGroup, cCmds, prgCmds, pCmdText);
        }
    }
}