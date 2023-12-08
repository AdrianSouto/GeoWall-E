using System;
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
    }


    public double PosX { get; }
    public double PosY { get; }

    public void Draw(Canvas canvas, SolidColorBrush color)
    {
        Ellipse punto = new Ellipse
        {
            Width = 5,
            Height = 5,
            Fill = color
        };
        Canvas.SetLeft(punto, PosX);
        Canvas.SetTop(punto, PosY);
        canvas.Children.Add(punto);
    }

    public override string Evaluate()
    {
        throw new NotImplementedException();
    }

    public override string value => "point";
}