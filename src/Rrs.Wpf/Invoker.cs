﻿using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;

namespace Rrs.Wpf;

public static class Invoker
{
    public static void RequeryCommands()
    {
        Application.Current.Dispatcher.Invoke((Action)CommandManager.InvalidateRequerySuggested);
    }

    public static DispatcherOperation BeginInvoke(Action a)
    {
        return Application.Current.Dispatcher.BeginInvoke(a);
    }

    public static void Invoke(Action action)
    {
        Application.Current.Dispatcher.Invoke(action);
    }

    public static T Invoke<T>(Func<T> func)
    {
        return Application.Current.Dispatcher.Invoke(func);
    }

    public static DispatcherOperation<T> InvokeAsync<T>(Func<T> f)
    {
        return Application.Current.Dispatcher.InvokeAsync(f);
    }

    public static DispatcherOperation InvokeAsync(Action a)
    {
        return Application.Current.Dispatcher.InvokeAsync(a);
    }

    public static void InvokeAsyncIfRequired(Action a)
    {
        if (Application.Current.Dispatcher.CheckAccess())
        {
            a();
        }
        else
        {
            Application.Current.Dispatcher.InvokeAsync(a);
        }
    }

    public static async Task<T> InvokeAsyncIfRequired<T>(Func<T> f)
    {
        if (Application.Current.Dispatcher.CheckAccess())
        {
            return f();
        }
        else
        {
            var dispatcherOperation = await Application.Current.Dispatcher.InvokeAsync(f);
            return dispatcherOperation;
        }
    }
}
