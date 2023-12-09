using System;
using System.Collections;
using System.Collections.Generic;
using GeoWalle.Backend.Model.Expressions;
using Hulk.Model;

namespace GeoWalle.Backend.Model.Objects;

public class Sequence : MyExpression, IEnumerator<MyExpression>
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

    public bool MoveNext()
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
    

    public MyExpression Current { get; private set; }


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
       /* string s = "";
        while (MoveNext())
        {
            s += Current.Evaluate();
        }

        return s;*/
       return "";
    }

    public override string value => "sequence";
}