using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using GeoWalle.Backend.Model.Expressions;
using GeoWalle.Backend.Model.Expressions.PredFunctions;

namespace GeoWalle.Backend.Model.GraphicObjects;

public class GSegment : PredFunction, IGraphicObject
{
    public readonly GPoint p1;
    public readonly GPoint p2;

    public GSegment(List<MyExpression> args) : base("segment",args )
    {
        p1 = (GPoint) args[0];
        p2 = (GPoint) args[1];
        PosX = (p2.PosX + p1.PosX)/2;
        PosY = (p2.PosY + p1.PosY)/2;
        MyGeometry = new LineGeometry(new Point(p1.PosX, p1.PosY), new Point(p2.PosX, p2.PosY));
    }

    public double PosX { get; }
    public double PosY { get; }
    public Geometry MyGeometry { get; }
    public string type => value;

    public void Draw(Canvas canvas, SolidColorBrush color)
    {
        Path segment = new Path
        {
            Data = MyGeometry,
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