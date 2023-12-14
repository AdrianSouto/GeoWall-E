namespace GeoWalle.Backend.Model.GraphicObjects;

public class GObjVar
{
    public readonly string Name;
    public readonly IGraphicObject Value;

    public GObjVar(string name, IGraphicObject value)
    {
        Name = name;
        Value = value;
    }
}