using KeyboardGestures.Core.Utilities;
using System.Diagnostics;


namespace KeyboardGestures.Core.Commands
{
    public class CommandExecutor : ICommandExecutor
    {
        private readonly ICommandExecutionService _executionPlatform;
        public CommandExecutor(ICommandExecutionService executionPlatform)
        {
            _executionPlatform = executionPlatform;
        }
        public void Execute(CommandDefinition cmd)
        {
            Debug.WriteLine($"Execution action: {cmd.CommandType}");

            switch (cmd.CommandType)
            {
                case CommandType.LaunchApp:
                    {
                        _executionPlatform.LaunchApp(cmd.ApplicationPath);
                        break;
                    }
                case CommandType.LaunchWebpage:
                    {
                        _executionPlatform.LaunchWebpage(cmd.Url);
                        break;
                    }
                case CommandType.CopyCurrentPath:
                    {
                        _executionPlatform.CopyCurrentPath();
                        break;
                    }
                case CommandType.ToggleMute:
                    {
                        _executionPlatform.ToggleMute();
                        break;
                    }
                case CommandType.TakeScreenshot:
                    {
                        _executionPlatform.TakeScreenshot();
                        break;
                    }
                default:
                    Debug.WriteLine($"Unknown action: ");
                    break;
            }
        }
    }

}
