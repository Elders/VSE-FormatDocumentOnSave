using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using EnvDTE;

namespace Elders.VSE_FormatDocumentOnSave
{

    public class AllowDenyDocumentFilter : IDocumentFilter
    {
        Func<Document, bool> isAllowed = doc => true;

        /// <summary>
        /// Everything is allowed when this ctor is used.
        /// </summary>
        public AllowDenyDocumentFilter() { }

        public AllowDenyDocumentFilter(IEnumerable<string> allowedExtensions, IEnumerable<string> deniedExtensions)
        {
            allowedExtensions = allowedExtensions.Where(x => x.Equals(".*") == false && string.IsNullOrEmpty(x) == false);
            deniedExtensions = deniedExtensions.Where(x => x.Equals(".*") == false && string.IsNullOrEmpty(x) == false);

            if (allowedExtensions.Count() > 0)
            {
                isAllowed = doc => allowedExtensions.Any(ext => doc.FullName.EndsWith(ext, true, CultureInfo.InvariantCulture));
            }
            else if (deniedExtensions.Count() > 0)
                isAllowed = doc => deniedExtensions.Any(ext => doc.FullName.EndsWith(ext, true, CultureInfo.InvariantCulture)) == false;
        }

        public bool IsAllowed(Document document)
        {
            return isAllowed(document);
        }
    }
}