using Rrs.Wpf;

namespace TestApp.ViewModels;

internal class VmBasedPage4ViewModel : ViewModel
{
    private int _timesPagedTo;

    public int TimesPagedTo
    {
        get => _timesPagedTo;
        set
        {
            _timesPagedTo = value;
            OnPropertyChanged(nameof(TimesPagedTo));
        }
    }
}
