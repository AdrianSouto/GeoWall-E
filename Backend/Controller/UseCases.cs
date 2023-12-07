using System.Collections.Generic;
using GeoWalle.Backend.Model.MyExceptions;
using GeoWalle.Backend.Model.Objects;

namespace GeoWalle.Backend.Controller;

public class UseCases
{
    public void Compile(string input)
    {
        if (!Utils.IsBalanced(input)) throw new SyntaxException("Input no balanceado");
        IEnumerable<Token> tokens = Lexer.GetTokens(input);
    }
}