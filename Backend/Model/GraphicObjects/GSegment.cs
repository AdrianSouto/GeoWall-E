using System.Collections.Generic;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using GeoWalle.Backend.Model.Expressions;
using GeoWalle.Backend.Model.Expressions.PredFunctions;

namespace GeoWalle.Backend.Model.GraphicObjects;

public class GSegment : PredFunction, IGraphicObject
{
    private GPoint p1;
    private GPoint p2;

    public GSegment(List<MyExpression> args) : base("segment",args )
    {
        p1 = (GPoint) args[0];
        p2 = (GPoint) args[1];
        PosX = (p2.PosX + p1.PosX)/2;
        PosY = (p2.PosY + p1.PosY)/2;

    }

    public double PosX { get; }
    public double PosY { get; }

    public void Draw(Canvas canvas, SolidColorBrush color)
    {
        Line segment = new Line
        {
            X1 = p1.PosX,
            Y1 = p1.PosY,
            X2 = p2.PosX,
            Y2 = p2.PosY,
            Stroke = color,
            StrokeThickness = 2
        };
        canvas.Children.Add(segment);
    }

    protected override int CantArgs => 2;
    public override string Evaluate()
    {
        throw new System.NotImplementedException();
    }
}