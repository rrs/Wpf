using Rrs.Wpf.Navigation;
using System.Windows;
using TestApp.Data.Objects;
using TestApp.ViewModels;
using TestApp.Views;

namespace TestApp.Presenters;

internal class Page6Presenter : IPresenter<Page6Data>
{
    private FrameworkElement _view;
    public Page6ViewModel ViewModel { get; }

    public Page6Presenter(FrameworkElement element, Page6ViewModel viewModel)
    {
        _view = element;
        ViewModel = viewModel;
    }

    public FrameworkElement PresentView(Page6Data? presenterArgs)
    {
        ViewModel.Text = presenterArgs?.Text;
        return _view;
    }
}

internal class Page6PresenterFactory : IPresenterFactory
{
    public Type PresenterType => typeof(Page6Presenter);

    public IPresenter CreatePresenter()
    {
        var vm = new Page6ViewModel();
        var view = new Page6View
        {
            DataContext = vm
        };
        return new Page6Presenter(view, vm);
    }
}

