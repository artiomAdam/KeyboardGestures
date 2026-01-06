namespace KeyboardGestures.Core.Commands
{
    public interface ICommandExecutionService
    {
        Task LaunchApp(string? path);
        Task LaunchWebpage(string? url);
        Task CopyCurrentPath();
        Task ToggleMute();
        Task TakeScreenshot();
    }
}
