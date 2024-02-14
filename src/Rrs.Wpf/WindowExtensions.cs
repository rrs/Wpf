using System.Windows;
using System.Windows.Interop;

namespace Rrs.Wpf;

public static class WindowExtensions
{
    public static T OwnIt<T>(this T w, IntPtr ownerHandle) where T : Window
    {
        if (ownerHandle == IntPtr.Zero) return w;
        new WindowInteropHelper(w)
        {
            Owner = ownerHandle
        };
        return w;
    }
}