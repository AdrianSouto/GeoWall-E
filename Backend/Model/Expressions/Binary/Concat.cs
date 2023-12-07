using Walle.Model.Expressions;

namespace GeoWalle.Backend.Model.Expressions.Binary;

public class Concat : BinaryMyExpression
{
    
    public Concat(MyExpression left, MyExpression right)
        : base(left, right)
    {
    }

    public override string value => "@";

    public override string Evaluate()
    {
        return LeftMyExpression.Evaluate() + RightMyExpression.Evaluate();
    }
}