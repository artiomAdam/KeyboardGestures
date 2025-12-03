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
                case CommandType.LaunchApp:
                {
                    if(!string.IsNullOrWhiteSpace(cmd.ApplicationPath))
                    {
                        try
                        {
                            Process.Start(new ProcessStartInfo
                            {
                                FileName = cmd.ApplicationPath,
                                UseShellExecute = true
                            });
                        }
                        catch (Exception ex)
                        {
                            Debug.WriteLine($"Failed to launch: {ex}");
                        }
                    }
                    else
                    {
                        Debug.WriteLine("LaunchApp called with no ApplicationPath");
                    }
                    break;
                }
                

                default:
                    Debug.WriteLine($"Unknown action: ");
                    break;
            }
        }
    }
}
