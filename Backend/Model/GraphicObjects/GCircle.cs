using System.Collections.Generic;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using GeoWalle.Backend.Model.Expressions;
using GeoWalle.Backend.Model.Expressions.PredFunctions;

namespace GeoWalle.Backend.Model.GraphicObjects;

public class GCircle : PredFunction, IGraphicObject
{
    private readonly double radio;

    public GCircle(List<MyExpression> args) : base("circle",args )
    {
        GPoint center = (GPoint) args[0];
        radio = double.Parse(args[1].Evaluate());
        PosX = center.PosX - radio;
        PosY = center.PosY - radio;

    }

    public double PosX { get; }
    public double PosY { get; }

    public void Draw(Canvas canvas, SolidColorBrush color)
    {
        Ellipse circunferencia = new Ellipse
        {
            Width = radio*2,
            Height = radio*2,
            Stroke = color,
            StrokeThickness = 2
        };
        Canvas.SetLeft(circunferencia, PosX);
        Canvas.SetTop(circunferencia, PosY);
        canvas.Children.Add(circunferencia);
    }

    protected override int CantArgs => 2;
    public override string Evaluate()
    {
        throw new System.NotImplementedException();
    }
}