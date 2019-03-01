using System;
using System.Threading.Tasks;
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

        public static Task<T> InvokeAsync<T>(this Dispatcher dispatcher, Func<T> f)
        {
            var tcs = new TaskCompletionSource<T>();
            Application.Current.Dispatcher.BeginInvoke((Action)(() =>
            {
                try
                {
                    var v = f();
                    tcs.SetResult(v);
                }
                catch (Exception e)
                {
                    tcs.SetException(e);
                }
            }));

            return tcs.Task;
        }

        public static Task InvokeAsync(this Dispatcher dispatcher, Action a)
        {
            var tcs = new TaskCompletionSource<object>();
            Application.Current.Dispatcher.BeginInvoke((Action)(() =>
            {
                try
                {
                    a();
                    tcs.SetResult(null);
                }
                catch (Exception e)
                {
                    tcs.SetException(e);
                }
            }));

            return tcs.Task;
        }
    }
}
