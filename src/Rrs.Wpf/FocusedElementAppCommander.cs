using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;

namespace Rrs.Wpf;

public class FocusedElementAppCommander : IApplicationCommander
{
    private readonly DependencyObject _focusScope;

    public Dispatcher Dispatcher { get; }

    public FocusedElementAppCommander(DependencyObject focusScope, Dispatcher dispatcher)
    {
        _focusScope = focusScope;
        Dispatcher = dispatcher;
    }

    public FocusedElementAppCommander(DependencyObject focusScope) : this(focusScope, focusScope.Dispatcher) { }

    public void Close(bool? dialogResult = null)
    {
        ApplicationCommands.Close.Execute(dialogResult, FocusManager.GetFocusedElement(_focusScope));
    }

    public Task CloseAsync(bool? dialogResult = null)
    {
        return Dispatcher.InvokeAsync(() => Close(dialogResult)).Task;
    }
}
