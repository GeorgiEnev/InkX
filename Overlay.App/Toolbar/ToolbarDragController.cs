using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Media;

namespace Overlay.App.Toolbar;

public sealed class ToolbarDragController
{
    private readonly Control toolbar;
    private readonly TranslateTransform transform = new();
    private bool dragging;
    private Point start;
    private Vector offset;

    public ToolbarDragController(Control toolbar)
    {
        this.toolbar = toolbar;
        toolbar.RenderTransform = transform;
    }

    public void OnPressed(PointerPressedEventArgs e, Control root)
    {
        dragging = true;
        start = e.GetPosition(root);
        offset = new Vector(transform.X, transform.Y);
        e.Pointer.Capture(toolbar);
    }

    public void OnMoved(PointerEventArgs e, Control root)
    {
        if (!dragging)
            return;

        var delta = e.GetPosition(root) - start;
        transform.X = offset.X + delta.X;
        transform.Y = offset.Y + delta.Y;
    }

    public void OnReleased(PointerReleasedEventArgs e)
    {
        dragging = false;
        e.Pointer.Capture(null);
    }
}
