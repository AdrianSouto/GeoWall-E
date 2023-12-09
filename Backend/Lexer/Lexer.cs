using System.Collections.Generic;
using System.Linq;
using GeoWalle.Backend.Model.MyExceptions;
using GeoWalle.Backend.Model.Objects;

namespace GeoWalle.Backend;

internal static class Lexer{
    private static readonly Dictionary<string, Token.TokenType> Symbols = new()
    {
        ["+"] = Token.TokenType.Sum,
        ["-"] = Token.TokenType.Resta,
        ["/"] = Token.TokenType.Div,
        ["*"] = Token.TokenType.Product,
        ["("] = Token.TokenType.OpenParenthesis,
        [")"] = Token.TokenType.CloseParenthesis,
        ["="] = Token.TokenType.Igual,
        ["\""] = Token.TokenType.Comillas,
        [";"] = Token.TokenType.Semicolon,
        ["^"] = Token.TokenType.Pow,
        [","] = Token.TokenType.Comma,
        ["!"] = Token.TokenType.Negation,
        ["&"] = Token.TokenType.And,
        ["|"] = Token.TokenType.Or,
        ["%"] = Token.TokenType.Resto,
        [">"] = Token.TokenType.MayorQ,
        ["<"] = Token.TokenType.MenorQ,
        ["@"] = Token.TokenType.Concat,
        ["{"] = Token.TokenType.OpenLlaves,
        ["}"] = Token.TokenType.ClosedLLaves,
    };
    public static IEnumerable<Token> GetTokens(string input){
        string currentToken = "";
        bool isPlainText = false;
        using IEnumerator<char> pointer = input.GetEnumerator();
        while(pointer.MoveNext()){ 
            if(char.IsWhiteSpace(pointer.Current) && !isPlainText){
                if(currentToken != ""){
                    yield return new Token(currentToken, ClasificarToken(currentToken));
                }
                currentToken = "";
                continue;
            }

            if (pointer.Current == '"')
            {
                if (isPlainText)
                {
                    yield return(new Token(currentToken, Token.TokenType.Text));
                    currentToken = "";
                    isPlainText = false;
                }
                else
                {
                    if(currentToken != ""){
                        yield return(new Token(currentToken, ClasificarToken(currentToken)));
                        currentToken = "";
                    }
                    isPlainText = true;
                }
                continue;
            }

            if (isPlainText)
            {
                currentToken += pointer.Current;
                continue;
            }
            
            if (pointer.Current == '>')
            {
                if (currentToken != "")
                    yield return new Token(currentToken, ClasificarToken(currentToken));
                
                if(!pointer.MoveNext())
                    throw new SyntaxException("Missing Semicolon at the end of the sentence");
                
                currentToken = "";

                if(pointer.Current == '=')
                {
                    yield return new Token(">=", Token.TokenType.MayorIgual);
                    continue;
                }
                yield return new Token(">", Token.TokenType.MayorQ);
            }
            
            if (pointer.Current == '<')
            {
                if (currentToken != "")
                    yield return new Token(currentToken, ClasificarToken(currentToken));
                
                if(!pointer.MoveNext())
                    throw new SyntaxException("Missing Semicolon at the end of the sentence");
                
                currentToken = "";

                if(pointer.Current == '=')
                {
                    yield return new Token("<=", Token.TokenType.MenorIgual);
                    continue;
                }
                yield return new Token("<", Token.TokenType.MenorQ);
            }
            
            
            if (pointer.Current == '!')
            {
                if (currentToken != "")
                    yield return new Token(currentToken, ClasificarToken(currentToken));
                
                if(!pointer.MoveNext())
                    throw new SyntaxException("Missing Semicolon at the end of the sentence");
                
                currentToken = "";

                if(pointer.Current == '=')
                {
                    yield return new Token("!=", Token.TokenType.Different);
                    continue;
                }
                yield return new Token("!", Token.TokenType.Negation);
            }
            
            if (pointer.Current == '=')
            {
                if (currentToken != "")
                    yield return new Token(currentToken, ClasificarToken(currentToken));
                
                if(!pointer.MoveNext())
                    throw new SyntaxException("Missing Semicolon at the end of the sentence");
                    
                currentToken = "";
                if(pointer.Current == '=')
                {
                    yield return new Token("==", Token.TokenType.Comparar);
                    continue;
                }
                
                if (pointer.Current == '>')
                {
                    yield return(new Token("=>", Token.TokenType.Arrow));
                    continue;
                }
                yield return new Token("=", Token.TokenType.Igual);
                if (char.IsWhiteSpace(pointer.Current))
                    continue;
            }
            
            if (Symbols.ContainsKey(pointer.Current.ToString()))
            {
                if (currentToken != "")
                    yield return new Token(currentToken, ClasificarToken(currentToken));
                yield return new Token(pointer.Current.ToString(), Symbols[pointer.Current.ToString()]);
                currentToken = "";
                continue;
            }
            
            if (pointer.Current == '.')
            {
                pointer.MoveNext();
                if (pointer.Current == '.')
                {
                    pointer.MoveNext();
                    if (pointer.Current == '.')
                    {
                        yield return new Token(currentToken, ClasificarToken(currentToken));
                        yield return new Token("...", Token.TokenType.PuntosSuspensivos);
                        currentToken = "";
                        continue;
                    }
                    throw new LexicalException("..");
                }
                currentToken += ',';
                continue; 
            }
            
            
            currentToken += pointer.Current;
        }
        if (isPlainText)
        {
            throw new SyntaxException("Missing closing Quotation Mark (\") after '"+currentToken+"'.");
        }

        if (currentToken != "")
            yield return new Token(currentToken, ClasificarToken(currentToken));
    }

