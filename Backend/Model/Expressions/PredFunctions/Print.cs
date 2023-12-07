using System.Collections.Generic;

namespace GeoWalle.Backend.Model.Expressions.PredFunctions;

class Print : PredFunction
{
    public Print(List<MyExpression> args) : base("Print", args)
    {
    }

    protected override int CantArgs => 1;

    public override string Evaluate()
    {
        return Args[0].Evaluate();
    }
    
}