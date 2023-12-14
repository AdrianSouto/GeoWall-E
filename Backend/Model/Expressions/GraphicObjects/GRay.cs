using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using GeoWalle.Backend.Model.Expressions.PredFunctions;
using GeoWalle.Backend.Model.GraphicObjects;

namespace GeoWalle.Backend.Model.Expressions.GraphicObjects;

public class GRay : PredFunction, IGraphicObject
{
    public GPoint p1;
    public GPoint p2;

    public GRay(List<MyExpression> args) : base("ray",args )
    {
        p1 = (GPoint) args[0];
        p2 = (GPoint) args[1];
        PosX = (p2.PosX + p1.PosX)/2;
        PosY = (p2.PosY + p1.PosY)/2;
        double m = (p2.PosY - p1.PosY) / (p2.PosX - p1.PosX);
        double n = p1.PosY - m * p1.PosX;
        double x2 = p1.PosX < p2.PosX? 10000 : -10000;
        double y2 = m * x2 + n;
        MyGeometry = new LineGeometry(new Point(p1.PosX, p1.PosY), new Point(x2, y2));

    }

    public double PosX { get; }
    public double PosY { get; }
    public Geometry MyGeometry { get; }

    public void Draw(Canvas canvas, SolidColorBrush color)
    {
        
        Path ray = new Path
        {
            Data = MyGeometry,
            Stroke = color,
            StrokeThickness = 2,
            
        };
        canvas.Children.Add(ray);
    }

    protected override int CantArgs => 2;
    public string type => value;
    public override string Evaluate()
    {
        throw new System.NotImplementedException();
    }
}