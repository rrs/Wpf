﻿using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Markup;

namespace Rrs.Wpf.Converters;

public class IsNullConverter : MarkupExtension, IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        => value == null;

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        => Binding.DoNothing;

    public override object ProvideValue(IServiceProvider serviceProvider)
        => this;
}
