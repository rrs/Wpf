using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;

namespace Rrs.Wpf.Navigation;

public class FocusedElementNavigator : INavigator
{
    private readonly DependencyObject _focusScope;
    private readonly Dispatcher _dispatcher;

    public FocusedElementNavigator(DependencyObject focusScope, Dispatcher dispatcher)
    {
        _focusScope = focusScope;
        _dispatcher = dispatcher;
    }

    public INavigatorPresenterData<T> With<T>(T data) => new NavigatorPresenterData<T>(_focusScope, _dispatcher, data);

    public void NextPage<TPage>(Action<TPage> pageAction, bool addCurrentToHistory = true)
    {
        NavigationCommands.NextPage.Execute(NavigationParameters.Create(pageAction, addCurrentToHistory), FocusManager.GetFocusedElement(_focusScope));
    }

    public void NextPage<TPage>(bool addCurrentToHistory = true)
    {
        NavigationCommands.NextPage.Execute(NavigationParameters.Create<TPage>(addCurrentToHistory), FocusManager.GetFocusedElement(_focusScope));
    }

    public void NextPage(Type pageType, bool addCurrentToHistory = true)
    {
        NavigationCommands.NextPage.Execute(new NavigationParameters(pageType, addCurrentToHistory), FocusManager.GetFocusedElement(_focusScope));
    }

    public void PreviousPage()
    {
        NavigationCommands.PreviousPage.Execute(null, FocusManager.GetFocusedElement(_focusScope));
    }

    public void FirstPage()
    {
        NavigationCommands.FirstPage.Execute(null, FocusManager.GetFocusedElement(_focusScope));
    }

    public Task NextPageAsync<TPage>(Action<TPage> pageAction, bool addCurrentToHistory = true)
    {
        return _dispatcher.InvokeAsync(() => NextPage(pageAction, addCurrentToHistory)).Task;
    }

    public Task NextPageAsync<TPage>(bool addCurrentToHistory = true)
    {
        return _dispatcher.InvokeAsync(() => NextPage<TPage>(addCurrentToHistory)).Task;
    }

    public Task NextPageAsync(Type pageType, bool addCurrentToHistory = true)
    {
        return _dispatcher.InvokeAsync(() => NextPage(pageType, addCurrentToHistory)).Task;
    }

    public Task PreviousPageAsync()
    {
        return _dispatcher.InvokeAsync(PreviousPage).Task;
    }

    public Task FirstPageAsync()
    {
        return _dispatcher.InvokeAsync(FirstPage).Task;
    }
}
