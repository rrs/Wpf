using System.Windows;
using System.Windows.Data;
using System.Windows.Threading;

namespace Rrs.Wpf;

public class BindingProxy : FrameworkElement
{
    public static readonly DependencyProperty InProperty =
        DependencyProperty.Register(nameof(In), typeof(object), typeof(BindingProxy), new FrameworkPropertyMetadata(InPropertyChanged)
        {
            BindsTwoWayByDefault = false,
            DefaultUpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged
        });

    private static void InPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (BindingOperations.GetBinding(d, OutProperty) != null)
            ((BindingProxy)d).Out = e.NewValue;
    }

    public object In
    {
        get => GetValue(InProperty);
        set => SetValue(InProperty, value);
    }

    public static readonly DependencyProperty OutProperty =
        DependencyProperty.Register(nameof(Out), typeof(object), typeof(BindingProxy), new FrameworkPropertyMetadata(OutPropertyChanged)
        {
            BindsTwoWayByDefault = true,
            DefaultUpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged
        });

    private static void OutPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        var source = DependencyPropertyHelper.GetValueSource(d, e.Property);

        var proxy = (BindingProxy)d;
        var expected = proxy.In;
        if (!ReferenceEquals(e.NewValue, expected))
        {
            proxy.Dispatcher.BeginInvoke(DispatcherPriority.DataBind, () =>
            {
                proxy.Out = proxy.In;
            });
        }
    }

    public object Out
    {
        get => GetValue(OutProperty);
        set => SetValue(OutProperty, value);
    }

    public BindingProxy()
    {
        Visibility = Visibility.Collapsed;
    }
}
