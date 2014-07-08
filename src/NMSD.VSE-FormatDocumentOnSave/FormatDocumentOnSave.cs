using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using EnvDTE;

namespace NMSD.VSE_FormatDocumentOnSave
{
    internal class FormatDocumentOnSave
    {
        private static bool shouldRegisterFormatDocumentOnSave = true;
        private readonly DocumentFormatter _documentFormatter;

        public FormatDocumentOnSave(DTE dte, DocumentFormatter documentFormatter)
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
            if (pguidCmdGroup == Microsoft.VisualStudio.VSConstants.CMDSETID.StandardCommandSet97_string)
            {
                switch (nCmdID)
                {
                    case (uint)Microsoft.VisualStudio.VSConstants.VSStd97CmdID.SaveProjectItem:
                        {
                            _documentFormatter.FormatCurrentActiveDocument();
                        }
                        break;
                    case (uint)Microsoft.VisualStudio.VSConstants.VSStd97CmdID.SaveSolution:
                        {
                            _documentFormatter.FormatNonSavedDocuments();
                        }
                        break;
                    default:
                        break;
                }
            }
        }
    }
}
