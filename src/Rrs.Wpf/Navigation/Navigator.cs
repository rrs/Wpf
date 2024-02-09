using System.Windows.Input;

namespace Rrs.Wpf.Navigation;

public class Navigator
{
    public static PageData<T> With<T>(T data) => new(data);

    public static void NextPage<TPage>(Action<TPage> pageAction, bool addCurrentToHistory = true)
    {
        ((ICommand)NavigationCommands.NextPage).Execute(NavigationParameters.Create(pageAction, addCurrentToHistory));
    }

    public static void NextPage<TPage>(bool addCurrentToHistory = true)
    {
        ((ICommand)NavigationCommands.NextPage).Execute(NavigationParameters.Create<TPage>(addCurrentToHistory));
    }

    public static void NextPageAsync<TPage>(Action<TPage> pageAction, bool addCurrentToHistory = true)
    {
        Invoker.InvokeAsync(() => NextPage(pageAction, addCurrentToHistory));
    }

    public static void NextPageAsync<TPage>(bool addCurrentToHistory = true)
    {
        Invoker.InvokeAsync(() => NextPage<TPage>(addCurrentToHistory));
    }

    public static void PreviousPage()
    {
        ((ICommand)NavigationCommands.PreviousPage).Execute(null);
    }

    public static void PreviousPageAsync()
    {
        Invoker.InvokeAsync(() => PreviousPage());
    }

    public static void FirstPage()
    {
        ((ICommand)NavigationCommands.FirstPage).Execute(null);
    }

    public static void FirstPageAsync()
    {
        Invoker.InvokeAsync(() => PreviousPage());
    }

    public static void Close(bool? dialogResult = null)
    {
        ((ICommand)ApplicationCommands.Close).Execute(dialogResult);
    }

    public static void CloseAsync(bool? dialogResult = null)
    {
        Invoker.InvokeAsync(() => Close(dialogResult));
    }

    public class PageData<TData>
    {
        private readonly TData _data;

        public PageData(TData data) => _data = data;

        public void NextPage<TPresenter>(bool addCurrentToHistory = true)
            where TPresenter : IPresenter<TData>
        {
            ((ICommand)NavigationCommands.NextPage).Execute(NavigationParameters.Create<TPresenter, TData>(_data, addCurrentToHistory));
        }

        public void NextPageAsync<TPresenter>(bool addCurrentToHistory = true)
            where TPresenter : IPresenter<TData>
        {
            Invoker.InvokeAsync(() => NextPage<TPresenter>(addCurrentToHistory));
        }
    }
}
