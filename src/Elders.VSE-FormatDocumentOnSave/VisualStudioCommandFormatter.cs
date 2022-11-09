using System;
using System.Linq;
using System.Windows.Forms;
using EnvDTE;
using EnvDTE80;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.VisualStudio.Shell;

namespace Elders.VSE_FormatDocumentOnSave
{
    public class VisualStudioCommandFormatter : IDocumentFormatter
    {
        const string defaultCommand = "Edit.FormatDocument";

        private readonly DTE dte;
        private readonly Events2 _dteEvents;
        private WindowKeyboardHook keyboardHook;
        private TextEditorEvents textEditorEvents;
        private readonly MemoryCache pendingDocuments;
        private readonly MemoryCache documentsInProgress;
        private readonly MemoryCacheEntryOptions cachedItems;

        public VisualStudioCommandFormatter(DTE dte)
        {
            Microsoft.VisualStudio.Shell.ThreadHelper.ThrowIfNotOnUIThread();

            this.dte = dte ?? throw new ArgumentNullException(nameof(dte));
            _dteEvents = (Events2)this.dte.Events;
            var opt = new MemoryCacheOptions();
            this.cachedItems = new MemoryCacheEntryOptions()
            {
                SlidingExpiration = TimeSpan.FromSeconds(5),
            };

            this.pendingDocuments = new MemoryCache(opt);
            this.documentsInProgress = new MemoryCache(opt);
            //UpdateCaptureEvents();
        }

        public void Format(Document document, IDocumentFilter filter, string command)
        {
            ThreadHelper.ThrowIfNotOnUIThread();

            var currentDoc = dte.ActiveDocument;

            if (pendingDocuments.TryGetValue(document.FullName, out _))
                return;

            documentsInProgress.Set(document.FullName, document);

            document.Activate();
            if (dte.ActiveWindow.Kind == "Document" && filter.IsAllowed(document))
            {
                if (string.IsNullOrWhiteSpace(command))
                {
                    command = defaultCommand;
                }

                dte.ExecuteCommand(command, string.Empty);

                pendingDocuments.Set(document.FullName, document, cachedItems);
            }

            currentDoc.Activate();
            documentsInProgress.Remove(document.FullName);
        }

        private void UpdateCaptureEvents()
        {
            ThreadHelper.ThrowIfNotOnUIThread();

            if (textEditorEvents is null)
            {
                textEditorEvents = _dteEvents.TextEditorEvents;
                textEditorEvents.LineChanged += OnLineChanged;
            }

            if (keyboardHook is null)
            {
                keyboardHook = new WindowKeyboardHook();
                keyboardHook.OnMessage += OnKeyboardMessage;
                keyboardHook.Install();
            }
        }

        private void OnLineChanged(TextPoint startPoint, TextPoint endPoint, int hint)
        {
            ThreadHelper.ThrowIfNotOnUIThread();

            Document currentDoc = startPoint.DTE.ActiveDocument;

            if (currentDoc is null || documentsInProgress.TryGetValue(currentDoc.FullName, out _))
                return;

            if (pendingDocuments.Count > 0 && pendingDocuments.TryGetValue(currentDoc.FullName, out _))
            {
                dte.ExecuteCommand("File.SaveSelectedItems");
            }
        }

        private void OnKeyboardMessage(Keys key, bool isPressing)
        {
            if (isPressing == false)
                return;

            if (pendingDocuments.Count == 0)
                return;

            if (_bypassKeys.Contains(key))
                return;

            ThreadHelper.ThrowIfNotOnUIThread();

            Document currentDoc = dte.ActiveDocument;
            pendingDocuments.Remove(currentDoc.FullName);
        }

        private readonly Keys[] _bypassKeys =
        {
            Keys.Up, Keys.Down, Keys.Left, Keys.Right,
            Keys.Home, Keys.End, Keys.PageUp, Keys.PageDown,
            Keys.Escape, Keys.CapsLock,
            Keys.ControlKey, Keys.ShiftKey, Keys.Alt,
            Keys.F1, Keys.F2, Keys.F3, Keys.F4, Keys.F5, Keys.F6, Keys.F7, Keys.F8, Keys.F9, Keys.F10, Keys.F11, Keys.F12,
        };
    }
}
