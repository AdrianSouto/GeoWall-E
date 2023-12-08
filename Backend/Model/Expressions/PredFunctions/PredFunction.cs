using System.Collections.Generic;
using GeoWalle.Backend.Model.MyExceptions;

namespace GeoWalle.Backend.Model.Expressions.PredFunctions;

public abstract class PredFunction : MyExpression
{
    protected List<MyExpression> Args;
    protected abstract int CantArgs { get; }
    protected PredFunction(string value, List<MyExpression> args)
    {
        if(args.Count != CantArgs)
            throw new SyntaxException("Invalid Arguments at "+value+" function");
        
        this.value = value;
        Args = args;
    }
    public override string value { get; }
}