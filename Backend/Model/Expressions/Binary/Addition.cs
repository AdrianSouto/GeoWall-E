using System;
using GeoWalle.Backend.Model.MyExceptions;
using Walle.Model.Expressions;

namespace GeoWalle.Backend.Model.Expressions.Binary;

class Addition : BinaryMyExpression
{
    
    public Addition(MyExpression left, MyExpression right)
        : base(left, right)
    {
    }

    public override string value => "+";

    public override string Evaluate()
    {
        try
        {
            return (double.Parse(LeftMyExpression.Evaluate()) + double.Parse(RightMyExpression.Evaluate())).ToString();
        }
        catch (FormatException)
        {
            throw new  SemanticException("No se puede sumar 2 Strings, el operador de concatenacion es '@'.");
        }
    }
}