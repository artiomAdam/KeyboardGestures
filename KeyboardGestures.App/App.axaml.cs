using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using Microsoft.Extensions.DependencyInjection;

namespace KeyboardGestures.App
{
    public partial class App : Application
    {
        public override void Initialize()
        {
            AvaloniaXamlLoader.Load(this);
        }

        public override void OnFrameworkInitializationCompleted()
        {
            if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
            {
                desktop.MainWindow = null;

                var trayIcon = new TrayIcon
                {
                    Icon = new WindowIcon("Assets/Icon.png"),
                    ToolTipText = "KeyboardGestures Running",
                    IsVisible = true,
                };

                var quitItem = new NativeMenuItem("Quit");
                quitItem.Click += (_, __) =>
                {
                    desktop.Shutdown();
                };

                trayIcon.Menu = [quitItem,];
                
            }

            base.OnFrameworkInitializationCompleted();
        }
    }
}