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
        _dispatcher = focusScope.Dispatcher;
    }

    public FocusedElementNavigator(DependencyObject focusScope) : this(focusScope, focusScope.Dispatcher) { }

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

    public void NextPage(string pageTypeName, bool addCurrentToHistory = true)
    {
        NavigationCommands.NextPage.Execute(new NavigationParameters(pageTypeName, addCurrentToHistory), FocusManager.GetFocusedElement(_focusScope));
    }

    public void NextPage(bool addCurrentToHistory = true)
    {
        NavigationCommands.NextPage.Execute(new NavigationParameters(addCurrentToHistory), FocusManager.GetFocusedElement(_focusScope));
    }

    public void GoToPage<TPage>(Action<TPage> pageAction)
    {
        ((ICommand)NavigationCommands.GoToPage).Execute(NavigationParameters.Create(pageAction));
        //NavigationCommands.GoToPage.Execute(NavigationParameters.Create(pageAction), FocusManager.GetFocusedElement(_focusScope));
    }

    public void GoToPage(Type pageType)
    {
        NavigationCommands.GoToPage.Execute(new NavigationParameters(pageType), FocusManager.GetFocusedElement(_focusScope));
    }

    public void GoToPage(string pageTypeName)
    {
        NavigationCommands.GoToPage.Execute(new NavigationParameters(pageTypeName), FocusManager.GetFocusedElement(_focusScope));
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

    public Task NextPageAsync(string pageTypeName, bool addCurrentToHistory = true)
    {
        return _dispatcher.InvokeAsync(() => NextPage(pageTypeName, addCurrentToHistory)).Task;
    }

    public Task NextPageAsync(bool addCurrentToHistory = true)
    {
        return _dispatcher.InvokeAsync(() => NextPage(addCurrentToHistory)).Task;
    }

    public Task GoToPageAsync<TPage>(Action<TPage> pageAction)
    {
        return _dispatcher.InvokeAsync(() => GoToPage(pageAction)).Task;
    }

    public Task GoToPageAsync(Type pageType)
    {
        return _dispatcher.InvokeAsync(() => GoToPage(pageType)).Task;
    }

    public Task GoToPageAsync(string pageTypeName)
    {
        return _dispatcher.InvokeAsync(() => GoToPage(pageTypeName)).Task;
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
