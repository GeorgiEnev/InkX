using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using Overlay.App.ColorPicker;
using Overlay.App.Drawing;
using Overlay.App.Toolbar;

namespace Overlay.App;

public partial class MainWindow : Window
{
    private readonly BrushState brush;
    private readonly DrawingController drawing;
    private readonly ToolbarDragController toolbar;
    private readonly ColorPickerController colorPicker;

    private bool isToolBarVisible = true;

    public MainWindow()
    {
        InitializeComponent();
        Focus();

        brush = new BrushState();
        drawing = new DrawingController(DrawCanvas, brush);
        toolbar = new ToolbarDragController(ToolBar);
        colorPicker = new ColorPickerController(
            ColorPickerImage,
            ColorPickerDot,
            brush,
            180
        );

        BrushSizeInput.Text = brush.Size.ToString("0");

        DrawCanvas.PointerPressed += (_, e) =>
        {
            CloseColorPopup();
            drawing.OnPressed(e);
        };

        DrawCanvas.PointerMoved += (_, e) =>
        {
            drawing.OnMoved(e);
        };

        DrawCanvas.PointerReleased += (_, _) =>
        {
            drawing.OnReleased();
        };
    }

    private void OnKeyDown(object? sender, KeyEventArgs e)
    {
        if (e.Key == Key.C)
        {
            drawing.Clear();
        }
        else if (e.Key == Key.T)
        {
            isToolBarVisible = !isToolBarVisible;
            ToolBar.IsVisible = isToolBarVisible;
            CloseColorPopup();
        }
    }

    private void IncreaseBrushSize(object? sender, RoutedEventArgs e)
    {
        brush.SetSize(brush.Size + BrushState.Step);
        BrushSizeInput.Text = brush.Size.ToString("0");
    }

    private void DecreaseBrushSize(object? sender, RoutedEventArgs e)
    {
        brush.SetSize(brush.Size - BrushState.Step);
        BrushSizeInput.Text = brush.Size.ToString("0");
    }

    private void OnBrushSizeInputKeyDown(object? sender, KeyEventArgs e)
    {
        if (e.Key == Key.Enter)
            ApplyBrushSizeFromInput();
    }

    private void OnBrushSizeInputChanged(object? sender, RoutedEventArgs e)
    {
        ApplyBrushSizeFromInput();
    }

    private void ApplyBrushSizeFromInput()
    {
        if (!double.TryParse(BrushSizeInput.Text, out var value))
        {
            BrushSizeInput.Text = brush.Size.ToString("0");
            return;
        }

        brush.SetSize(value);
        BrushSizeInput.Text = brush.Size.ToString("0");
    }

    private void OnToolBarPointerPressed(object? sender, PointerPressedEventArgs e)
    {
        toolbar.OnPressed(e, this);
    }

    private void OnToolBarPointerMoved(object? sender, PointerEventArgs e)
    {
        toolbar.OnMoved(e, this);
    }

    private void OnToolBarPointerReleased(object? sender, PointerReleasedEventArgs e)
    {
        toolbar.OnReleased(e);
    }

    private void ToggleColorPopup(object? sender, RoutedEventArgs e)
    {
        ColorPopup.IsOpen = !ColorPopup.IsOpen;
    }

    private void OnColorPickerPressed(object? sender, PointerPressedEventArgs e)
    {
        colorPicker.OnPressed(e);
    }

    private void OnColorPickerMoved(object? sender, PointerEventArgs e)
    {
        colorPicker.OnMoved(e);
    }

    private void OnColorPickerReleased(object? sender, PointerReleasedEventArgs e)
    {
        colorPicker.OnReleased();
    }

    private void OnHueChanged(object? sender, Avalonia.Controls.Primitives.RangeBaseValueChangedEventArgs e)
    {
        colorPicker.SetHue(e.NewValue);
    }

    private void CloseColorPopup()
    {
        ColorPopup.IsOpen = false;
    }
}
