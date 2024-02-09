using Rrs.Wpf;
using Rrs.Wpf.Navigation;
using System.Windows.Input;
using TestApp.Data.Objects;
using TestApp.Presenters;

namespace TestApp.ViewModels;

internal class Page1ViewModel : ViewModel
{
    public ICommand NextPageCommand { get; }

    public int Cookies { get; set; }

    public Page1ViewModel()
    {
        NextPageCommand = new RelayCommand(_ => Navigator.With(new Page2Data(Cookies)).NextPage<Page2Presenter>());
    }
}

