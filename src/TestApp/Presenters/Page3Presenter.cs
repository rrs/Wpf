using Rrs.Wpf.Navigation;
using System.Windows;
using TestApp.Views;

namespace TestApp.Presenters;

internal class Page3Presenter : IPresenter<int>
{
    public FrameworkElement PresentView(int cookiesClicked)
    {
        var view = new Page3View();
        view.NumberOfCookies.Text = $"Number of Cookies Clicked {cookiesClicked}";
        return view;
    }
}