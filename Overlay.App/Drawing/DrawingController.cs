using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Shapes;
using Avalonia.Input;
using Avalonia.Media;
using System.Collections.Generic;

namespace Overlay.App.Drawing;

public sealed class DrawingController
{
    private readonly Canvas canvas;
    private readonly BrushState brush;

    private Point? lastPoint;
    private List<Line>? currentStroke;

    private readonly Stack<List<Line>> undoStack = new();
    private readonly Stack<List<Line>> redoStack = new();

    public DrawingController(Canvas canvas, BrushState brush)
    {
        this.canvas = canvas;
        this.brush = brush;
    }

    public void OnPressed(PointerPressedEventArgs e)
    {
        lastPoint = e.GetPosition(canvas);
        currentStroke = new List<Line>();
        redoStack.Clear();
    }

    public void OnMoved(PointerEventArgs e)
    {
        if (lastPoint is null || currentStroke is null)
            return;

        var current = e.GetPosition(canvas);

        var line = new Line
        {
            StartPoint = lastPoint.Value,
            EndPoint = current,
            Stroke = brush.Brush,
            StrokeThickness = brush.Size,
            StrokeLineCap = PenLineCap.Round
        };

        canvas.Children.Add(line);
        currentStroke.Add(line);

        lastPoint = current;
    }

    public void OnReleased()
    {
        if (currentStroke is { Count: > 0 })
            undoStack.Push(currentStroke);

        currentStroke = null;
        lastPoint = null;
    }

    public void Undo()
    {
        if (undoStack.Count == 0)
            return;

        var stroke = undoStack.Pop();

        foreach (var line in stroke)
            canvas.Children.Remove(line);

        redoStack.Push(stroke);
    }

    public void Redo()
    {
        if (redoStack.Count == 0)
            return;

        var stroke = redoStack.Pop();

        foreach (var line in stroke)
            canvas.Children.Add(line);

        undoStack.Push(stroke);
    }

    public void Clear()
    {
        canvas.Children.Clear();
        undoStack.Clear();
        redoStack.Clear();
        currentStroke = null;
        lastPoint = null;
    }
}
