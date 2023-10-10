using Rrs.Types;
using System.Windows.Data;

namespace Rrs.Wpf;

public static class BindingHelper
{
    public static object ResolveValue(object value)
    {
        if (value is BindingExpression binding)
        {
            return binding.ResolvedSource.GetRuntimeProperty(binding.ResolvedSourcePropertyName);
        }

        return value;
    }
}
