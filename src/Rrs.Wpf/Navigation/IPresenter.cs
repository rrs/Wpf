using System.Windows;

namespace Rrs.Wpf.Navigation;

public interface IPresenter
{
    FrameworkElement PresentView(object? presenterArgs = null);
}

public interface IPresenter<TModel> : IPresenter
{
    FrameworkElement PresentView(TModel? presenterArgs);

#if NET6_0_OR_GREATER
    FrameworkElement IPresenter.PresentView(object? presenterArgs) => PresentView(presenterArgs is TModel args ? args : default);
#endif
}

public interface IPresenterFactory
{
    IPresenter CreatePresenter();
    Type PresenterType { get; }
}