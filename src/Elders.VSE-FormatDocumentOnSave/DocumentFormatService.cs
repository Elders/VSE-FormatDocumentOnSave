using System;
using Elders.VSE_FormatDocumentOnSave.Configurations;
using EnvDTE;

namespace Elders.VSE_FormatDocumentOnSave
{
    public class DocumentFormatService
    {
        readonly DTE dte;
        readonly Func<Document, IConfiguration> getGeneralCfg;
        readonly IDocumentFormatter formatter;

        public DocumentFormatService(DTE dte, Func<Document, IConfiguration> getGeneralCfg)
        {
            this.dte = dte;
            this.getGeneralCfg = getGeneralCfg;

            formatter = new VisualStudioCommandFormatter(dte);
        }

        public void FormatDocument(Document doc)
        {
            try
            {
                var cfg = getGeneralCfg(doc);
                var filter = new AllowDenyDocumentFilter(cfg.Allowed, cfg.Denied);

                formatter.Format(doc, filter, cfg.Command);
            }
            catch (Exception) { }   // Do not do anything here on purpose.
        }
    }
}