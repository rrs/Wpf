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

    public override ValidationResult Validate(object value, CultureInfo cultureInfo)
    {
        if (value is BindingExpression binding)
        {
            //value = binding.ResolvedSource.GetType().GetProperty(binding.ResolvedSourcePropertyName).GetValue(binding.ResolvedSource);
            value = GetterResolver.Get(binding.ResolvedSource, binding.ResolvedSourcePropertyName);
        }
        
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
                //var getters = properties.ToDictionary(o => o.Name, o => (Func<object, object>)Delegate.CreateDelegate(typeof(Func<object, object>), o.GetGetMethod()));
                _getters[sourceType] = getters;
            }

            return _getters[sourceType][propertyName](source);
        }
    }

    //private interface IGetterResolver
    //{

    //}

    //private class DelegateResolver<T>
    //{
    //    private static IDictionary<string, Func<T, object>> _propertyGetters;
        
    //    static DelegateResolver()
    //    {
    //        var properties = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);

    //        _propertyGetters = properties.ToDictionary(o => o.Name, o => (Func<T, object>)Delegate.CreateDelegate(typeof(Func<T, object>), o.GetGetMethod()));
    //    }

    //    public object Get(T source, string property)
    //    {
    //        return _propertyGetters[property](source);
    //    }
    //}

}
