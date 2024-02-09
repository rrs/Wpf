using Rrs.Wpf;
using Rrs.Wpf.Navigation;
using System.Windows.Input;
using TestApp.Presenters;

namespace TestApp.ViewModels;

internal class Page2ViewModel : ViewModel
{
    public ICommand NextPageCommand { get; }
    public ICommand CookieCommand { get; }

    private int _cookies;
    public int Cookies
    {
        get => _cookies;
        set
        {
            _cookies = value;
            OnPropertyChanged(nameof(Cookies));
            OnPropertyChanged(nameof(ShowSecret));
        }
    }

    public bool ShowSecret => Cookies == 1337;

    public Page2ViewModel()
    {
        NextPageCommand = new RelayCommand(_ => Navigator.With<int>(Cookies).NextPage<Page3Presenter>());
        CookieCommand = new RelayCommand(_ => Cookies++);
    }
}
