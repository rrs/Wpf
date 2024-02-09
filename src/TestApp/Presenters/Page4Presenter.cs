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
        return new Page4View
        {
            DataContext = new Page4ViewModel
            {
                Name = data?.Name
            }
        };
    }
}