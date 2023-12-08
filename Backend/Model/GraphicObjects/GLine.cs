using System.Collections.Generic;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using GeoWalle.Backend.Model.Expressions;
using GeoWalle.Backend.Model.Expressions.PredFunctions;

namespace GeoWalle.Backend.Model.GraphicObjects;

public class GLine : PredFunction, IGraphicObject
{
    private GPoint p1;
    private GPoint p2;

    public GLine(List<MyExpression> args) : base("line",args )
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
        double x1 = 1000;
        double m = (p2.PosY - p1.PosY) / (p2.PosX - p1.PosX);
        double n = p1.PosY - m * p1.PosX;
        double y1 = m * x1 + n;
        double x2 = -1000;
        double y2 = m * x2 + n;
        Line line = new Line
        {
            X1 = x1,
            Y1 = y1,
            X2 = x2,
            Y2 = y2,
            Stroke = color,
            StrokeThickness = 2,
            
        };
        canvas.Children.Add(line);
    }

    protected override int CantArgs => 2;
    public override string Evaluate()
    {
        throw new System.NotImplementedException();
    }
}