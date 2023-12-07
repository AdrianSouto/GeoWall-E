using System;
using System.Collections.Generic;
using GeoWalle.Backend.Model.MyExceptions;

namespace GeoWalle.Backend.Model.Objects;

public class UserFunction
{
    public string Name;
    public List<Token> FunBody;
    public List<string> ParamNames;
    private static List<UserFunction> userFunctions = new();

    

    public UserFunction(string name, List<string> paramNames, List<Token> funBody)
    {
        Name = name;
        FunBody = funBody;
        ParamNames = paramNames;
    }
    public static void AddFunction(UserFunction f)
    {
        UserFunction? uf = FindFunction(f.Name);
        if(uf == null)
            userFunctions.Add(f);
        else
        {
            throw new SyntaxException("Ha intentado declarar más de una función con el nombre: " + f.Name);
            /*Console.WriteLine("Ya existe una funcion con ese nombre, si desea sobreescribirla pulse 's' de lo contrario pulse cualquier otra tecla");
            if (Console.ReadKey().KeyChar == 's')
            {
                UserFunctions.Remove(uf);
                UserFunctions.Add(f);
            }

            Console.WriteLine();*/
        }
            
    }
    public static UserFunction? FindFunction(string funName)
    {
        return userFunctions.Find(x => x.Name == funName);
    }

    public static void CleanFunctions()
    {
        userFunctions.Clear();
    }
}