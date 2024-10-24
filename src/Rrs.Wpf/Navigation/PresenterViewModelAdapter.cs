using System.Windows;
using System.Windows.Controls;

namespace Rrs.Wpf.Navigation;

internal class PresenterViewModelAdapter : IPresenter, IApplyPageAction
{
    private readonly FrameworkElement _e;
    private readonly object _vm;

    public PresenterViewModelAdapter(object vm)
    {
        _vm = vm;
        _e = new ContentPresenter
        {
            Content = _vm,
        };
    }

    public FrameworkElement PresentView(object? presenterArgs) => _e;

    public void ApplyPageAction(Action<object>? pageAction)
    {
        pageAction?.Invoke(_vm);
    }
}
