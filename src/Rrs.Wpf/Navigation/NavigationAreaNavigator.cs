namespace Rrs.Wpf.Navigation;

public class NavigationAreaNavigator
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

    public void NextPage()
    {
        foreach (var n in _attachedNavigationAreas) n.NextPage();
    }

    public void NextPage<TPage>(Action<TPage> pageAction, bool addCurrentToHistory = true)
    {
        foreach (var n in _attachedNavigationAreas) n.NextPage(NavigationParameters.Create(pageAction, addCurrentToHistory));
    }

    public void NextPage<TPage>(bool addCurrentToHistory)
    {
        foreach (var n in _attachedNavigationAreas) n.NextPage(NavigationParameters.Create<TPage>(addCurrentToHistory));
    }

    public void NextPage(string viewModelName)
    {
        foreach (var n in _attachedNavigationAreas) n.NextPage(viewModelName);
    }

    public void PreviousPage()
    {
        foreach (var n in _attachedNavigationAreas) n.PreviousPage();
    }

    public void FirstPage()
    {
        foreach (var n in _attachedNavigationAreas) n.FirstPage();
    }
}