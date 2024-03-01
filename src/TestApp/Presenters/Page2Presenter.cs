using Rrs.Wpf.Navigation;
using System.Windows;
using TestApp.Data.Objects;
using TestApp.ViewModels;
using TestApp.Views;

namespace TestApp.Presenters;

internal class Page2Presenter : IPresenter<Page2Data>
{
    public FrameworkElement PresentView(Page2Data? data)
    {
        var view = new Page2View();
        var navigator = new FocusedElementNavigator(view, view.Dispatcher);
        view.DataContext = new Page2ViewModel(navigator) { Cookies = data?.Cookies ?? 0 };

        return view;
    }
}