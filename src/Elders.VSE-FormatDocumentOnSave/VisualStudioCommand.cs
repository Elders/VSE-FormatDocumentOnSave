using System;

namespace Elders.VSE_FormatDocumentOnSave
{
    public class VisualStudioCommand
    {
        public VisualStudioCommand(string command, string arguments = "")
        {
            if (string.IsNullOrEmpty(command)) throw new ArgumentNullException(nameof(command));

            Command = command;
            Arguments = arguments;
        }

        public string Command { get; private set; }

        public string Arguments { get; private set; }
    }
}