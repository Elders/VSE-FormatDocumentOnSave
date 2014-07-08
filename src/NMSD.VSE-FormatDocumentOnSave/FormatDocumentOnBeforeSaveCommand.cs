using EnvDTE;
using Microsoft.VisualStudio;

namespace NMSD.VSE_FormatDocumentOnSave
{
    internal class FormatDocumentOnBeforeSaveCommand
    {
        private static bool shouldRegisterFormatDocumentOnSave = true;
        private readonly DocumentFormatter _documentFormatter;

        public FormatDocumentOnBeforeSaveCommand(DTE dte, DocumentFormatter documentFormatter)
        {
            _documentFormatter = documentFormatter;
            if (shouldRegisterFormatDocumentOnSave)
            {
                var commandEvents = dte.Events.CommandEvents;
                commandEvents.BeforeExecute += commandEvents_BeforeExecute;

                shouldRegisterFormatDocumentOnSave = false;
            }
        }

        void commandEvents_BeforeExecute(string Guid, int ID, object CustomIn, object CustomOut, ref bool CancelDefault)
        {
            FormatDocument(Guid, (uint)ID);
        }

        void FormatDocument(string pguidCmdGroup, uint nCmdID)
        {
            if (pguidCmdGroup != VSConstants.CMDSETID.StandardCommandSet97_string)
                return;

            switch ((VSConstants.VSStd97CmdID)nCmdID)
            {
                case VSConstants.VSStd97CmdID.Save:
                case VSConstants.VSStd97CmdID.SaveProjectItem:
                case VSConstants.VSStd97CmdID.SaveAs:
                case VSConstants.VSStd97CmdID.SaveProjectItemAs:
                    _documentFormatter.FormatCurrentActiveDocument();
                    break;
                case VSConstants.VSStd97CmdID.SaveSolution:
                    _documentFormatter.FormatNonSavedDocuments();
                    break;
            }
        }
    }
}
