using Rrs.Wpf.Navigation;
using System.Windows;
using TestApp.Data.Objects;
using TestApp.Views;

namespace TestApp.Presenters;

internal class Page5Presenter : IPresenter<Page5Data>
{
    public FrameworkElement PresentView(Page5Data? presenterArgs)
    {
        var view = new Page5View();
        view.Date.Text = (presenterArgs?.Date ?? DateTime.Now).ToString("dd-MM-yyyy");
        return view;
    }
}
