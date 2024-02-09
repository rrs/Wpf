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
        return new Page2View
        {
            DataContext = new Page2ViewModel {  Cookies = data?.Cookies ?? 0 }
        };
    }
}