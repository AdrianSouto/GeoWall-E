using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using GeoWalle.Backend.Model.Expressions;
using GeoWalle.Backend.Model.Expressions.PredFunctions;

namespace GeoWalle.Backend.Model.GraphicObjects;

public class GLine : PredFunction, IGraphicObject
{
    public GPoint p1;
    public GPoint p2;

    public GLine(List<MyExpression> args) : base("line",args )
    {
        p1 = (GPoint) args[0];
        p2 = (GPoint) args[1];
        PosX = (p2.PosX + p1.PosX)/2;
        PosY = (p2.PosY + p1.PosY)/2;
        double x1 = 10000;
        double m = (p2.PosY - p1.PosY) / (p2.PosX - p1.PosX);
        double n = p1.PosY - m * p1.PosX;
        double y1 = m * x1 + n;
        double x2 = -10000;
        double y2 = m * x2 + n;
        p1 = new GPoint(new Point(x1, y1));
        p2 = new GPoint(new Point(x2, y2));
        MyGeometry = new LineGeometry(new Point(x1, y1), new Point(x2, y2));

    }

    public double PosX { get; }
    public double PosY { get; }
    public Geometry MyGeometry { get; }

    public void Draw(Canvas canvas, SolidColorBrush color)
    {
        
        Path line = new Path
        {
            Data = MyGeometry,
            Stroke = color,
            StrokeThickness = 2,
            
        };
        canvas.Children.Add(line);
    }

    protected override int CantArgs => 2;
    public string type => value;
    public override string Evaluate()
    {
        throw new System.NotImplementedException();
    }
}