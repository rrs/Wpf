using Rrs.Wpf.Navigation.Transitions;
using System.Windows.Markup;

namespace Rrs.Wpf.Navigation;

public class Navigation : MarkupExtension
{
    public string? PageName { get; set; }
    public Type? PageType { get; set; }
    public string? ElementName { get; set; }
    public object? Parameter { get; set; }
    public bool AddCurrentPageToHistory { get; set; } = true;
    public ITransition? ForwardsTransition { get; set; }
    public ITransition? BackwardsTransition { get; set; }

    public Navigation() { }

    public Navigation(string name)
    {
        PageName = name;
    }

    public override object ProvideValue(IServiceProvider serviceProvider)
    {
        return new NavigationParameters
        {
            PageTypeName = PageName,
            PageType = PageType,
            FrameworkElementName = ElementName,
            PresenterArgs = Parameter,
            AddCurrentPageToHistory = AddCurrentPageToHistory,
            ForwardsTransition = ForwardsTransition,
            BackwardsTransition = BackwardsTransition,
        };
    }
}
