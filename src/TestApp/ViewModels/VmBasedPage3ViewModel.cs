using Rrs.Wpf;
using System.Windows.Input;

namespace TestApp.ViewModels;

internal class VmBasedPage3ViewModel
{
    public ICommand CloseCommand { get; }

    public VmBasedPage3ViewModel(IApplicationCommander appCommander)
    {
        int clicks = 0;
        var t = new Timer(_ =>
        {
            Interlocked.Exchange(ref clicks, 0);
        });
        CloseCommand = new RelayCommand(_ =>
        {
            var currentClicks = Interlocked.Increment(ref clicks);
            if (clicks > 5)
            {
                appCommander.Close();
            }
            t.Change(TimeSpan.FromMilliseconds(600), TimeSpan.Zero);
        });
    }
}
