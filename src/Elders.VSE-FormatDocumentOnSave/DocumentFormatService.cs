using System;
using EnvDTE;

namespace Elders.VSE_FormatDocumentOnSave
{
    public class DocumentFormatService
    {
        readonly DTE dte;
        readonly Func<ExtensionsCfg> getExtensionCfg;
        readonly IDocumentFormatter formatter;

        public DocumentFormatService(DTE dte, Func<ExtensionsCfg> getExtensionCfg)
        {
            this.dte = dte;
            this.getExtensionCfg = getExtensionCfg;

            var formatCmd = new VisualStudioCommand("Edit.FormatDocument");
            formatter = new VisualStudioCommandFormatter(dte, formatCmd);
        }

        public void FormatDocument(Document doc)
        {
            try
            {
                var cfg = getExtensionCfg();
                var filter = new AllowDenyDocumentFilter(cfg.Allowed.Split(' '), cfg.Denied.Split(' '));

                formatter.Format(doc, filter);
            }
            catch (Exception) { }   // Do not do anything here on purpose.
        }
    }
}