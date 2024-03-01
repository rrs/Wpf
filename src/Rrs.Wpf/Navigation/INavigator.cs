namespace Rrs.Wpf.Navigation;

public interface INavigator
{
    INavigatorPresenterData<T> With<T>(T data);

    void NextPage<TPage>(Action<TPage> pageAction, bool addCurrentToHistory = true);
    void NextPage<TPage>(bool addCurrentToHistory = true);
    void NextPage(Type pageType, bool addCurrentToHistory = true);
    void NextPage(string pageTypeName, bool addCurrentToHistory = true);
    void NextPage(bool addCurrentToHistory = true);
    void GoToPage<TPage>(Action<TPage> pageAction);
    void GoToPage(Type pageType);
    void GoToPage(string pageTypeName);
    void PreviousPage();
    void FirstPage();

    Task NextPageAsync<TPage>(Action<TPage> pageAction, bool addCurrentToHistory = true);
    Task NextPageAsync<TPage>(bool addCurrentToHistory = true);
    Task NextPageAsync(Type pageType, bool addCurrentToHistory = true);
    Task NextPageAsync(string pageTypeName, bool addCurrentToHistory = true);
    Task NextPageAsync(bool addCurrentToHistory = true);
    Task GoToPageAsync<TPage>(Action<TPage> pageAction);
    Task GoToPageAsync(Type pageType);
    Task GoToPageAsync(string pageTypeName);
    Task PreviousPageAsync();
    Task FirstPageAsync();
}

public interface INavigatorPresenterData<TData>
{
    void NextPage<TPresenter>(bool addCurrentToHistory = true)
        where TPresenter : IPresenter<TData>;

    void GoToPage<TPresenter>()
        where TPresenter : IPresenter<TData>;

    Task NextPageAsync<TPresenter>(bool addCurrentToHistory = true)
        where TPresenter : IPresenter<TData>;

    Task GoToPageAsync<TPresenter>()
    where TPresenter : IPresenter<TData>;
}

