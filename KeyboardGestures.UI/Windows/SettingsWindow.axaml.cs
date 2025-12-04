using Avalonia.Controls;
using KeyboardGestures.UI.ViewModels;

namespace KeyboardGestures.UI.Windows;

public partial class SettingsWindow : Window
{
    public SettingsWindow(SettingsViewModel vm)
    {
        InitializeComponent();
        DataContext = vm;
    }
}