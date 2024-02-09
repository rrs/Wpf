﻿using System.Windows;

namespace Rrs.Wpf;

public static class LogicalTree
{
    public static T? FindLogicalChild<T>(this DependencyObject obj)
        where T : DependencyObject
    {
        var children = LogicalTreeHelper.GetChildren(obj);

        foreach (var o in LogicalTreeHelper.GetChildren(obj))
        {
            if (children is T t) return t;
            if (o is not DependencyObject child) continue;

            var childOfChild = FindLogicalChild<T>(child);
            if (childOfChild != null) return childOfChild;
        }

        return null;
    }

    public static T? FindLogicalParent<T>(this DependencyObject obj)
        where T : DependencyObject
    {
        var parent = LogicalTreeHelper.GetParent(obj);
        while (parent != null)
        {
            if (parent is T typed) return typed;
            parent = LogicalTreeHelper.GetParent(parent);
        }

        return null;
    }

    public static T? FindChild<T>(this DependencyObject? parent, string childName) where T : DependencyObject
    {
        // Confirm parent and childName are valid.
        if (parent == null) return null;
        T? foundChild = null;

        foreach (var o in LogicalTreeHelper.GetChildren(parent))
        {
            var child = o as DependencyObject;
            // If the child is not of the request child type child
            if (child is not T childType)
            {
                // recursively drill down the tree
                foundChild = FindChild<T>(child, childName);
                // If the child is found, break so we do not overwrite the found child.
                if (foundChild != null) break;
            }
            else if (!string.IsNullOrEmpty(childName))
            {
                // If the child's name is set for search
                if (child is FrameworkElement frameworkElement && frameworkElement.Name == childName)
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

