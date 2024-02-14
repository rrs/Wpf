namespace Rrs.Wpf;

public interface IApplicationCommander
{
    void Close(bool? dialogResult = null);
    Task CloseAsync(bool? dialogResult = null);
}
