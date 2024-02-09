using System.Windows;
using System.Windows.Markup;

namespace Rrs.Wpf.Navigation;

public class NavigationTarget : MarkupExtension
{
    private readonly string _navigationAreaName;

    public NavigationTarget(string navigationAreaName)
    {
        _navigationAreaName = navigationAreaName;
    }

    public override object? ProvideValue(IServiceProvider serviceProvider)
    {
        if (serviceProvider.GetService(typeof(IProvideValueTarget)) is not IProvideValueTarget provideValueTarget) return null;

        var targetObject = (FrameworkElement)provideValueTarget.TargetObject;
        var dependencyProperty = (DependencyProperty)provideValueTarget.TargetProperty;
        var targetProperty = targetObject.GetType().GetProperty(dependencyProperty.Name);
        if (targetObject == null || targetProperty == null) return targetObject;

        targetObject.Loaded += TargetObject_Loaded;

        return targetObject;

        void TargetObject_Loaded(object sender, RoutedEventArgs e)
        {
            FrameworkElement? navigationProxy = VisualTree.FindVisualParent<NavigationHost>(sender as DependencyObject);
            if (navigationProxy == null) navigationProxy = VisualTree.FindVisualParent<Window>(sender as DependencyObject);
            var navigationArea = VisualTree.FindChildBreadthSearch<NavigationArea>(navigationProxy, _navigationAreaName);
            if (navigationArea == null) return;
            targetProperty.SetValue(targetObject, navigationArea);
        }
    }
}
