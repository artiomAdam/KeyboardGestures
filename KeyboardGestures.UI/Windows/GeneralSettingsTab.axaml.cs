using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using KeyboardGestures.UI.ViewModels;

namespace KeyboardGestures.UI.Windows;

public partial class GeneralSettingsTab : UserControl
{
    public GeneralSettingsTab()
    {
        InitializeComponent();
    }

    private void OnActivationKeyFocus(object? sender, GotFocusEventArgs e)
    {
        (DataContext as GeneralSettingsViewModel)?.BeginRecording();
    }

    private void OnActivationKeyLostFocus(object? sender, RoutedEventArgs e)
    {
        (DataContext as GeneralSettingsViewModel)?.StopRecording();
    }
}