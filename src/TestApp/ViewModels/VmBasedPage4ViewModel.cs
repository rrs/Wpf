using Rrs.Wpf;
using Rrs.Wpf.Navigation;

namespace TestApp.ViewModels;

internal class VmBasedPage4ViewModel : ViewModel, INavigateTo
{
    private int _timesPagedTo;
    private readonly IApplicationNavigator _navigator;

    public int TimesPagedTo
    {
        get => _timesPagedTo;
        set
        {
            _timesPagedTo = value;
            OnPropertyChanged(nameof(TimesPagedTo));
        }
    }

    public bool ShouldBounce { get; set; }

    public VmBasedPage4ViewModel(IApplicationNavigator navigator)
    {
        _navigator = navigator;
    }

    public void OnNavigatedTo()
    {
        if (ShouldBounce)
        {
            _navigator.PreviousPage();
        }
    }
}
