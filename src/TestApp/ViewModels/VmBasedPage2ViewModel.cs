using Rrs.Wpf;
using Rrs.Wpf.Navigation;

namespace TestApp.ViewModels;

internal class VmBasedPage2ViewModel : ViewModel, INavigateTo
{
    private int timesNavigatedTo;

    public int TimesNavigatedTo
    {
        get => timesNavigatedTo;
        set
        {
            timesNavigatedTo = value;
            OnPropertyChanged(nameof(TimesNavigatedTo));
        }
    }

    public void OnNavigatedTo() => TimesNavigatedTo++;
}
