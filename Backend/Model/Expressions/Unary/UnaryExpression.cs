using GeoWalle.Backend.Model.Expressions;

namespace Walle.Model.Expressions.Unary;

public abstract class UnaryExpression : MyExpression
{
    protected UnaryExpression(string value) => this.value = value;
    public abstract override string Evaluate();

    public override string value { get; }
}