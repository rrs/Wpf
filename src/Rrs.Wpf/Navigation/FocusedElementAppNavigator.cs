using System.Windows;
using System.Windows.Threading;

namespace Rrs.Wpf.Navigation;

public class FocusedElementAppNavigator: FocusedElementNavigator, IApplicationNavigator
{
    private readonly FocusedElementAppCommander _appCommander;
    public Dispatcher Dispatcher { get; }

    public FocusedElementAppNavigator(DependencyObject focusScope) : this(focusScope, focusScope.Dispatcher) { }
    public FocusedElementAppNavigator(DependencyObject focusScope, Dispatcher dispatcher) : base(focusScope, dispatcher)
    {
        _appCommander = new(focusScope);
        Dispatcher = dispatcher;
    }

    public void Close(bool? dialogResult = null) => _appCommander.Close(dialogResult);
    public Task CloseAsync(bool? dialogResult = null) => _appCommander.CloseAsync(dialogResult);
}
