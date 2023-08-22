using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Windows.Controls;
using System.Windows.Data;
using System.Xml.Linq;

namespace Rrs.Wpf.ValidationRules;

public class NumberValidationRule : ValidationRule
{
    public decimal? Min { get; set; }
    public decimal? Max { get; set; }

    public string ErrorNumberStringFormat { get; set; }

    public override ValidationResult Validate(object value, CultureInfo cultureInfo)
    {
        if (value is BindingExpression binding)
        {
            value = GetterResolver.Get(binding.ResolvedSource, binding.ResolvedSourcePropertyName);
        }
        
        if (decimal.TryParse(value?.ToString(), out var d))
        {
            if (Min.HasValue && d < Min) return new ValidationResult(false, $"Value can not be less than {FormatNumber(Min.Value, ErrorNumberStringFormat)}");
            if (Max.HasValue && d > Max) return new ValidationResult(false, $"Value can not be more than {FormatNumber(Max.Value, ErrorNumberStringFormat)}");
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

        static string FormatNumber(decimal value, string formatString) => string.IsNullOrWhiteSpace(formatString) ? value.ToString() : value.ToString(formatString);
    }

    private static class GetterResolver
    {
        private static readonly Dictionary<Type, Dictionary<string, Func<object, object>>> _getters = new();

        public static object Get(object source, string propertyName)
        {
            var sourceType = source.GetType();
            if (!_getters.ContainsKey(sourceType))
            {
                var properties = sourceType.GetProperties(BindingFlags.Public | BindingFlags.Instance);
                var getters = new Dictionary<string, Func<object, object>>();
                foreach(var prop in properties)
                {
                    var parameterExpression = Expression.Parameter(typeof(object));
                    var body = Expression.Convert(Expression.Property(Expression.Convert(parameterExpression, sourceType), propertyName), typeof(object));
                    var lambda = Expression.Lambda<Func<object, object>>(body, parameterExpression);
                    getters.Add(prop.Name, lambda.Compile());
                }
                _getters[sourceType] = getters;
            }

            return _getters[sourceType][propertyName](source);
        }
    }
}
