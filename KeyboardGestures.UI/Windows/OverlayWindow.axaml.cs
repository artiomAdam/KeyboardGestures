using Avalonia;
using Avalonia.Animation;
using Avalonia.Animation.Easings;
using Avalonia.Controls;
using Avalonia.Media;
using Avalonia.Styling;
using Avalonia.Threading;
using KeyboardGestures.UI.ViewModels;
using System.Diagnostics;

namespace KeyboardGestures.UI.Windows;

public partial class OverlayWindow : Window
{

    private TranslateTransform SlideTransform =>
    (TranslateTransform?)RootBorder.RenderTransform!;
    public OverlayWindow(OverlayViewModel vm)
    {
        InitializeComponent();
        DataContext = vm;
    }

    protected override async void OnOpened(EventArgs e)
    {
        base.OnOpened(e);

        var screen = Screens.Primary.WorkingArea;
        Height = screen.Height;
        Width = 300;

        Position = new PixelPoint(screen.X, screen.Y);

        // Wait for layout so Width is valid, then place offscreen
        await Dispatcher.UIThread.InvokeAsync(() => { }, DispatcherPriority.Render);
        SlideTransform.X = -Width;
    }

    public async Task OpenOverlay()
    {
        Show();

        await Dispatcher.UIThread.InvokeAsync(() => { }, DispatcherPriority.Render);
        await AnimateSlideIn();
    }

    public async Task CloseOverlay()
    {
        await AnimateSlideOut();
        Hide();
    }

    private async Task AnimateSlideIn()
    {
        var animation = new Animation
        {
            Duration = TimeSpan.FromMilliseconds(90),
            Easing = new CubicEaseOut(),
            FillMode = FillMode.Forward, 
            Children =
        {
            new KeyFrame
            {
                Cue = new Cue(0),
                Setters = { new Setter(TranslateTransform.XProperty, -Width) }
            },
            new KeyFrame
            {
                Cue = new Cue(1),
                Setters = { new Setter(TranslateTransform.XProperty, 0d) }
            }
        }
        };

        await animation.RunAsync(RootBorder);
    }


    private async Task AnimateSlideOut()
    {
        var animation = new Animation
        {
            Duration = TimeSpan.FromMilliseconds(80),
            Easing = new CubicEaseIn(),
            FillMode = FillMode.Forward, 
            Children =
        {
            new KeyFrame
            {
                Cue = new Cue(0),
                Setters = { new Setter(TranslateTransform.XProperty, 0d) }
            },
            new KeyFrame
            {
                Cue = new Cue(1),
                Setters = { new Setter(TranslateTransform.XProperty, -Width) }
            }
        }
        };

        await animation.RunAsync(RootBorder);
    }









}