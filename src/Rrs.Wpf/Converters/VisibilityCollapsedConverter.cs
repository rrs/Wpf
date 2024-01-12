using System.Windows;

namespace Rrs.Wpf.Converters;

public sealed class VisibilityCollapsedConverter : BooleanConverter<Visibility>
{
    public VisibilityCollapsedConverter() :
        base(Visibility.Visible, Visibility.Collapsed)
    { }
}
