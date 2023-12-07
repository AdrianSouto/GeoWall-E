using System;
using GeoWalle.Backend.Model.MyExceptions;
using Walle.Model.Expressions;

namespace GeoWalle.Backend.Model.Expressions.Binary;
class MayorQ : BinaryMyExpression
{
    
    public MayorQ(MyExpression left, MyExpression right)
        : base(left, right)
    {
    }

    public override string value => ">";

    public override string Evaluate(){
        try
        {
            return (double.Parse(LeftMyExpression.Evaluate()) > double.Parse(RightMyExpression.Evaluate())).ToString();
        }
        catch (FormatException)
        {
            throw new SemanticException("La operacion " + value + " solo esta concebida para números");
        }
    }
}