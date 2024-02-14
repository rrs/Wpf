using System.Windows;
using System.Windows.Threading;

namespace Rrs.Wpf.Navigation;

public class FocusedElementAppNavigator(DependencyObject focusScope, Dispatcher dispatcher) : FocusedElementNavigator(focusScope, dispatcher), IApplicationNavigator
{
    private readonly FocusedElementAppCommander _appCommander = new(focusScope, dispatcher);

    public void Close(bool? dialogResult = null) => _appCommander.Close(dialogResult);
    public Task CloseAsync(bool? dialogResult = null) => _appCommander.CloseAsync(dialogResult);
}
