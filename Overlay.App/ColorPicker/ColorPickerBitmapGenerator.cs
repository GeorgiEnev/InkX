using Avalonia;
using Avalonia.Media.Imaging;
using Avalonia.Platform;

namespace Overlay.App.ColorPicker;

public static class ColorPickerBitmapGenerator
{
    public static WriteableBitmap Generate(int size, double hue)
    {
        var bitmap = new WriteableBitmap(
            new PixelSize(size, size),
            new Vector(96, 96),
            PixelFormat.Bgra8888,
            AlphaFormat.Unpremul);

        using var fb = bitmap.Lock();

        unsafe
        {
            uint* buffer = (uint*)fb.Address;

            for (int y = 0; y < size; y++)
                for (int x = 0; x < size; x++)
                {
                    double s = (double)x / (size - 1);
                    double v = 1 - (double)y / (size - 1);

                    var c = HsvColor.ToColor(hue, s, v);

                    buffer[y * size + x] =
                        ((uint)c.A << 24) |
                        ((uint)c.R << 16) |
                        ((uint)c.G << 8) |
                         c.B;
                }
        }

        return bitmap;
    }
}
