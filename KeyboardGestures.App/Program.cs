using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using Avalonia;
using KeyboardGestures.Core.Commands;
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
                        var handler = Services.GetRequiredService<GestureHandler>();

                        hook.KeyEventReceived += interpreter.OnKeyEvent;
                        hook.Start();
                    }
                
                ).StartWithClassicDesktopLifetime(args);
        }

        public static void ConfigureServices()
        {
            var services = new ServiceCollection();


            services.AddSingleton<IKeyboardHookService, WindowsKeyboardHookService>();
            services.AddSingleton<IGestureInterpreter, GestureInterpreter>();

            var registry = new CommandRegistry();
            var jsonPath = Path.Combine(AppContext.BaseDirectory, "Resources\\commands.json");
            if(File.Exists(jsonPath))
            {
                var json = File.ReadAllText(jsonPath);
                var cmds = JsonSerializer.Deserialize<List<CommandDefinition>>(json);
                if(cmds != null)
                {
                    foreach (var cmd in cmds)
                        registry.Register(cmd);
                }
            }
            services.AddSingleton(registry);


            services.AddSingleton<ICommandExecutor, CommandExecutor>();
            services.AddSingleton<GestureHandler>();
            


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
