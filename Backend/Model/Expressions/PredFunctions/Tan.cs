using System;
using System.Collections.Generic;

namespace GeoWalle.Backend.Model.Expressions.PredFunctions;

class Tan : PredFunction
{
    public Tan(List<MyExpression> args) : base("Tan", args)
    {
    }

    protected override int CantArgs => 1;

    public override string Evaluate()
    {
        return Math.Tan(double.Parse(Args[0].Evaluate())).ToString();
    }
}