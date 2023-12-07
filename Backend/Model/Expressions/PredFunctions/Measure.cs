using System;
using GeoWalle.Backend.Model.GraphicObjects;

namespace GeoWalle.Backend.Model.Expressions.PredFunctions;

public class Measure : MyExpression
{
    private readonly GPoint p1;
    private readonly GPoint p2;
    public Measure(GPoint p1, GPoint p2)
    {
        this.p1 = p1;
        this.p2 = p2;
    }

    public override string Evaluate()
    {
        return Math.Sqrt(Math.Pow(p2.x - p1.x, 2) + Math.Pow(p2.y - p1.y, 2)).ToString();
    }

    public override string value { get; }
}