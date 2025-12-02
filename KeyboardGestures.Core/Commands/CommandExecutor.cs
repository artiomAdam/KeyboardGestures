using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KeyboardGestures.Core.Commands
{
    public class CommandExecutor : ICommandExecutor
    {
        public void Execute(string actionId)
        {
            Debug.WriteLine($"Execution action: {actionId}");

            switch(actionId)
            {
                case "LaunchApp":
                    Debug.WriteLine("Launch an app");
                    // TODO: launch app
                    break;
                case "ShowOverlay":
                    Debug.WriteLine("Show an overlay");
                    // TODO: show the overlay window
                    break;
                case "CopyPath":
                    // TODO: copy the path to current directory
                    break;

                default:
                    Debug.WriteLine($"Unknown action: {actionId}");
                    break;
            }
        }
    }
}
