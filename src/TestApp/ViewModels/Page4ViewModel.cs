using Rrs.Wpf;

namespace TestApp.ViewModels;

internal class Page4ViewModel
{
    public Page4ViewModel(IApplicationCommander appCommander)
    {
        ViewModels = new List<object>
        {
            new VmBasedPage1ViewModel(),
            new VmBasedPage2ViewModel(),
            new VmBasedPage3ViewModel(appCommander),
        };
    }

    public List<object> ViewModels { get; }

    public string? Name { get; set; }
}
