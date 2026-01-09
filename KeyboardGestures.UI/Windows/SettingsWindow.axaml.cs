using Avalonia.Controls;
using KeyboardGestures.UI.ViewModels;

namespace KeyboardGestures.UI.Windows;

public partial class SettingsWindow : Window
{
    public SettingsWindow()
    {
        InitializeComponent();
    }
    public SettingsWindow(CommandSettingsViewModel commandSettingsVM, GeneralSettingsViewModel generalSettingsVM) : this()
    {
       
       DataContext = new SettingsViewModel(commandSettingsVM, generalSettingsVM);
    }

    protected override void OnClosing(WindowClosingEventArgs e)
    {
        e.Cancel = true;
        Hide();
    }  
}