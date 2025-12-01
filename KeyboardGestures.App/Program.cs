using System;
using Avalonia;
using Microsoft.Extensions.DependencyInjection;

namespace KeyboardGestures.App
{
    internal class Program
    {
        public static IServiceProvider Services { get; private set; }
        // Initialization code. Don't use any Avalonia, third-party APIs or any
        // SynchronizationContext-reliant code before AppMain is called: things aren't initialized
        // yet and stuff might break.
        [STAThread]
        public static void Main(string[] args)
        {
            ConfigureServices();
            BuildAvaloniaApp()
                .StartWithClassicDesktopLifetime(args);
        }

        public static void ConfigureServices()
        {
            var services = new ServiceCollection();

            // add dependencies here:
            // e.g services.AddSingleton<IGestutureCommandExecutionService, GestureCommandExecutionService>();
            // or services.AddSingleton<OverlayViewModel>();



            Services = services.BuildServiceProvider();
        }

        // Avalonia configuration, don't remove; also used by visual designer.
        public static AppBuilder BuildAvaloniaApp()
            => AppBuilder.Configure<App>()
                .UsePlatformDetect()
                .WithInterFont()
                .LogToTrace();
    }
}
