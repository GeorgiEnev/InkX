using Avalonia.Media;
using System;

namespace Overlay.App.ColorPicker;

public static class HsvColor
{
    public static Color ToColor(double h, double s, double v)
    {
        double c = v * s;
        double x = c * (1 - Math.Abs((h / 60) % 2 - 1));
        double m = v - c;

        double r = 0, g = 0, b = 0;

        if (h < 60) { r = c; g = x; }
        else if (h < 120) { r = x; g = c; }
        else if (h < 180) { g = c; b = x; }
        else if (h < 240) { g = x; b = c; }
        else if (h < 300) { r = x; b = c; }
        else { r = c; b = x; }

        return Color.FromArgb(
            255,
            (byte)((r + m) * 255),
            (byte)((g + m) * 255),
            (byte)((b + m) * 255)
        );
    }
}
