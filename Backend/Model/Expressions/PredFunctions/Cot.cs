using System;
using System.Collections.Generic;
using Walle.Model.Expressions;

namespace GeoWalle.Backend.Model.Expressions.PredFunctions;

internal class Cot : PredFunction
{
    public Cot(List<MyExpression> args) : base("Cot", args)
    {
    }

    protected override int CantArgs => 1;

    public override string Evaluate()
    {
        return (1/Math.Tan(double.Parse(Args[0].Evaluate()))).ToString();
    }
}