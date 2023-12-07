using Walle.Model.Expressions.Unary;

namespace Hulk.Model;

public class Text : UnaryExpression
{
    public Text(string value) : base(value){}
    public override string Evaluate()
    {
        return value;
    }
}