using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using KeyboardGestures.UI.Windows;
using System;
using System.Diagnostics;

namespace KeyboardGestures.App
{
    public partial class App : Application
    {
        public TrayMenuWindow? _menuWindow;
        public override void Initialize()
        {
            _menuWindow = new TrayMenuWindow();
            AvaloniaXamlLoader.Load(this);
        }

        public override void OnFrameworkInitializationCompleted()
        {
            if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
            {
                desktop.MainWindow = null;
            }

            base.OnFrameworkInitializationCompleted();
        }

        private void OnTrayClicked(object? sender, EventArgs e)
        {
            Debug.WriteLine("TrayClick");
            if(_menuWindow is TrayMenuWindow tray)
            {

                

                if (!tray.IsVisible) tray.ShowAtCursor();
                else tray.Hide();
            }
        }
        private void Click_Quit(object? sender, System.EventArgs args)
        {
            if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
            {
                desktop.Shutdown();
            }
        }


    }
}