using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
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

        public AllowDenyDocumentFilter(IEnumerable<string> allowedExtensions, IEnumerable<string> deniedExtensions)
        {
            ThreadHelper.ThrowIfNotOnUIThread();

            IEnumerable<string> bannedExtensions = deniedExtensions
                .Except(allowedExtensions)
                .Where(ext => ext.Equals(".*") == false && string.IsNullOrEmpty(ext) == false);

            IEnumerable<string> approvedExtensions = allowedExtensions
                .Except(deniedExtensions)
                .Where(ext => ext.Equals(".*") == false && string.IsNullOrEmpty(ext) == false);

            bool areAllExtensionsAllowed = allowedExtensions.Where(ext => ext.Equals(".*")).Any();
            bool areAllExtensionsDenied = deniedExtensions.Where(ext => ext.Equals(".*")).Any();

            isAllowed = doc =>
                areAllExtensionsAllowed ||
                approvedExtensions.Any(ext => doc.FullName.EndsWith(ext, StringComparison.OrdinalIgnoreCase)) ||
                (bannedExtensions.Any(ext => doc.FullName.EndsWith(ext, StringComparison.OrdinalIgnoreCase)) == false && areAllExtensionsDenied == false);
        }

        public bool IsAllowed(Document document)
        {
            return IsForbiddenExtension(document) == false && isAllowed(document);
        }

        /// <summary>
        /// We are forcibly denying the extensions bellow for VS2022. There are fundamental changes in visual studio which makes this plugin unusable.
        /// People just do not realize this and they do not read the issues => https://github.com/Elders/VSE-FormatDocumentOnSave/issues/44
        /// For that reason the extension will never work for the specified extensions.
        /// </summary>
        private bool IsForbiddenExtension(Document doc)
        {
            return ForbiddenExtensions.Where(f => doc.FullName.EndsWith(f, StringComparison.OrdinalIgnoreCase)).Any();
        }

        static IEnumerable<string> ForbiddenExtensions => new List<string> { ".razor", ".html", ".xml", ".cshtml" };
    }
}