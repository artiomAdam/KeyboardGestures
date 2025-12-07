using System.Diagnostics;

namespace KeyboardGestures.Core.Commands
{
    public class CommandExecutor : ICommandExecutor
    {
        public void Execute(CommandDefinition cmd)
        {
            Debug.WriteLine($"Execution action: {cmd.CommandType}");

            switch (cmd.CommandType)
            {
                case CommandType.LaunchApp:
                    {
                        LaunchApp(cmd.ApplicationPath);
                        break;
                    }
                case CommandType.LaunchWebpage:
                    {
                        LaunchWebpage(cmd.Url);
                        break;
                    }
                case CommandType.CopyCurrentPath:
                    {
                        CopyCurrentPath();
                        break;
                    }
                case CommandType.ToggleMute:
                    {
                        ToggleMute();
                        break;
                    }
                case CommandType.TakeScreenshot:
                    {
                        TakeScreenshot();
                        break;
                    }
                default:
                    Debug.WriteLine($"Unknown action: ");
                    break;
            }
        }
        private void LaunchApp(string? path)
        {
            if (string.IsNullOrWhiteSpace(path))
                return;

            try
            {
                Process.Start(new ProcessStartInfo
                {
                    FileName = path,
                    UseShellExecute = true
                });
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"LaunchApp error: {ex}");
            }
        }

        private void LaunchWebpage(string? url)
        {
            if (string.IsNullOrWhiteSpace(url))
                return;

            Task.Run(() =>
            {
                try
                {
                    Process.Start(new ProcessStartInfo
                    {
                        FileName = url,
                        UseShellExecute = true
                    });
                }
                catch (Exception ex)
                {
                    Debug.WriteLine($"OpenWebpage error: {ex}");
                }
            });
        }
        private void CopyCurrentPath()
        {
            return;
        }
        private void ToggleMute()
        {
            return;
        }
        private void TakeScreenshot()
        {
            return;
        }
    }

}
