using System.ComponentModel.Composition;
using EnvDTE;
using Microsoft.VisualStudio;
using Microsoft.VisualStudio.Editor;
using Microsoft.VisualStudio.OLE.Interop;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Text.Editor;
using Microsoft.VisualStudio.TextManager.Interop;
using Microsoft.VisualStudio.Utilities;

namespace KeyBindingTest
{
    [Export(typeof(IVsTextViewCreationListener))]
    [ContentType("text")]
    [TextViewRole(PredefinedTextViewRoles.Editable)]
    internal class KeyBindingCommandFilterProvider : IVsTextViewCreationListener
    {
        [Import(typeof(IVsEditorAdaptersFactoryService))]
        internal IVsEditorAdaptersFactoryService editorFactory = null;

        [Import(typeof(SVsServiceProvider))]
        internal SVsServiceProvider ServiceProvider = null;

        public void VsTextViewCreated(IVsTextView textViewAdapter)
        {
            var dte = (DTE)ServiceProvider.GetService(typeof(DTE));
            AddCommandFilter(textViewAdapter, new KeyBindingCommandFilter(dte));
        }

        void AddCommandFilter(IVsTextView viewAdapter, KeyBindingCommandFilter commandFilter)
        {
            if (commandFilter.m_added == false)
            {
                //get the view adapter from the editor factory
                IOleCommandTarget next;
                int hr = viewAdapter.AddCommandFilter(commandFilter, out next);

                if (hr == VSConstants.S_OK)
                {
                    commandFilter.m_added = true;
                    //you'll need the next target for Exec and QueryStatus 
                    if (next != null)
                        commandFilter.m_nextTarget = next;
                }
            }
        }
    }
}