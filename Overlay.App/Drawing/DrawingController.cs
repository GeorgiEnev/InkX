using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Shapes;
using Avalonia.Input;
using Avalonia.Media;

namespace Overlay.App.Drawing;

public sealed class DrawingController
{
    private readonly Canvas canvas;
    private readonly BrushState brush;
    private Point? last;

    public DrawingController(Canvas canvas, BrushState brush)
    {
        this.canvas = canvas;
        this.brush = brush;
    }

    public void OnPressed(PointerPressedEventArgs e)
    {
        last = e.GetPosition(canvas);
    }

    public void OnMoved(PointerEventArgs e)
    {
        if (last is null)
            return;

        var current = e.GetPosition(canvas);

        canvas.Children.Add(new Line
        {
            StartPoint = last.Value,
            EndPoint = current,
            Stroke = brush.Brush,
            StrokeThickness = brush.Size,
            StrokeLineCap = PenLineCap.Round
        });

        last = current;
    }

    public void OnReleased()
    {
        last = null;
    }

    public void Clear()
    {
        canvas.Children.Clear();
        last = null;
    }
}
