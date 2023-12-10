using System;
using System.Collections.Generic;
using GeoWalle.Backend.Model.GraphicObjects;

namespace GeoWalle.Backend.Model.Expressions.PredFunctions;

public class Measure : PredFunction
{
    private readonly GPoint p1;
    private readonly GPoint p2;
    public Measure(List<MyExpression> args) : base("measure",args)
    {
        p1 = (GPoint)args[0];
        p2 = (GPoint)args[1];
    }

    protected override int CantArgs => 2;

    public override string Evaluate()
    {
        return Math.Sqrt(Math.Pow(p2.PosX - p1.PosX, 2) + Math.Pow(p2.PosY - p1.PosY, 2)).ToString();
    }

    public static double operator +(Measure x1, double x2)
    {
        return double.Parse(x1.Evaluate()) + x2;
    }
    public static double operator -(Measure x1, double x2)
    {
        return double.Parse(x1.Evaluate()) - x2;
    }
    public static double operator *(Measure x1, double x2)
    {
        return double.Parse(x1.Evaluate()) * x2;
    }
    public static double operator /(Measure x1, double x2)
    {
        return double.Parse(x1.Evaluate()) / x2;
    }
    
}