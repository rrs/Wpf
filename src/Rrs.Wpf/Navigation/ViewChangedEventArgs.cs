using System.Windows;

namespace Rrs.Wpf.Navigation;

public delegate void ViewChangedEventHandler(object sender, ViewChangedEventArgs e);
public class ViewChangedEventArgs : RoutedEventArgs
{
    public FrameworkElement? View { get; }
    public ViewChangedEventArgs(RoutedEvent routedEvent, FrameworkElement? view) : base(routedEvent)
    {
        View = view;
    }
}
