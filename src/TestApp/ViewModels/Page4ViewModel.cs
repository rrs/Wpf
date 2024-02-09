namespace TestApp.ViewModels;

internal class Page4ViewModel
{
    public List<object> ViewModels { get; } = new List<object>
    {
        new VmBasedPage1ViewModel(),
        new VmBasedPage2ViewModel(),
        new VmBasedPage3ViewModel(),
    };

    public string? Name { get; set; }
}
