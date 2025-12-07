using Avalonia;
using Avalonia.ReactiveUI;
using KeyboardGestures.Core.Commands;
using KeyboardGestures.Core.Gestures;
using KeyboardGestures.Core.JsonStorage;
using KeyboardGestures.Core.KeyboardHook;
using KeyboardGestures.UI.ViewModels;
using KeyboardGestures.UI.Windows;
using Microsoft.Extensions.DependencyInjection;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Reactive.Concurrency;
using System.Text.Json;
using System.Text.Json.Serialization;

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


            services.AddSingleton<IKeyboardHookService, WindowsKeyboardHookService>();
            services.AddSingleton<IGestureInterpreter, GestureInterpreter>();

            var registry = new CommandRegistry();
            var jsonPath = Path.Combine(AppContext.BaseDirectory, "Resources\\commands.json");
            services.AddSingleton<IJsonStorageService>(new JsonCommandStorageService(jsonPath));

            services.AddSingleton<CommandRegistry>();            // empty registry
            services.AddSingleton<ICommandService, CommandService>(); // service loads registry

            services.AddSingleton<ICommandExecutor, CommandExecutor>();
            services.AddSingleton<GestureHandler>();

            // windows:
            services.AddSingleton<TrayMenuWindow>();

            services.AddSingleton<OverlayViewModel>();
            services.AddSingleton<OverlayWindow>();

            services.AddSingleton<SettingsViewModel>();
            services.AddSingleton<SettingsWindow>();


            Services = services.BuildServiceProvider();
        }

        // Avalonia configuration, don't remove; also used by visual designer.
        public static AppBuilder BuildAvaloniaApp()
            => AppBuilder.Configure<App>()
                .UsePlatformDetect()
                .WithInterFont()
                .LogToTrace().UseReactiveUI();
    }
}
