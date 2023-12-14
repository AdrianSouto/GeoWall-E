using System;
using System.Collections.Generic;
using System.Windows;
using GeoWalle.Backend.Model.GraphicObjects;
using GeoWalle.Backend.Model.MyExceptions;
using GeoWalle.Backend.Model.Objects;

namespace GeoWalle.Backend.Model.Expressions.GraphicObjects;

public class Intersect : Sequence
{
    private IGraphicObject o1;
    private IGraphicObject o2;
    private IEnumerator<MyExpression> interseccion;

    public Intersect(List<MyExpression> param) : base(0)
    {
        o1 = (IGraphicObject)param[0];
        o2 = (IGraphicObject)param[1];
        if (o1 is GCircle c1)
        {
            if(o2 is GCircle c2)
            {
                interseccion = Utils.CircleCircleIntersection(
                    c1.PosX,
                    c1.PosY,
                    c1.radio,
                    c2.PosX,
                    c2.PosY,
                    c2.radio);
            }else if(o2 is GLine l2)
            {
                interseccion = Utils.CircleLineIntersection(
                    c1.PosX,
                    c1.PosY,
                    c1.radio,
                    l2.p1.PosX,
                    l2.p1.PosY,
                    l2.p2.PosX,
                    l2.p2.PosY
                    );
            }
        }else if (o1 is GLine l1)
        {
            if(o2 is GCircle c2)
            {
                interseccion = Utils.CircleLineIntersection(
                    c2.PosX,
                    c2.PosY,
                    c2.radio,
                    l1.p1.PosX,
                    l1.p1.PosY,
                    l1.p2.PosX,
                    l1.p2.PosY
                    );
            }else if(o2 is GLine l2)
            {
                interseccion = Utils.LineLineIntersection(
                    l1.p1.PosX,
                    l1.p1.PosY,
                    l1.p2.PosX,
                    l1.p2.PosY,
                    l2.p1.PosX,
                    l2.p1.PosY,
                    l2.p2.PosX,
                    l2.p2.PosY
                    );
            }else if(o2 is GSegment s2)
            {
                interseccion = Utils.LineLineIntersection(
                    l1.p1.PosX,
                    l1.p1.PosY,
                    l1.p2.PosX,
                    l1.p2.PosY,
                    s2.p1.PosX,
                    s2.p1.PosY,
                    s2.p2.PosX,
                    s2.p2.PosY
                    );
            }
        }else if (o1 is GSegment s1)
        {
            if (o2 is GSegment s2)
            {
                interseccion = Utils.LineLineIntersection(
                    s1.p1.PosX,
                    s1.p1.PosY,
                    s1.p2.PosX,
                    s1.p2.PosY, 
                    s2.p1.PosX,
                    s2.p1.PosY,
                    s2.p2.PosX,
                    s2.p2.PosY
                    );
            }else if (o2 is GCircle c2)
            {
                interseccion = Utils.CircleLineIntersection(
                    c2.PosX,
                    c2.PosY,
                    c2.radio,
                    s1.p1.PosX,
                    s1.p1.PosY,
                    s1.p2.PosX,
                    s1.p2.PosY
                    );
            }else if(o2 is GLine l2)
            {
                interseccion = Utils.LineLineIntersection(
                    s1.p1.PosX,
                    s1.p1.PosY,
                    s1.p2.PosX,
                    s1.p2.PosY,
                    l2.p1.PosX,
                    l2.p1.PosY,
                    l2.p2.PosX,
                    l2.p2.PosY
                    );
            }
        }

        if (interseccion == null)
            throw new SemanticException("No se puede calcular la interseccion de " + o1.type + " y " + o2.type);
    }
    
    public override bool MoveNext()
    {
        if(interseccion.MoveNext())
        {
            current = interseccion.Current;
            return true;
        }

        return false;
    }

    private MyExpression current { get; set; }
    public override MyExpression Current => current;

    

    public override string Evaluate()
    {
        throw new System.NotImplementedException();
    }

    public override string value => "intersect";
}