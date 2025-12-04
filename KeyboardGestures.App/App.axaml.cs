using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using KeyboardGestures.UI.Windows;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Diagnostics;

namespace KeyboardGestures.App
{
    public partial class App : Application
    {
        public TrayMenuWindow? _menuWindow;
        public override void Initialize()
        {
            
            AvaloniaXamlLoader.Load(this);
        }

        public override void OnFrameworkInitializationCompleted()
        {
            if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
            {
                desktop.MainWindow = null;
            }
            _menuWindow = Program.Services.GetRequiredService<TrayMenuWindow>();
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


        


    }
}