using System.Globalization;
using System.Windows.Controls;

namespace Rrs.Wpf.ValidationRules;

public class NumberValidationRule : ValidationRule
{
    public decimal? Min { get; set; }
    public decimal? Max { get; set; }

    public override ValidationResult Validate(object value, CultureInfo cultureInfo)
    {
        if (decimal.TryParse(value?.ToString(), out var d))
        {
            if (Min.HasValue && d < Min) return new ValidationResult(false, $"Value can not be less than {Min}");
            if (Max.HasValue && d > Max) return new ValidationResult(false, $"Value can not be more than {Max}");
            return ValidationResult.ValidResult;
        }
        else if (value == null)
        {
            return new ValidationResult(false, $"Value can not be nothing");
        }
        else
        {
            return new ValidationResult(false, $"Value is not a number");
        }
    }
}
