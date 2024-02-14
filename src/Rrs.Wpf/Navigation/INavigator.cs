namespace Rrs.Wpf.Navigation;

public interface INavigator
{
    INavigatorPresenterData<T> With<T>(T data);

    void NextPage<TPage>(Action<TPage> pageAction, bool addCurrentToHistory = true);
    void NextPage<TPage>(bool addCurrentToHistory = true);
    void NextPage(Type pageType, bool addCurrentToHistory = true);
    void PreviousPage();
    void FirstPage();

    Task NextPageAsync<TPage>(Action<TPage> pageAction, bool addCurrentToHistory = true);
    Task NextPageAsync<TPage>(bool addCurrentToHistory = true);
    Task NextPageAsync(Type pageType, bool addCurrentToHistory = true);
    Task PreviousPageAsync();
    Task FirstPageAsync();
}

public interface INavigatorPresenterData<TData>
{
    void NextPage<TPresenter>(bool addCurrentToHistory = true)
        where TPresenter : IPresenter<TData>;

    Task NextPageAsync<TPresenter>(bool addCurrentToHistory = true)
        where TPresenter : IPresenter<TData>;
}

