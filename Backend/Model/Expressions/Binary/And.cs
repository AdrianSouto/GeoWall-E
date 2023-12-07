using System;
using GeoWalle.Backend.Model.MyExceptions;
using Walle.Model.Expressions;

namespace GeoWalle.Backend.Model.Expressions.Binary;

class And : BinaryMyExpression
{
    
    public And(MyExpression left, MyExpression right)
        : base(left, right)
    {
    }

    public override string value => "&";

    public override string Evaluate(){
        try
        {
            return (bool.Parse(LeftMyExpression.Evaluate()) && bool.Parse(RightMyExpression.Evaluate())).ToString();
        }
        catch (FormatException)
        {
            throw new  SemanticException("La operacion " + value+ " solo esta concebida para booleans");
        }
    }
}