    private static Token.TokenType ClasificarToken(string token)
    {
        if (double.TryParse(token, out _))
        {
            return Token.TokenType.Number;
        }
        switch(token.ToLower()){
            case "sqrt":
                return Token.TokenType.Sqrt;
            case "true":
            case "false":
                return Token.TokenType.Bool;
            case "sin":
                return Token.TokenType.Sin;
            case "cos":
                return Token.TokenType.Cos;
            case "tan":
                return Token.TokenType.Tan;
            case "cot" :
                return Token.TokenType.Cot;
            case "log":
                return Token.TokenType.Log;
            case "print":
                return Token.TokenType.Print;
            case "let":
                return Token.TokenType.VarDeclarationKeyWord;
            case "in":
                return Token.TokenType.VarInKeyWord;
            case "function":
                return Token.TokenType.FunDeclarationKeyWord;
            case "if":
                return Token.TokenType.If;
            case "else":
                return Token.TokenType.Else;
            case "point":
                return Token.TokenType.PointDecl;
            case "circle":
                return Token.TokenType.CircleDecl;
            case "measure":
                return Token.TokenType.MeasureDecl;
            case "line":
                return Token.TokenType.LineDecl;
            case "segment":
                return Token.TokenType.SegmentDecl;
            case "ray":
                return Token.TokenType.RayDecl;
            case "arc":
                return Token.TokenType.ArcDecl;
            case "draw":
                return Token.TokenType.Draw;
            case "color":
                return Token.TokenType.Color;
            case "restore":
                return Token.TokenType.Restore;
            case "intersect":
                return Token.TokenType.Intersect;
            case "_":
                return Token.TokenType.Underscore;
            case "points":
                return Token.TokenType.PointsFigure;
            case "samples":
                return Token.TokenType.SampleCanvas;
            case "randoms":
                return Token.TokenType.RandomsValuesN;
            default:
                if(char.IsDigit(token.First()))
                    throw new LexicalException(token);
                    
                return Token.TokenType.ID;
        }
    }
}