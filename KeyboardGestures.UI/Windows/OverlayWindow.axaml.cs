using Avalonia.Controls;
using KeyboardGestures.UI.ViewModels;

namespace KeyboardGestures.UI.Windows;

public partial class OverlayWindow : Window
{
    public OverlayWindow(OverlayViewModel vm)
    {
        InitializeComponent();
        DataContext = vm;
    }
}