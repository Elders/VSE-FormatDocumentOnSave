using System;
using EnvDTE;

namespace Elders.VSE_FormatDocumentOnSave
{
    public class DocumentFormatService
    {
        readonly DTE dte;

        public DocumentFormatService(DTE dte)
        {
            this.dte = dte;
        }

        public void FormatDocument(Document doc)
        {
            try
            {
                var formatCmd = new VisualStudioCommand("Edit.FormatDocument");
                var formatter = new VisualStudioCommandFormatter(dte, formatCmd);
                var filter = new AllowDenyDocumentFilter(new[] { ".cs" }, new[] { ".cshtml", ".cs" });

                formatter.Format(doc, filter);
            }
            catch (Exception) { }   // Do not do anything here on purpose.
        }
    }
}