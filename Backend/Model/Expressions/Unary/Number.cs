using Walle.Model.Expressions.Unary;

namespace Hulk.Model;

public class Number : UnaryExpression
{
    public Number(string value) : base(value){}
    public override string Evaluate()
    {
        return value;
    }
    
}