using Rrs.Wpf.Navigation.Transitions;

namespace Rrs.Wpf.Navigation;

public class NavigationParameters
{
    public string? PageTypeName { get; set; }
    public Type? PageType { get; set; }
    public string? FrameworkElementName { get; set; }
    public object? PresenterArgs { get; set; }
    public bool AddCurrentPageToHistory { get; set; } = true;
    public Action<object>? ViewModelPageAction { get; set; }
    public ITransition? ForwardsTransition { get; set; }
    public ITransition? BackwardsTransition { get; set; }

    public NavigationParameters() { }

    public NavigationParameters(Type pageType, string? elementName = null)
    {
        PageType = pageType;
        FrameworkElementName = elementName;
    }

    public static NavigationParameters Create<TViewModel>(Action<TViewModel> pageAction, bool addCurrentPageToHistory)
        => new()
        {
            PageType = typeof(TViewModel),
            ViewModelPageAction = o => pageAction.Invoke((TViewModel)o),
            AddCurrentPageToHistory = addCurrentPageToHistory
        };

    public static NavigationParameters Create<TPresenter, TViewModel>(TViewModel presenterArgs, bool addCurrentPageToHistory)
            where TPresenter : IPresenter<TViewModel>
        => new()
        {
            PageType = typeof(TPresenter),
            PresenterArgs = presenterArgs,
            AddCurrentPageToHistory = addCurrentPageToHistory
        };

    public static NavigationParameters Create<TPage>(bool addCurrentPageToHistory)
        => new()
        {
            PageType = typeof(TPage),
            AddCurrentPageToHistory = addCurrentPageToHistory
        };
}
