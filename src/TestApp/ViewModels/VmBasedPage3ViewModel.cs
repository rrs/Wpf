using Rrs.Wpf;
using Rrs.Wpf.Navigation;
using System.Windows.Input;

namespace TestApp.ViewModels;

internal class VmBasedPage3ViewModel
{
    public ICommand NextPageCommand { get; }

    public ICommand CloseCommand { get; }

    public bool ShouldBounce { get; set; } = true;

    public VmBasedPage3ViewModel(IApplicationNavigator navigator)
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
                navigator.Close();
            }
            t.Change(TimeSpan.FromMilliseconds(600), TimeSpan.Zero);
        });

        NextPageCommand = new RelayCommand(_ => navigator.NextPage<VmBasedPage4ViewModel>(o =>
        {
            o.TimesPagedTo++;
            o.ShouldBounce = ShouldBounce;
        }));
    }
}
