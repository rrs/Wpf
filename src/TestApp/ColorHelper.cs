using System.Windows.Media;

namespace TestApp;

internal static class ColorHelper
{
    public static Color ContrastingForegroundColor(this Color color)
    {
        double RgbToSrgb(double d)
        {
            d /= 255.0;
            return (d > 0.03928)
                ? Math.Pow((d + 0.055) / 1.055, 2.4)
                : d / 12.92;
        }
        var r = RgbToSrgb(color.R);
        var g = RgbToSrgb(color.G);
        var b = RgbToSrgb(color.B);

        var luminance = 0.2126 * r + 0.7152 * g + 0.0722 * b;
        return luminance > 0.179 ? Colors.Black : Colors.White;
    }
}
