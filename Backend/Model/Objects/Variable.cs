using GeoWalle.Backend.Model.Expressions;

namespace GeoWalle.Backend.Model.Objects;

public class Variable
{
    public string Name;
    private MyExpression VarTree { get; }

    private string _value = "";

    public string Value
    {
        get
        {
            if (_valueRequested) return _value;
            _valueRequested = true;
            _value = VarTree.Evaluate();
            return _value;
        }
    }
    private bool _valueRequested;

    public Variable(string name, MyExpression varTree)
    {
        Name = name;
        VarTree = varTree;
    }
    
    

}