using System;
using System.Collections.Generic;
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
    public void AddVar(Variable v)
    {
        if (FindVar(v.Name) == null)
            myVars.Add(v);
        else
            throw new SyntaxException("La variable "+v.Name+" ya existe en el contexto actual.");
    }

    public Variable? FindVar(string varName)
    {
        return myVars.Find(x => x.Name == varName);

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