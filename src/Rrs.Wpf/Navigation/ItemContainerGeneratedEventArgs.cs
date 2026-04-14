using System.Windows;

namespace Rrs.Wpf.Navigation;

public delegate void ItemContainerGeneratedEventHandler(object sender, ItemContainerGeneratedEventArgs e);
public class ItemContainerGeneratedEventArgs : RoutedEventArgs
{
    public FrameworkElement? ItemContainer { get; }
    public ItemContainerGeneratedEventArgs(RoutedEvent routedEvent, FrameworkElement? itemContainer) : base(routedEvent)
    {
        ItemContainer = itemContainer;
    }
}
