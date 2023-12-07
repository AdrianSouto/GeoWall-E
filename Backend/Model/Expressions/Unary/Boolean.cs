namespace Walle.Model.Expressions.Unary;

public class Bool : UnaryExpression
{
    public Bool(string value) : base(value){}
    public override string Evaluate()
    {
        return value;
    }
    
}