using Avalonia.Controls;
using Avalonia.Input;
using Overlay.App.Drawing;
using System;

namespace Overlay.App.ColorPicker;

public sealed class ColorPickerController
{
    private readonly Image image;
    private readonly Control dot;
    private readonly BrushState brush;
    private readonly int size;

    private double s = 1;
    private double v = 1;
    private bool picking;

    public double Hue { get; private set; } = 240;

    public ColorPickerController(Image image, Control dot, BrushState brush, int size)
    {
        this.image = image;
        this.dot = dot;
        this.brush = brush;
        this.size = size;

        Regenerate();
    }

    public void SetHue(double value)
    {
        Hue = 360 - value;
        Regenerate();
    }

    public void OnPressed(PointerPressedEventArgs e)
    {
        picking = true;
        Update(e);
    }

    public void OnMoved(PointerEventArgs e)
    {
        if (picking)
            Update(e);
    }

    public void OnReleased()
    {
        picking = false;
    }

    private void Regenerate()
    {
        image.Source = ColorPickerBitmapGenerator.Generate(size, Hue);
        Apply();
        UpdateDot();
    }

    private void Update(PointerEventArgs e)
    {
        var p = e.GetPosition(image);

        s = Math.Clamp(p.X / (size - 1), 0, 1);
        v = Math.Clamp(1 - p.Y / (size - 1), 0, 1);

        Apply();
        UpdateDot();
    }

    private void Apply()
    {
        brush.SetColor(HsvColor.ToColor(Hue, s, v));
    }

    private void UpdateDot()
    {
        Canvas.SetLeft(dot, s * (size - 1) - dot.Bounds.Width / 2);
        Canvas.SetTop(dot, (1 - v) * (size - 1) - dot.Bounds.Height / 2);
    }
}
