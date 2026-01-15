using Avalonia.Media;
using System;

namespace Overlay.App.Drawing;

public sealed class BrushState
{
    public double Size { get; private set; } = 2;
    public IBrush Brush { get; private set; } = Brushes.Black;

    public const double MinSize = 2;
    public const double MaxSize = 50;
    public const double Step = 2;

    public void SetSize(double value)
    {
        Size = Math.Clamp(
            Math.Round(value / Step) * Step,
            MinSize,
            MaxSize
        );
    }

    public void SetColor(Color color)
    {
        Brush = new SolidColorBrush(color);
    }
}
