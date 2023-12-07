using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using GeoWalle.Backend.Model.Expressions.PredFunctions;

namespace GeoWalle.Backend.Model.GraphicObjects;

public class GCircle : IGraphicObject
{
    private readonly GPoint center;
    private readonly double radio;

    public GCircle(GPoint center, Measure measure)
    {
        this.center = center;
        radio = double.Parse(measure.Evaluate());
    }
    public void Draw(Canvas canvas, SolidColorBrush color)
    {
        Ellipse circunferencia = new Ellipse
        {
            Width = radio,
            Height = radio,
            Stroke = color,
            StrokeThickness = 2
        };
        Canvas.SetLeft(circunferencia, center.x * canvas.Width);
        Canvas.SetTop(circunferencia, center.y * canvas.Height);
    }
}