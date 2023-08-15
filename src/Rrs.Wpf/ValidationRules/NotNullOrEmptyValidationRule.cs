using System.Globalization;
using System.Windows.Controls;

namespace Rrs.Wpf.ValidationRules;

public class NotNullOrEmptyValidationRule : ValidationRule
{
    public bool AllowWhiteSpace { get; set; }

    public override ValidationResult Validate(object value, CultureInfo cultureInfo)
    {
        if (value is string s)
        {
            if (AllowWhiteSpace && !string.IsNullOrEmpty(s)) return ValidationResult.ValidResult;
            if (!string.IsNullOrWhiteSpace(s)) return ValidationResult.ValidResult;
        }
        return new ValidationResult(false, $"Value can not be null or empty{(AllowWhiteSpace ? "" : " or whitespace")}");
    }
}
