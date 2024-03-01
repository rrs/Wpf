using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;

namespace Rrs.Wpf.Navigation;

public class NavigatorPresenterData<TData> : INavigatorPresenterData<TData>
{
    private readonly DependencyObject _focusScope;
    private readonly Dispatcher _dispatcher;
    private readonly TData _data;

    public NavigatorPresenterData(DependencyObject focusScope, Dispatcher dispatcher, TData data)
    {
        _focusScope = focusScope;
        _dispatcher = dispatcher;
        _data = data;
    }

    public void NextPage<TPresenter>(bool addCurrentToHistory = true)
        where TPresenter : IPresenter<TData>
    {
        NavigationCommands.NextPage.Execute(NavigationParameters.Create<TPresenter, TData>(_data, addCurrentToHistory), FocusManager.GetFocusedElement(_focusScope));
    }

    public void GoToPage<TPresenter>()
        where TPresenter : IPresenter<TData>
    {
        NavigationCommands.GoToPage.Execute(NavigationParameters.Create<TPresenter, TData>(_data), FocusManager.GetFocusedElement(_focusScope));
    }

    public Task NextPageAsync<TPresenter>(bool addCurrentToHistory = true)
        where TPresenter : IPresenter<TData>
    {
        return _dispatcher.InvokeAsync(() => NextPage<TPresenter>(addCurrentToHistory)).Task;
    }

    public Task GoToPageAsync<TPresenter>()
        where TPresenter : IPresenter<TData>
    {
        return _dispatcher.InvokeAsync(() => GoToPage<TPresenter>()).Task;
    }
}