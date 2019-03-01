using System;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;

namespace Rrs.Wpf.Core
{
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
            return (T)Application.Current.Dispatcher.Invoke(func);
        }
    }
}
