using System;
using System.Collections;
using System.Collections.Generic;
using System.Windows.Controls;
using System.Windows.Media;
using GeoWalle.Backend.Model.Expressions;
using GeoWalle.Backend.Model.GraphicObjects;
using GeoWalle.Backend.Model.MyExceptions;
using Hulk.Model;

namespace GeoWalle.Backend.Model.Objects;

public class Sequence : MyExpression, IEnumerator<MyExpression>, IGraphicObject
{
    private IEnumerator<MyExpression> list;
    private int start;
    private int end;
    private bool lastMoveNextResult = true;
    public Sequence(List<MyExpression> list)
    {
        this.list = list.GetEnumerator();
    }

    public Sequence(int start, int end = int.MaxValue)
    {
        this.start = start;
        this.end = end;
    }

    public virtual bool MoveNext()
    {
        if (!lastMoveNextResult)
            throw new InvalidOperationException("Intentó hacer MoveNext después de haber retornado false");
        if (list != null)
        {
            lastMoveNextResult = list.MoveNext();
            Current = list.Current;
        }
        else if (start > end)
            lastMoveNextResult = false;
        else
        {
            Current = new Number((start++).ToString());
            lastMoveNextResult = true;
        }
        return lastMoveNextResult;
    }
    

    public virtual MyExpression Current { get; private set; }


    public void Reset()
    {
        throw new System.NotImplementedException();
    }


    object IEnumerator.Current => Current;

    public void Dispose()
    {
        throw new System.NotImplementedException();
    }

    public override string Evaluate()
    {
        string s = "";
        while (MoveNext())
        {
            s += Current.Evaluate();
        }

        return s;
    }

    public override string value => "sequence";
    public double PosX => 0.0;
    public double PosY => 0.0;
    public Geometry MyGeometry { get; }
    public void Draw(Canvas canvas, SolidColorBrush color)
    {
        while (MoveNext())
        {
            try
            {
                ((IGraphicObject)Current).Draw(canvas, Color.GetColor());
            }
            catch (InvalidCastException e)
            {
                throw new SemanticException(Current.value + " no se puede dibujar");
            }
        }
    }
    public static Sequence operator +(Sequence s1, Sequence s2)
    {
        List<MyExpression> union = new();
        while (s1.MoveNext())
        {
            union.Add(s1.Current);
        }
        while (s2.MoveNext())
        {
            union.Add(s2.Current);
        }

        return new Sequence(union);
    }
    public string type => value;
}