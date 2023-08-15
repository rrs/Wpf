using System.Windows;
using System.Windows.Media;

namespace Rrs.Wpf;

public static class VisualTree
{
    /// <summary>
    /// Finds a visual child of a given type.
    /// http://msdn.microsoft.com/en-us/library/bb613579.aspx
    /// </summary>
    /// <typeparam name="T">The type to search for.</typeparam>
    /// <param name="obj">The object at the root of the tree to search.</param>
    /// <returns>The visual child.</returns>
    public static T FindVisualChild<T>(this DependencyObject obj)
        where T : DependencyObject
    {
        for (int i = 0; i < VisualTreeHelper.GetChildrenCount(obj); i++)
        {
            DependencyObject child = VisualTreeHelper.GetChild(obj, i);
            if (child != null && child is T)
            {
                return (T)child;
            }
            else
            {
                T childOfChild = FindVisualChild<T>(child);
                if (childOfChild != null)
                {
                    return childOfChild;
                }
            }
        }

        return null;
    }

    /// <summary>
    /// Finds a visual parent of a given type.
    /// http://msdn.microsoft.com/en-us/library/bb613579.aspx
    /// </summary>
    /// <typeparam name="T">The type to search for.</typeparam>
    /// <param name="obj">The object at the root of the tree to search.</param>
    /// <returns>The visual parent.</returns>
    public static T FindVisualParent<T>(this DependencyObject obj)
        where T : DependencyObject
    {
        DependencyObject parent = VisualTreeHelper.GetParent(obj);
        while (parent != null)
        {
            T typed = parent as T;
            if (typed != null)
            {
                return typed;
            }

            parent = VisualTreeHelper.GetParent(parent);
        }

        return null;
    }

    public static T FindChild<T>(this DependencyObject parent, string childName) where T : DependencyObject
    {
        // Confirm parent and childName are valid.
        if (parent == null) return null;
        T foundChild = null;
        int childrenCount = VisualTreeHelper.GetChildrenCount(parent);
        for (int i = 0; i < childrenCount; i++)
        {
            var child = VisualTreeHelper.GetChild(parent, i);
            // If the child is not of the request child type child
            T childType = child as T;
            if (childType == null)
            {
                // recursively drill down the tree
                foundChild = FindChild<T>(child, childName);
                // If the child is found, break so we do not overwrite the found child.
                if (foundChild != null) break;
            }
            else if (!string.IsNullOrEmpty(childName))
            {
                var frameworkElement = child as FrameworkElement;
                // If the child's name is set for search
                if (frameworkElement != null && frameworkElement.Name == childName)
                {
                    // if the child's name is of the request name
                    foundChild = (T)child;
                    break;
                }
            }
            else
            {
                // child element found.
                foundChild = (T)child;
                break;
            }
        }
        return foundChild;
    }
}
