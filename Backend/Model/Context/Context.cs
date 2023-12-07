using System;
using System.Collections.Generic;
using GeoWalle.Backend.Model.GraphicObjects;
using GeoWalle.Backend.Model.MyExceptions;
using GeoWalle.Backend.Model.Objects;
using Hulk.Model;

namespace GeoWalle.Backend.Model.Context;

public class Context
{
    private readonly List<Variable> myVars = new()
    {
        new Variable("PI", new Number(double.Pi.ToString())),
        new Variable("E", new Number(Math.E.ToString()))
    };
    private readonly List<GObjVar> myGObjects = new();

    public void AddVar(Variable v)
    {
        if (FindVar(v.Name) == null && FindGObject(v.Name) == null)
            myVars.Add(v);
        else
            throw new SyntaxException("La variable "+v.Name+" ya existe en el contexto actual.");
    }
    public void AddGObject(GObjVar o)
    {
        if (FindVar(o.Name) == null && FindGObject(o.Name) == null)
            myGObjects.Add(o);
        else
            throw new SyntaxException("La variable "+o.Name+" ya existe en el contexto actual.");
    }

    public Variable? FindVar(string varName)
    {
        return myVars.Find(x => x.Name == varName);

    }
    public GObjVar? FindGObject(string objName)
    {
        return myGObjects.Find(x => x.Name == objName);

    }

    public Context Clone()
    {
        Context c = new Context();
        foreach (Variable v in myVars)
        {
            if (FindVar(v.Name) == null)
            {
                c.AddVar(v);
            }
        }

        return c;
    }
    
}