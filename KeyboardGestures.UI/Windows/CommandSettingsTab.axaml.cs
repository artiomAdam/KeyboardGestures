using Avalonia.Controls;
using Avalonia.Interactivity;
using KeyboardGestures.UI.ViewModels;

namespace KeyboardGestures.UI.Windows;

public partial class SettingsWindow : Window
{
    public SettingsWindow(CommandSettingsViewModel vm)
    {
        InitializeComponent();
        DataContext = vm;
    }

    private void SequenceBox_GotFocus(object? sender, RoutedEventArgs e)
    {
        if (DataContext is CommandSettingsViewModel vm)
            vm.OnSequenceInputGotFocus();
    }

    private void SequenceBox_LostFocus(object? sender, RoutedEventArgs e)
    {
        if (DataContext is CommandSettingsViewModel vm)
            vm.OnSequenceInputLostFocus();
    }

    protected override void OnClosing(WindowClosingEventArgs e)
    {
        e.Cancel = true;
        Hide();
    }
}