using System.Globalization;
using System.Windows.Data;

namespace Rrs.Wpf.Converters;

public enum MathOperation
{
    Add,
    Subtract,
    Multiply,
    Divide
}

public sealed class MathConverter : IValueConverter
{
    public MathOperation Operation { get; set; }
    public double? Min { get; set; }
    public double? Max { get; set; }

    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        try
        {
            double value1 = System.Convert.ToDouble(value, CultureInfo.InvariantCulture);
            double value2 = System.Convert.ToDouble(parameter, CultureInfo.InvariantCulture);
            switch (Operation)
            {
                case MathOperation.Add:
                    return Limit(value1 + value2);
                case MathOperation.Divide:
                    return Limit(value1 / value2);
                case MathOperation.Multiply:
                    return Limit(value1 * value2);
                case MathOperation.Subtract:
                    return Limit(value1 - value2);
                default:
                    return Binding.DoNothing;
            }
            double Limit(double value)
            {
                if (Min.HasValue && value < Min.Value) return Min.Value;
                if (Max.HasValue && value > Max.Value) return Max.Value;
                return value;
            }

        }
        catch (FormatException)
        {
            return Binding.DoNothing;
        }
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        return Binding.DoNothing;
    }
}
