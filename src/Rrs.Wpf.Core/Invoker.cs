using System;
using System.Windows;

namespace Rrs.Wpf.Core
{
    public static class Invoker
    {
        public static void BeginInvoke(Action a)
        {
            Application.Current.Dispatcher.BeginInvoke(a);
        }
    }
}
