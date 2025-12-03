using System.Diagnostics;

namespace KeyboardGestures.Core.Commands
{
    public class CommandExecutor : ICommandExecutor
    {
        public void Execute(CommandDefinition cmd)
        {
            Debug.WriteLine($"Execution action: {cmd.CommandType}");

            switch(cmd.CommandType)
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
                }
                

                default:
                    Debug.WriteLine($"Unknown action: ");
                    break;
            }
        }
    }
}
