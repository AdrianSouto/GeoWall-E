using System;
using System.Windows.Media;
using GeoWalle.Backend.Model.Expressions;
using GeoWalle.Backend.Model.GraphicObjects;

namespace GeoWalle.Backend.Model.Objects;

public class Samples : Sequence
{

    public Samples() : base(0)
    {
        MoveNext();
    }
        
    public override bool MoveNext()
    {
        current = new GPoint();
        return true;
    }
    private MyExpression current { get; set; }
    public override MyExpression Current => current;
}