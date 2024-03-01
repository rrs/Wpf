using System.Globalization;
using System.Windows.Controls;

namespace Rrs.Wpf.ValidationRules;

public class NumberValidationRule : ValidationRule
{
    public decimal? Min { get; set; }
    public decimal? Max { get; set; }

    public string? MinErrorFormatString { get; set; }
    public string? MaxErrorFormatString { get; set; }

    public override ValidationResult Validate(object value, CultureInfo cultureInfo)
    {
        value = BindingHelper.ResolveValue(value);
        
        if (decimal.TryParse(value?.ToString(), out var d))
        {
            if (Min.HasValue && d < Min) return new ValidationResult(false, string.Format(MinErrorFormatString ?? "Value can not be less than {0}", Min.Value));
            if (Max.HasValue && d > Max) return new ValidationResult(false, string.Format(MaxErrorFormatString ?? "Value can not be more than {0}", Max.Value));
            return ValidationResult.ValidResult;
        }
        else if (value == null)
        {
            return new ValidationResult(false, $"Value is required");
        }
        else
        {
            return new ValidationResult(false, $"Value is not a number");
        }
    }
}
