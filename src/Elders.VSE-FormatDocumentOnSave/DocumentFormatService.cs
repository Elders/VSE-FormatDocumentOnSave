using System;
using Elders.VSE_FormatDocumentOnSave.Configurations;
using EnvDTE;

namespace Elders.VSE_FormatDocumentOnSave
{
    public class DocumentFormatService
    {
        readonly Func<Document, IConfiguration> getGeneralCfg;
        readonly IDocumentFormatter formatter;

        public DocumentFormatService(DTE dte, Func<Document, IConfiguration> getGeneralCfg)
        {
            this.getGeneralCfg = getGeneralCfg;

            formatter = new VisualStudioCommandFormatter(dte);
        }

        public void FormatDocument(Document doc)
        {
            try
            {
                var cfg = getGeneralCfg(doc);
                var filter = new AllowDenyDocumentFilter(cfg.Allowed, cfg.Denied);

                foreach (string splitCommand in cfg.Commands.Trim().Split(' '))
                {
                    try
                    {
                        string commandName = splitCommand.Trim();
                        formatter.Format(doc, filter, commandName);
                    }
                    catch (Exception) { }   // may be we can log which command has failed and why
                }
            }
            catch (Exception) { }   // Do not do anything here on purpose.
        }
    }
}