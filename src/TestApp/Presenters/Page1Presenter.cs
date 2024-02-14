using Rrs.Wpf.Navigation;
using System.Windows;
using TestApp.ViewModels;
using TestApp.Views;

namespace TestApp.Presenters;

internal class Page1Presenter : IPresenter
{
    public FrameworkElement PresentView(object? _)
    {
        var view = new Page1View();
        var navigator = new FocusedElementNavigator(view, view.Dispatcher);
        view.DataContext = new Page1ViewModel(navigator);

        return view;
    }
}
