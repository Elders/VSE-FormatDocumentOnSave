using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.IO;
using EnvDTE;
using Microsoft.VisualStudio.Shell;

namespace Elders.VSE_FormatDocumentOnSave
{

    public class AllowDenyDocumentFilter : IDocumentFilter
    {
        private readonly Func<Document, bool> isAllowed = doc => true;

        /// <summary>
        /// Everything is allowed when this ctor is used.
        /// </summary>
        public AllowDenyDocumentFilter() { }

        public bool MatchesList(IEnumerable<string> list, string filePath)
        {
            string fileName = Path.GetFileName(filePath);
            string extension = Path.GetExtension(filePath);

            foreach (string element in list)
            { 
                if(string.IsNullOrEmpty(element))
                    continue;
                if (element.Equals(".*"))
                    return true;
                if (fileName.EndsWith(element, StringComparison.OrdinalIgnoreCase))
                    return true;
                if (extension.EndsWith(element, StringComparison.OrdinalIgnoreCase))
                    return true;
            }

            return false;
        }
        
        public AllowDenyDocumentFilter(IEnumerable<string> allowedExtensions, IEnumerable<string> deniedExtensions)
        {
            ThreadHelper.ThrowIfNotOnUIThread();

            isAllowed = doc =>
            {
                ThreadHelper.ThrowIfNotOnUIThread();
                if(MatchesList(allowedExtensions, doc.FullName))
                    if(!MatchesList(deniedExtensions, doc.FullName))
                        return true;
                return false;
            };
        }

        public bool IsAllowed(Document document)
        {
            ThreadHelper.ThrowIfNotOnUIThread();
            return IsForbiddenExtension(document) == false && isAllowed(document);
        }

        /// <summary>
        /// We are forcibly denying the extensions bellow for VS2022. There are fundamental changes in visual studio which makes this plugin unusable.
        /// People just do not realize this and they do not read the issues => https://github.com/Elders/VSE-FormatDocumentOnSave/issues/44
        /// For that reason the extension will never work for the specified extensions.
        /// </summary>
        private bool IsForbiddenExtension(Document doc)
        {
            return ForbiddenExtensions.Where(f => { ThreadHelper.ThrowIfNotOnUIThread(); return doc.FullName.EndsWith(f, StringComparison.OrdinalIgnoreCase); }).Any();
        }

        static IEnumerable<string> ForbiddenExtensions => new List<string> { ".razor", ".html", ".xml", ".cshtml" };
    }
}