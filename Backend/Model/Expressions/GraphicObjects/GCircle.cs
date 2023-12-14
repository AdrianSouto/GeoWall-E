using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using GeoWalle.Backend.Model.Expressions;
using GeoWalle.Backend.Model.Expressions.PredFunctions;

namespace GeoWalle.Backend.Model.GraphicObjects;

public class GCircle : PredFunction, IGraphicObject
{
    public readonly double radio;

    public GCircle(List<MyExpression> args) : base("circle",args )
    {
        GPoint center = (GPoint) args[0];
        radio = double.Parse(args[1].Evaluate());
        PosX = center.PosX;
        PosY = center.PosY;
        MyGeometry = new EllipseGeometry(new Point(PosX,PosY), radio, radio);
    }

    public double PosX { get; }
    public double PosY { get; }
    public Geometry MyGeometry { get; }
    
    public void Draw(Canvas canvas, SolidColorBrush color)
    {
        Path circunferencia = new Path
        {
            Data = MyGeometry,
            Stroke = color,
            StrokeThickness = 2
        };
        canvas.Children.Add(circunferencia);
    }

    protected override int CantArgs => 2;
    public string type => value;
    public override string Evaluate()
    {
        throw new System.NotImplementedException();
    }
}