using Rrs.Wpf;
using Rrs.Wpf.Navigation;
using System.Windows.Input;
using TestApp.Data.Objects;
using TestApp.Presenters;

namespace TestApp.ViewModels;

internal class Page1ViewModel : ViewModel
{
    private readonly INavigator _navigator;

    public ICommand NextPageCommand { get; }

    public int Cookies { get; set; }

    public Page1ViewModel(INavigator navigator)
    {
        NextPageCommand = new RelayCommand(NextPage);
        _navigator = navigator;
    }

    private async void NextPage(object? obj)
    {
        await _navigator.With(new Page2Data(Cookies)).NextPageAsync<Page2Presenter>();
    }
}

