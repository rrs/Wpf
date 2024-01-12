using System.Windows;

namespace Rrs.Wpf.Converters;

public class VisibilityHiddenConverter : BooleanConverter<Visibility>
{
    public VisibilityHiddenConverter()
        : base(Visibility.Visible, Visibility.Hidden) { }
}
