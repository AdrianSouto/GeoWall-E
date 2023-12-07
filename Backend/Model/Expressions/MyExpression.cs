namespace GeoWalle.Backend.Model.Expressions;

public abstract class MyExpression
{
    public abstract string Evaluate();

    public abstract string value { get; }
}