using System.Windows;

namespace Rrs.Wpf.Navigation;

internal class PresenterViewAdapter : IPresenter
{
    private readonly FrameworkElement _e;
    public PresenterViewAdapter(FrameworkElement e) => _e = e;
    public FrameworkElement PresentView(object? _) => _e;
}
