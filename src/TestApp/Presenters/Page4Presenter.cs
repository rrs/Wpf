using Rrs.Wpf;
using Rrs.Wpf.Navigation;
using System.Windows;
using TestApp.Data.Objects;
using TestApp.ViewModels;
using TestApp.Views;

namespace TestApp.Presenters;

internal class Page4Presenter : IPresenter<Page4Data>
{
    public FrameworkElement PresentView(Page4Data? data)
    {
        var view = new Page4View();
        var appCommander = new FocusedElementAppCommander(view, view.Dispatcher);
        view.DataContext = new Page4ViewModel(appCommander) { Name = data?.Name };

        return view;
    }
}