namespace Rrs.Wpf.Navigation;

public class NavigationAreaNavigator : INavigator
{
    private readonly HashSet<NavigationArea> _attachedNavigationAreas = [];

    public void Attach(NavigationArea n)
    {
        _attachedNavigationAreas.Add(n);
    }

    public void Detach(NavigationArea n)
    {
        _attachedNavigationAreas.Remove(n);
    }

    public void NextPage<TPage>(Action<TPage> pageAction, bool addCurrentToHistory = true)
    {
        foreach (var n in _attachedNavigationAreas) n.NextPage(NavigationParameters.Create(pageAction, addCurrentToHistory));
    }

    public void NextPage<TPage>(bool addCurrentToHistory = true)
    {
        foreach (var n in _attachedNavigationAreas) n.NextPage(NavigationParameters.Create<TPage>(addCurrentToHistory));
    }

    public void NextPage(Type pageType, bool addCurrentToHistory = true)
    {
        foreach (var n in _attachedNavigationAreas) n.NextPage(new NavigationParameters(pageType, addCurrentToHistory));
    }

    public void PreviousPage()
    {
        foreach (var n in _attachedNavigationAreas) n.PreviousPage();
    }

    public void FirstPage()
    {
        foreach (var n in _attachedNavigationAreas) n.FirstPage();
    }

    public Task NextPageAsync<TPage>(Action<TPage> pageAction, bool addCurrentToHistory = true)
    {
        return ApplyToAllAsync(_attachedNavigationAreas, n => n.NextPage(NavigationParameters.Create(pageAction, addCurrentToHistory)));
    }

    public Task NextPageAsync<TPage>(bool addCurrentToHistory = true)
    {
        return ApplyToAllAsync(_attachedNavigationAreas, n => n.NextPage(NavigationParameters.Create<TPage>(addCurrentToHistory)));
    }

    public Task NextPageAsync(Type pageType, bool addCurrentToHistory = true)
    {
        return ApplyToAllAsync(_attachedNavigationAreas, n => n.NextPage(new NavigationParameters(pageType, addCurrentToHistory)));
    }

    public Task PreviousPageAsync()
    {
        return ApplyToAllAsync(_attachedNavigationAreas, n => n.PreviousPage());
    }

    public Task FirstPageAsync()
    {
        return ApplyToAllAsync(_attachedNavigationAreas, n => n.FirstPage());
    }

    public INavigatorPresenterData<T> With<T>(T data)
    {
        return new NavigationAreaPresenterData<T>(_attachedNavigationAreas, data);
    }

    public static Task ApplyToAllAsync(IEnumerable<NavigationArea> navigationAreas, Action<NavigationArea> navigationAction)
    {
        var tasks = new List<Task>();
        foreach (var n in navigationAreas) tasks.Add(n.Dispatcher.InvokeAsync(() => navigationAction(n)).Task);
        return Task.WhenAll(tasks);
    }

    public class NavigationAreaPresenterData<TData> : INavigatorPresenterData<TData>
    {
        private readonly IEnumerable<NavigationArea> _navigationAreas;
        private readonly TData _data;

        public NavigationAreaPresenterData(IEnumerable<NavigationArea> navigationAreas, TData data)
        {
            _navigationAreas = navigationAreas;
            _data = data;
        }

        public void NextPage<TPresenter>(bool addCurrentToHistory = true) where TPresenter : IPresenter<TData>
        {
            foreach (var n in _navigationAreas) n.NextPage(NavigationParameters.Create<TPresenter, TData>(_data, addCurrentToHistory));
        }

        public Task NextPageAsync<TPresenter>(bool addCurrentToHistory = true) where TPresenter : IPresenter<TData>
        {
            return ApplyToAllAsync(_navigationAreas, n => n.NextPage(NavigationParameters.Create<TPresenter, TData>(_data, addCurrentToHistory)));
        }
    }
}