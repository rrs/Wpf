using Rrs.Wpf.Navigation;
using System.Windows;
using System.Windows.Media;
using TestApp.Views;

namespace TestApp.Presenters;

internal class SubWindowPresenter : IPresenter
{
    private readonly Random _random = new Random();

    public FrameworkElement PresentView(object? presenterArgs = null)
    {
        var view = new SubWindowView();
        var backgroundColor = Color.FromRgb((byte)_random.Next(256), (byte)_random.Next(256), (byte)_random.Next(256));
        var foreColor = ColorHelper.ContrastingForegroundColor(backgroundColor);
        view.Background = new SolidColorBrush(backgroundColor);
        view.Foreground = new SolidColorBrush(foreColor);
        var htmlColor = System.Drawing.ColorTranslator.ToHtml(System.Drawing.Color.FromArgb(backgroundColor.A, backgroundColor.R, backgroundColor.G, backgroundColor.B));
        view.RandomNumber.Text = htmlColor;
        return view;
    }
}
