using Rrs.Wpf;
using Rrs.Wpf.Navigation;
using System.Windows.Input;
using TestApp.Presenters;

namespace TestApp.ViewModels;

internal class SubWindowViewModel
{
    public NavigationAreaNavigator Navigator { get; } = new NavigationAreaNavigator();

    public ICommand PreviousPageCommand { get; }
    public ICommand NextPageCommand { get; }

    public SubWindowViewModel()
    {
        PreviousPageCommand = new RelayCommand(_ => Navigator.PreviousPage());
        NextPageCommand = new RelayCommand(_ => Navigator.NextPage<SubWindowPresenter>());
    }
}
