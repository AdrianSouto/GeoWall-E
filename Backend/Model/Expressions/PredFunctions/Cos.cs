using System;
using System.Collections.Generic;
using Walle.Model.Expressions;

namespace GeoWalle.Backend.Model.Expressions.PredFunctions;

class Cos : PredFunction
{
    public Cos(List<MyExpression> args) : base("Cos", args)
    {
    }

    protected override int CantArgs => 1;

    public override string Evaluate()
    {
        return Math.Cos(double.Parse(Args[0].Evaluate())).ToString();
    }
}