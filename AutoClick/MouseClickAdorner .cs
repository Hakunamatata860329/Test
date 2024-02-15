using System.Windows;
using System.Windows.Documents;
using System.Windows.Media;

public class MouseClickAdorner : Adorner
{
    private readonly Point _clickPoint;

    public MouseClickAdorner(UIElement adornedElement, Point clickPoint)
        : base(adornedElement)
    {
        _clickPoint = clickPoint;
    }

    protected override void OnRender(DrawingContext drawingContext)
    {
        // 使用紅色筆劃繪紅色框
        Pen pen = new Pen(Brushes.Red, 2);
        drawingContext.DrawRectangle(null, pen, new Rect(_clickPoint, new Size(2, 2)));
    }
}