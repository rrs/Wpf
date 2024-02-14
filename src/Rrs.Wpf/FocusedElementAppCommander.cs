using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;

namespace Rrs.Wpf;

public class FocusedElementAppCommander : IApplicationCommander
{
    private readonly DependencyObject _focusScope;
    private readonly Dispatcher _dispatcher;

    public FocusedElementAppCommander(DependencyObject focusScope, Dispatcher dispatcher)
    {
        _focusScope = focusScope;
        _dispatcher = dispatcher;
    }

    public void Close(bool? dialogResult = null)
    {
        ApplicationCommands.Close.Execute(dialogResult, FocusManager.GetFocusedElement(_focusScope));
    }

    public Task CloseAsync(bool? dialogResult = null)
    {
        return _dispatcher.InvokeAsync(() => Close(dialogResult)).Task;
    }
}
