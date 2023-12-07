using GeoWalle.Backend.Model.Expressions;

namespace Walle.Model.Expressions.Unary;

public class Negation : UnaryExpression
{
    public Negation(MyExpression rightMyExpression) : base(rightMyExpression.value)
    {
        RightMyExpression = rightMyExpression;
    }
    private MyExpression RightMyExpression { get;}

    public override string Evaluate()
    {
        return (!bool.Parse(RightMyExpression.Evaluate())).ToString();
    }

}