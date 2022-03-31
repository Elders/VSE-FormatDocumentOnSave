using System.Linq;
using System.Threading.Tasks;
using EnvDTE;
using Microsoft.VisualStudio;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;

namespace Elders.VSE_FormatDocumentOnSave
{
    internal class FormatDocumentOnBeforeSave : IVsRunningDocTableEvents3
    {
        private readonly DTE _dte;
        private readonly RunningDocumentTable _runningDocumentTable;
        private readonly DocumentFormatService _documentFormatter;

        public FormatDocumentOnBeforeSave(DTE dte, RunningDocumentTable runningDocumentTable, DocumentFormatService documentFormatter)
        {
            _runningDocumentTable = runningDocumentTable;
            _documentFormatter = documentFormatter;
            _dte = dte;
        }

        public int OnAfterFirstDocumentLock(uint docCookie, uint dwRDTLockType, uint dwReadLocksRemaining, uint dwEditLocksRemaining)
        {
            return VSConstants.S_OK;
        }

        public int OnBeforeLastDocumentUnlock(uint docCookie, uint dwRDTLockType, uint dwReadLocksRemaining, uint dwEditLocksRemaining)
        {
            return VSConstants.S_OK;
        }

        public int OnAfterSave(uint docCookie)
        {
            return VSConstants.S_OK;
        }

        public int OnAfterAttributeChange(uint docCookie, uint grfAttribs)
        {
            return VSConstants.S_OK;
        }

        public int OnBeforeDocumentWindowShow(uint docCookie, int fFirstShow, IVsWindowFrame pFrame)
        {
            return VSConstants.S_OK;
        }

        public int OnAfterDocumentWindowHide(uint docCookie, IVsWindowFrame pFrame)
        {
            return VSConstants.S_OK;
        }

        public int OnAfterAttributeChangeEx(uint docCookie, uint grfAttribs, IVsHierarchy pHierOld, uint itemidOld, string pszMkDocumentOld, IVsHierarchy pHierNew, uint itemidNew, string pszMkDocumentNew)
        {
            return VSConstants.S_OK;
        }

        public int OnBeforeSave(uint docCookie)
        {
            ThreadHelper.ThrowIfNotOnUIThread();

            var document = FindDocument(docCookie);

            if (document == null)
                return VSConstants.S_OK;

            _documentFormatter.FormatDocument(document);

            return VSConstants.S_OK;
        }

        private Document FindDocument(uint docCookie)
        {
            ThreadHelper.ThrowIfNotOnUIThread();

            var documentInfo = _runningDocumentTable.GetDocumentInfo(docCookie);
            var documentPath = documentInfo.Moniker;

            return _dte.Documents.Cast<Document>().FirstOrDefault(doc =>
            {
                ThreadHelper.ThrowIfNotOnUIThread();
                return doc.FullName == documentPath;
            });
        }

        public void OnAfterDocumentLockCountChanged(uint docCookie, uint dwRDTLockType, uint dwOldLockCount, uint dwNewLockCount)
        {

        }
    }
}