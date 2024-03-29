﻿using System.Windows;
using System.Windows.Controls;

namespace Rrs.Wpf;

public static class ScrollViewerAssist
{
    public static readonly DependencyProperty AutoScrollProperty = DependencyProperty.RegisterAttached("AutoScroll", typeof(bool), typeof(ScrollViewerAssist), new PropertyMetadata(false, AutoScrollPropertyChanged));

    public static bool GetAutoScroll(DependencyObject obj)
    {
        return (bool)obj.GetValue(AutoScrollProperty);
    }

    public static void SetAutoScroll(DependencyObject obj, bool value)
    {
        obj.SetValue(AutoScrollProperty, value);
    }

    public static void AutoScrollPropertyChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args)
    {
        if (obj is ScrollViewer scrollViewer)
        {
            if ((bool)args.NewValue)
            {
                scrollViewer.ScrollChanged += ScrollViewer_ScrollChanged;
                scrollViewer.ScrollToEnd();
            }
            else
            {
                scrollViewer.ScrollChanged -= ScrollViewer_ScrollChanged;
            }
        }
    }

    private static void ScrollViewer_ScrollChanged(object sender, ScrollChangedEventArgs e)
    {
        // Only scroll to bottom when the extent changed. Otherwise you can't scroll up
        if (e.ExtentHeightChange != 0)
        {
            var scrollViewer = sender as ScrollViewer;
            scrollViewer?.ScrollToBottom();
        }
    }

}
