using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Threading;
using KeyboardGestures.UI.Utilities;

namespace KeyboardGestures.UI.Windows;

public partial class TrayMenuWindow : Window
{

    private CancellationTokenSource? _mouseLeaveCts;
    private readonly TimeSpan _hideDelay = TimeSpan.FromMilliseconds(400);
    private readonly SettingsWindow _settingsWindow;
    public TrayMenuWindow(SettingsWindow settingsWindow)
    {
        InitializeComponent();
        this.Deactivated += (_, _) => Hide();
        this.PointerEntered += OnPointerEntered;
        this.PointerExited += OnPointerExited;
        _settingsWindow = settingsWindow;
    }

    private void OnPointerEntered(object? sender, PointerEventArgs e)
    {
        _mouseLeaveCts?.Cancel();
        _mouseLeaveCts = null;
    }

    private void OnPointerExited(object? sender, PointerEventArgs e)
    {
        _mouseLeaveCts?.Cancel();
        _mouseLeaveCts = new CancellationTokenSource();
        var token = _mouseLeaveCts.Token;

        _ = Task.Run(async () => { 
            try
            {
                await Task.Delay(_hideDelay, token);
                if(!token.IsCancellationRequested)
                {
                    await Dispatcher.UIThread.InvokeAsync(() =>
                    {
                        Hide();
                    });
                }
            }
            catch(TaskCanceledException)
            {
                // drop
            }
        });
    }
    public void ShowAtCursor()
    {
        var screenPos = Win32Cursor.GetCursorPixelPoint();

        this.Position = new PixelPoint(screenPos.X,
                                       screenPos.Y - (int)(Height + 10));
        Show();
        Activate();
    }

    public void OnSettingsClick(object sender, RoutedEventArgs e)
    {
        _settingsWindow.Show();
    }

    public void OnQuitClick(object sender, RoutedEventArgs e)
    {
        Environment.Exit(0);
    }

}