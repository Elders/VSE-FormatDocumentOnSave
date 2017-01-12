using System;
using EnvDTE;

namespace Elders.VSE_FormatDocumentOnSave
{
    public class DocumentFormatService
    {
        readonly DTE dte;
        readonly Func<GeneralCfg> getGeneralCfg;
        readonly IDocumentFormatter formatter;

        public DocumentFormatService(DTE dte, Func<GeneralCfg> getGeneralCfg)
        {
            this.dte = dte;
            this.getGeneralCfg = getGeneralCfg;

            formatter = new VisualStudioCommandFormatter(dte);
        }

        public void FormatDocument(Document doc)
        {
            try
            {
                var cfg = getGeneralCfg();
                var filter = new AllowDenyDocumentFilter(cfg.Allowed.Split(' '), cfg.Denied.Split(' '));

                formatter.Format(doc, filter, cfg.Command);
            }
            catch (Exception) { }   // Do not do anything here on purpose.
        }
    }
}