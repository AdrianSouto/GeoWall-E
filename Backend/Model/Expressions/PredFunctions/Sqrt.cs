using System;
using System.Collections.Generic;
using Walle.Model.Expressions;

namespace GeoWalle.Backend.Model.Expressions.PredFunctions;

class Sqrt : PredFunction
{
    public Sqrt(List<MyExpression> args) : base("Sqrt", args)
    {
    }

    protected override int CantArgs => 1;

    public override string Evaluate()
    {
        return Math.Sqrt(double.Parse(Args[0].Evaluate())).ToString();
    }
}