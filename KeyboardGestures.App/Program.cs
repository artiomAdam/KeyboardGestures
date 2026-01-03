using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.ReactiveUI;
using KeyboardGestures.Core.Commands;
using KeyboardGestures.Core.Gestures;
using KeyboardGestures.Core.JsonStorage;
using KeyboardGestures.Core.KeyboardHook;
using KeyboardGestures.Core.Settings;
using KeyboardGestures.UI.ExecutionPlatform;
using KeyboardGestures.UI.ViewModels;
using KeyboardGestures.UI.Windows;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.IO;


namespace KeyboardGestures.App
{
    internal class Program
    {
        public static IServiceProvider Services { get; private set; }
        [STAThread]
        public static void Main(string[] args)
        {
            
            BuildAvaloniaApp().AfterSetup( _ =>
                    {
                        ConfigureServices();
                        var hook = Services.GetRequiredService<IKeyboardHookService>();
                        var interpreter = Services.GetRequiredService<IGestureInterpreter>();
                        var handler = Services.GetRequiredService<GestureHandler>();

                        OverlayWindow? overlay = null;

                        handler.OverlayShouldOpen += async () =>
                        {
                            if (overlay == null)
                            {
                                overlay = Services.GetRequiredService<OverlayWindow>();
                                await overlay.OpenOverlay();
                            }
                        };

                        handler.OverlayShouldClose += async () =>
                        {
                            if (overlay is not null)
                            { 
                                await overlay.CloseOverlay();
                                overlay = null;
                            }
                            
                            
                        };

                        hook.KeyEventReceived += interpreter.OnKeyEvent;
                        hook.Start();
                    }
                
                ).StartWithClassicDesktopLifetime(args);
        }

        public static void ConfigureServices()
        {
            var services = new ServiceCollection();

            // win32 keyboard hook
            services.AddSingleton<IKeyboardHookService, WindowsKeyboardHookService>();

            // storage services
            var jsonCommandsPath = Path.Combine(AppContext.BaseDirectory, "Resources\\commands.json");
            var jsonSettingsPath = Path.Combine(AppContext.BaseDirectory, "Resources\\settings.json");
            services.AddSingleton<IJsonStorage<List<CommandDefinition>>>(_ => new JsonFileStorage<List<CommandDefinition>>(jsonCommandsPath));
            services.AddSingleton<IJsonStorage<AppSettings>>(_ => new JsonFileStorage<AppSettings>(jsonSettingsPath));

            // commands and gestures
            services.AddSingleton<IGestureInterpreter, GestureInterpreter>();
            services.AddSingleton<CommandRegistry>();
            services.AddSingleton<ICommandService, CommandService>();
            services.AddSingleton<ICommandExecutor, CommandExecutor>();
            services.AddSingleton<GestureHandler>();
            services.AddSingleton<IExecutionPlatform, CommandExecutionPlatform>();

            // windows:
            services.AddSingleton<TrayMenuWindow>();
            services.AddSingleton<OverlayViewModel>();
            services.AddSingleton<OverlayWindow>();
            services.AddSingleton<SettingsViewModel>();
            services.AddSingleton<SettingsWindow>();
            

            Services = services.BuildServiceProvider();
        }

       
        public static AppBuilder BuildAvaloniaApp()
            => AppBuilder.Configure<App>()
                .UsePlatformDetect()
                .WithInterFont()
                .LogToTrace().UseReactiveUI();
    }
}
