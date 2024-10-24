using System.Windows.Threading;

namespace Rrs.Wpf;

public interface IApplicationCommander
{
    public Dispatcher Dispatcher { get; }
    void Close(bool? dialogResult = null);
    Task CloseAsync(bool? dialogResult = null);
}
