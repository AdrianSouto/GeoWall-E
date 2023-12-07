using GeoWalle.Backend.Model.Expressions;

namespace Walle.Model.Expressions.Unary;

public class NegativeNumber : UnaryExpression
{
    public NegativeNumber(MyExpression rightMyExpression) : base(rightMyExpression.value)
    {
        RightMyExpression = rightMyExpression;
    }
    private MyExpression RightMyExpression { get;}

    public override string Evaluate()
    {
        return (-double.Parse(RightMyExpression.Evaluate())).ToString();
    }

}