using EnvDTE;

namespace Elders.VSE_FormatDocumentOnSave
{
    public interface IDocumentFilter
    {
        bool IsAllowed(Document document);
    }
}