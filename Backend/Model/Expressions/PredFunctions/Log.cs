using System;
using System.Collections.Generic;

namespace GeoWalle.Backend.Model.Expressions.PredFunctions;

class Log : PredFunction
{ 
    public Log(List<MyExpression> args) : base("log", args)
    {
    }

    protected override int CantArgs => 2;

    public override string Evaluate()
    {
            return Math.Log(double.Parse(Args[1].Evaluate()), double.Parse(Args[0].Evaluate())).ToString();
    }
}