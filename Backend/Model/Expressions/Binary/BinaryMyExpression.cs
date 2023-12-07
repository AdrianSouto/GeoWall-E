using Walle.Model.Expressions;

namespace GeoWalle.Backend.Model.Expressions.Binary;

public abstract class BinaryMyExpression : MyExpression
{
    // Make the properties public
    protected MyExpression LeftMyExpression { get;}
    protected MyExpression RightMyExpression { get;}
    
    // Make the constructor public
    protected BinaryMyExpression(MyExpression left, MyExpression right)
    {
        LeftMyExpression = left;
        RightMyExpression = right;
    }
}