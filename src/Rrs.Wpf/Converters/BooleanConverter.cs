using System;
using System.Collections.Generic;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Markup;

namespace Rrs.Wpf.Converters;

public abstract class BooleanConverter<T> : MarkupExtension, IValueConverter
{
    public BooleanConverter(T trueValue, T falseValue)
    {
        True = trueValue;
        False = falseValue;
    }

    public T? True { get; set; }
    public T? False { get; set; }
    public bool Inverted { get; set; }

    public virtual object? Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        return Inverted^(value is bool b && b) ? True : False;
    }

    public virtual object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        return Inverted^(value is T t && True != null && EqualityComparer<T>.Default.Equals(t, True));
    }

    public override object ProvideValue(IServiceProvider serviceProvider) => this;
}
