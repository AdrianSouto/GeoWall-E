using System;
using System.Collections.Generic;

namespace GeoWalle.Backend.Model.Expressions.PredFunctions;

class Sin : PredFunction
{
    public Sin(List<MyExpression> args) : base("Sin", args)
    {
    }

    protected override int CantArgs => 1;

    public override string Evaluate()
    {
        return Math.Sin(double.Parse(Args[0].Evaluate())).ToString();
    }
}