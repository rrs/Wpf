using System.Globalization;
using System.Text.RegularExpressions;
using System.Windows.Controls;

namespace Rrs.Wpf.ValidationRules;

public class EmailValidationRule : ValidationRule
{
    private static readonly Regex _emailPattern = new(@"\A(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?)\Z", RegexOptions.Compiled | RegexOptions.IgnoreCase);

    public override ValidationResult Validate(object value, CultureInfo cultureInfo)
    {
        return value is string s && _emailPattern.IsMatch(s)
            ? ValidationResult.ValidResult
            : new ValidationResult(false, "Invalid Email Address");
    }
}
