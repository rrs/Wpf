using Rrs.Wpf;
using Rrs.Wpf.Navigation;

namespace TestApp.ViewModels;

internal class Page4ViewModel
{
    public Page4ViewModel(IApplicationNavigator navigator)
    {
        ViewModels = new List<object>
        {
            new VmBasedPage1ViewModel(),
            new VmBasedPage2ViewModel(),
            new VmBasedPage3ViewModel(navigator),
            new VmBasedPage4ViewModel(navigator)
        };
    }

    public List<object> ViewModels { get; }

    public string? Name { get; set; }
}
