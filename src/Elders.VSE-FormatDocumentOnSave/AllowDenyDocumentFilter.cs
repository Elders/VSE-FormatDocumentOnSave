using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using EnvDTE;

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
            return isAllowed(document);
        }
    }
}