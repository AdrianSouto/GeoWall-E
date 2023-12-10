using System;
using GeoWalle.Backend.Model.Expressions;
using Hulk.Model;

namespace GeoWalle.Backend.Model.Objects;

public class Randoms : Sequence
{
    public Randoms() : base(0)
    {
        MoveNext();
    }
        
    public override bool MoveNext()
    {
        Random random = new Random();
        current = new Number(Math.Abs(random.Next()).ToString());
        return true;
    }
    private MyExpression current { get; set; }
    public override MyExpression Current => current;
}