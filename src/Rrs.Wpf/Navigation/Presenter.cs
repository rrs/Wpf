using System.Windows;

namespace Rrs.Wpf.Navigation;

public abstract class Presenter<T> : IPresenter<T>
{
    public abstract FrameworkElement PresentView(T? presenterArgs);
    FrameworkElement IPresenter.PresentView(object? presenterArgs)
        => PresentView(presenterArgs is T args ? args : default);
}
