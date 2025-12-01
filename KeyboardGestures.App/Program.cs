using System;
using Avalonia;
using KeyboardGestures.Core.Gestures;
using KeyboardGestures.Core.KeyboardHook;
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
            BuildAvaloniaApp().AfterSetup( _ =>
                    {
                        ConfigureServices();
                        var hook = Services.GetRequiredService<IKeyboardHookService>();
                        var interpreter = Services.GetRequiredService<IGestureInterpreter>();

                        hook.KeyEventReceived += interpreter.OnKeyEvent;
                        hook.Start();
                    }
                
                ).StartWithClassicDesktopLifetime(args);
        }

        public static void ConfigureServices()
        {
            var services = new ServiceCollection();

            // add dependencies here:
            // e.g services.AddSingleton<IGestutureCommandExecutionService, GestureCommandExecutionService>();
            // or services.AddSingleton<OverlayViewModel>();

            services.AddSingleton<IKeyboardHookService, WindowsKeyboardHookService>();
            services.AddSingleton<IGestureInterpreter, GestureInterpreter>();


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
