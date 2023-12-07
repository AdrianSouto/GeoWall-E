using System.Collections.Generic;
using System.Linq;

namespace GeoWalle.Backend;

public static class Utils
{
    public static bool IsBalanced(string input)
    {
        Dictionary<char, char> specials = new();
        specials.Add('{','}');
        specials.Add('(',')');
        Stack<char> stack = new Stack<char>();
        foreach (char c in input)
        {
            if(specials.Keys.Contains(c)){
                stack.Push(c);
            }else if (specials.Values.Contains(c))
            {
                if (stack.Count == 0)
                    return false;
                char lastChar = stack.Pop();
                if (specials[lastChar] != c)
                    return false;
            }
        }
        return stack.Count == 0;
    }
}