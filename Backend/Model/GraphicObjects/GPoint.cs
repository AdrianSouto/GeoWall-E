using System;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace GeoWalle.Backend.Model.GraphicObjects;

public class GPoint : IGraphicObject
{
    public readonly double x;
    public readonly double y;
    public GPoint()
    {
        Random random = new Random();
        x = random.NextDouble();
        y = random.NextDouble();
    }
    public void Draw(Canvas canvas, SolidColorBrush color)
    {
        Ellipse punto = new Ellipse
        {
            Width = 5,
            Height = 5,
            Fill = color
        };
        Canvas.SetLeft(punto, x * canvas.Width);
        Canvas.SetTop(punto, y * canvas.Height);
        canvas.Children.Add(punto);
    }
}