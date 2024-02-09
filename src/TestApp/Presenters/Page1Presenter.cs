using Rrs.Wpf.Navigation;
using System.Windows;
using TestApp.ViewModels;
using TestApp.Views;

namespace TestApp.Presenters;

internal class Page1Presenter : IPresenter
{
    public FrameworkElement PresentView(object? _)
    {
        return new Page1View
        {
            DataContext = new Page1ViewModel()
        };
    }
}
