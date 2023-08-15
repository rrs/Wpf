using System.Globalization;
using System.Windows.Controls;

namespace Rrs.Wpf.ValidationRules;

public class NotNullValidationRule : ValidationRule
{
    public override ValidationResult Validate(object value, CultureInfo cultureInfo)
    {
        if (value is null) return new ValidationResult(false, "Value can not be null");
        return ValidationResult.ValidResult;
    }
}
