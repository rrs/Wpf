using Rrs.Wpf.Navigation.Transitions;
using System.Windows;

namespace Rrs.Wpf.Navigation;

public class NavigationParameters
{
    public string? PageTypeName { get; set; }
    public Type? PageType { get; set; }
    public string? FrameworkElementName { get; set; }
    public object? PresenterArgs { get; set; }
    public object? NavigationArgs { get; set; }
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

    public NavigationParameters(bool addCurrentPageToHistory)
    {
        AddCurrentPageToHistory = addCurrentPageToHistory;
    }

    public NavigationParameters(Type pageType, bool addCurrentPageToHistory)
    {
        PageType = pageType;
        AddCurrentPageToHistory = addCurrentPageToHistory;
    }

    public NavigationParameters(string pageTypeName, bool addCurrentPageToHistory = true)
    {
        PageTypeName = pageTypeName;
        AddCurrentPageToHistory = addCurrentPageToHistory;
    }

    public static NavigationParameters Create<TViewModel>(Action<TViewModel> pageAction, bool addCurrentPageToHistory = true)
        => new()
        {
            PageType = typeof(TViewModel),
            ViewModelPageAction = o => pageAction.Invoke((TViewModel)o),
            AddCurrentPageToHistory = addCurrentPageToHistory
        };

    public static NavigationParameters Create<TPresenter, TViewModel>(TViewModel presenterArgs, bool addCurrentPageToHistory = true)
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
