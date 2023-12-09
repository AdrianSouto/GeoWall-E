using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using GeoWalle.Backend.Model.Expressions;

namespace GeoWalle.Backend.Model.GraphicObjects;

public class GPoint : MyExpression, IGraphicObject
{
    public GPoint()
    {
        Random random = new Random();
        PosX = random.NextDouble() * GraphicObjects.Draw.canvas.ActualWidth;
        PosY = random.NextDouble() * GraphicObjects.Draw.canvas.ActualHeight;
        MyGeometry = new EllipseGeometry(new Point(PosX, PosY), 5, 5);
    }


    public double PosX { get; }
    public double PosY { get; }
    public Geometry MyGeometry { get; }

    public void Draw(Canvas canvas, SolidColorBrush color)
    {
        Path point = new Path
        {
            Data = MyGeometry,
            Fill = color,
        };
        canvas.Children.Add(point);
    }

    public override string Evaluate()
    {
        throw new NotImplementedException();
    }

    public override string value => "point";
}