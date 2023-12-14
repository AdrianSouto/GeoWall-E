using System;
using System.Collections.Generic;
using GeoWalle.Backend.Model.Context;
using GeoWalle.Backend.Model.Expressions;
using GeoWalle.Backend.Model.Expressions.Binary;
using GeoWalle.Backend.Model.Expressions.GraphicObjects;
using GeoWalle.Backend.Model.Expressions.PredFunctions;
using GeoWalle.Backend.Model.GraphicObjects;
using GeoWalle.Backend.Model.MyExceptions;
using GeoWalle.Backend.Model.Objects;
using Hulk.Model;
using Walle.Model.Expressions.Unary;

namespace GeoWalle.Backend.Parser;

public class Parser
{
    private Randoms randoms = new();
    private Samples samples = new();
    private Points points;
    private IEnumerator<Token> tokens;
    public Parser(IEnumerator<Token> tokens)
    {
        this.tokens = tokens;
        tokens.MoveNext();
        Color.ChangeColor("black");
        UserFunction.CleanFunctions();
    }

    public IEnumerable<MyExpression> Parse(Context context)
    {
        do
        {
            yield return ParseExpression(context);
        } while (tokens.MoveNext());
    }
    private MyExpression ParseExpression(Context context)
    {
        switch (tokens.Current.Type)
        {
            case Token.TokenType.FunDeclarationKeyWord:
                tokens.MoveNext();
                ParseFunDeclaration();
                return null!;
            case Token.TokenType.VarDeclarationKeyWord:
                tokens.MoveNext();
                return ParseVarDeclaration(context);
            case Token.TokenType.If:
                tokens.MoveNext();
                return ParseIfDeclaration(context);
            /*case Token.TokenType.ID:
                ObjectDecl(context);
                return null!;*/
            case Token.TokenType.Draw:
                tokens.MoveNext();
                return ParseDraw(context);
            default:
                return ParseAndOr(context);
        }
    }

    private MyExpression ParseDraw(Context context)
    {
        Draw d;
        
        MyExpression objToDraw = ParseExpression(context);
        string label = "";
        if (tokens.Current.Type == Token.TokenType.Text)
        {
            label = tokens.Current.Value;
            tokens.MoveNext();
        }

        d = new Draw(objToDraw, label);
        
        return d;
    }

    private void ObjectDecl(Context context)
    {
            List<string> vars = GetFunDeclParams(Token.TokenType.Igual);
            tokens.MoveNext();
            MyExpression value = ParseExpression(context);
            if (vars.Count == 1)
            {
                context.AddVar(new Variable(vars[0], value));
                return;
            }

            var sequence = value as Sequence;
            sequence?.MoveNext();
            for (int i = 0; i < vars.Count; i++)
            {
                if (context.FindVar(vars[i]) == null && vars[i] != "_"&& i < vars.Count - 1)
                {
                    context.AddVar(new Variable(vars[i], sequence!.Current));
                }
                if (i >= vars.Count - 2 )
                {
                    if (vars[i+1]!="_")
                    {
                        context.AddVar(new Variable(vars[i+1], value));
                    }
                    break;
                }
                sequence!.MoveNext();
            }
    }

    private MyExpression ParseAndOr(Context context)
    {
        MyExpression left = Compare(context);
        
        while(
            tokens.Current.Type != Token.TokenType.Semicolon && (
                  tokens.Current.Type == Token.TokenType.And || 
                  tokens.Current.Type == Token.TokenType.Or)
             )
        {
            Token.TokenType currentOp = tokens.Current.Type;
            tokens.MoveNext();
            MyExpression right = Compare(context);
            switch (currentOp)
            {
                case Token.TokenType.And:
                    left = new And(left, right);
                    break;
                case Token.TokenType.Or:
                    left = new Or(left, right);
                    break;
            }
        }
        return left;
    }
    private MyExpression Compare(Context context)
    {
        MyExpression left = ParseSum(context);
        
        while(tokens.Current.Type != Token.TokenType.Semicolon && 
              tokens.Current.Type
                  is Token.TokenType.Comparar
                  or Token.TokenType.Different
                  or Token.TokenType.MayorQ
                  or Token.TokenType.MenorQ 
                  or Token.TokenType.MayorIgual
                  or Token.TokenType.MenorIgual)
        {
            Token.TokenType currentOp = tokens.Current.Type;
            tokens.MoveNext();
            MyExpression right = ParseSum(context);
            switch (currentOp)
            {
                case Token.TokenType.Comparar:
                    left = new Compare(left, right);
                    break;
                case Token.TokenType.Different:
                    left = new Different(left, right);
                    break;
                case Token.TokenType.MayorQ:
                    left = new MayorQ(left, right);
                    break;
                case Token.TokenType.MenorQ:
                    left = new MenorQ(left, right);
                    break;
                case Token.TokenType.MayorIgual:
                    left = new MayorIgual(left, right);
                    break;
                case Token.TokenType.MenorIgual:
                    left = new MenorIgual(left, right);
                    break;
            }
        }
        return left;
    }
    
    private MyExpression ParseSum(Context context)
    {
        MyExpression left = Concat(context);
        
        while(tokens.Current.Type != Token.TokenType.Semicolon && (tokens.Current.Type == Token.TokenType.Sum || tokens.Current.Type == Token.TokenType.Resta))
        {
            Token.TokenType currentOp = tokens.Current.Type;
            tokens.MoveNext();
            MyExpression right = Concat(context);
            if(currentOp == Token.TokenType.Sum)
                if (left is Sequence sleft && right is Sequence sright)
                {
                    left = sleft + sright;
                }else
                    left = new Addition(left, right);
            else 
                left = new Subtraction(left, right);
        }
        return left;
    }
    
    private MyExpression Concat(Context context)
    {
        MyExpression left = ParseMult(context);
        
        while(tokens.Current.Type != Token.TokenType.Semicolon && tokens.Current.Type == Token.TokenType.Concat)
        {
            Token.TokenType currentOp = tokens.Current.Type;
            tokens.MoveNext();
            MyExpression right = ParseMult(context);
            if (currentOp == Token.TokenType.Concat)
                left = new Concat(left, right);
        }
        return left;
    }

    private MyExpression ParseMult(Context context)
    {
        MyExpression left = ParsePow(context);
        while(tokens.Current.Type != Token.TokenType.Semicolon && (
                  tokens.Current.Type == Token.TokenType.Product || 
                  tokens.Current.Type == Token.TokenType.Div ||
                  tokens.Current.Type == Token.TokenType.Resto
                  ))
        {
            Token.TokenType currentOp = tokens.Current.Type;
            tokens.MoveNext();
            MyExpression right = ParsePow(context);
            if(currentOp == Token.TokenType.Product)
                left= new Product(left, right);
            else if(currentOp == Token.TokenType.Div)
                left = new Division(left, right);
            else
                left = new Resto(left, right);
        }

        return left;
    }

    private MyExpression ParsePow(Context context)
    {
        MyExpression left = ParseTerm(context);
        while (tokens.Current.Type != Token.TokenType.Semicolon && tokens.Current.Type == Token.TokenType.Pow)
        {
            Token.TokenType currentOp = tokens.Current.Type;
            tokens.MoveNext();
            MyExpression right = ParseTerm(context);
            if (currentOp == Token.TokenType.Pow)
                left = new Power(left, right);
        }

        return left;
    }

    private MyExpression ParseTerm(Context context)
    {
        Token current = tokens.Current;
        switch (current.Type)
        {
            case Token.TokenType.Number:
                tokens.MoveNext();
                return new Number(current.Value);
            case Token.TokenType.Text:
                tokens.MoveNext();
                return new Text(current.Value);
            case Token.TokenType.Bool:
                tokens.MoveNext();
                return new Bool(current.Value);
            case Token.TokenType.Sin:
                tokens.MoveNext();
                if (tokens.Current.Type != Token.TokenType.OpenParenthesis)
                    throw new SyntaxException("Missing Open Parenthesis in Sin declaration");
                tokens.MoveNext();
                return new Sin(GetParams(context));
            case Token.TokenType.Cos:
                tokens.MoveNext();
                if (tokens.Current.Type != Token.TokenType.OpenParenthesis)
                    throw new SyntaxException("Missing Open Parenthesis in Cos declaration");
                tokens.MoveNext();
                return new Cos(GetParams(context));
            case Token.TokenType.Tan:
                tokens.MoveNext();
                if (tokens.Current.Type != Token.TokenType.OpenParenthesis)
                    throw new SyntaxException("Missing Open Parenthesis in Tan declaration");
                tokens.MoveNext();
                return new Tan(GetParams(context));
            case Token.TokenType.Cot:
                tokens.MoveNext();
                if (tokens.Current.Type != Token.TokenType.OpenParenthesis)
                    throw new SyntaxException("Missing Open Parenthesis in Cot declaration");
                tokens.MoveNext();
                return new Cot(GetParams(context));
            case Token.TokenType.Log:
                tokens.MoveNext();
                if (tokens.Current.Type != Token.TokenType.OpenParenthesis)
                    throw new SyntaxException("Missing Open Parenthesis in Log declaration");
                tokens.MoveNext();
                return new Log(GetParams(context));
            case Token.TokenType.Sqrt:
                tokens.MoveNext();
                if (tokens.Current.Type != Token.TokenType.OpenParenthesis)
                    throw new SyntaxException("Missing Open Parenthesis in Sqrt declaration");
                tokens.MoveNext();
                return new Sqrt(GetParams(context));
            case Token.TokenType.Print:
                tokens.MoveNext();
                if (tokens.Current.Type != Token.TokenType.OpenParenthesis)
                    throw new SyntaxException("Missing Open Parenthesis in Print declaration");
                tokens.MoveNext();
                return new Print(GetParams(context));
            case Token.TokenType.MeasureDecl:
                tokens.MoveNext();
                if (tokens.Current.Type != Token.TokenType.OpenParenthesis)
                    throw new SyntaxException("Missing Open Parenthesis in measure declaration");
                tokens.MoveNext();
                return new Measure(GetParams(context));
            case Token.TokenType.CircleDecl:
                tokens.MoveNext();
                return ParseCircle(context);
            case Token.TokenType.PointDecl:
                tokens.MoveNext();
                if (tokens.Current.Type != Token.TokenType.ID)
                    throw new SyntaxException("VarName expected in let-in expression");
                context.AddVar(new Variable(tokens.Current.Value, new GPoint()));
                tokens.MoveNext();
                return null!;
            case Token.TokenType.SegmentDecl:
                tokens.MoveNext();
                if (tokens.Current.Type != Token.TokenType.OpenParenthesis)
                    throw new SyntaxException("Missing Open Parenthesis in segment declaration");
                tokens.MoveNext();
                return new GSegment(GetParams(context));
            case Token.TokenType.LineDecl:
                tokens.MoveNext();
                if (tokens.Current.Type != Token.TokenType.OpenParenthesis)
                    throw new SyntaxException("Missing Open Parenthesis in line declaration");
                tokens.MoveNext();
                return new GLine(GetParams(context));
            case Token.TokenType.RayDecl:
                tokens.MoveNext();
                if (tokens.Current.Type != Token.TokenType.OpenParenthesis)
                    throw new SyntaxException("Missing Open Parenthesis in line declaration");
                tokens.MoveNext();
                return new GRay(GetParams(context));
            case Token.TokenType.Intersect:
                tokens.MoveNext();
                if (tokens.Current.Type != Token.TokenType.OpenParenthesis)
                    throw new SyntaxException("Missing Open Parenthesis in intersect declaration");
                tokens.MoveNext();
                return new Intersect(GetParams(context));
            case Token.TokenType.Color:
                tokens.MoveNext();
                Color.ChangeColor(tokens.Current.Value);
                tokens.MoveNext();
                return null!;
            case Token.TokenType.Restore:
                tokens.MoveNext();
                Color.RestoreColor();
                return null!;
            case Token.TokenType.PointsFigure:
                tokens.MoveNext();
                if (tokens.Current.Type != Token.TokenType.OpenParenthesis)
                    throw new SyntaxException("Missing Open Parenthesis in the points(); declaration");
                tokens.MoveNext();
                var par = GetParams(context);
                if (points == null)
                    return new Points((IGraphicObject)par[0]);
                return points;
            case Token.TokenType.SampleCanvas:
                tokens.MoveNext();
                if (tokens.Current.Type != Token.TokenType.OpenParenthesis)
                    throw new SyntaxException("Missing Open Parenthesis in the samples(); declaration");
                tokens.MoveNext();
                if (tokens.Current.Type != Token.TokenType.CloseParenthesis)
                    throw new SyntaxException("Missing Closed Parenthesis in the samples(); declaration");
                tokens.MoveNext();
                return samples;
            case Token.TokenType.RandomsValuesN:
                tokens.MoveNext();
                if (tokens.Current.Type != Token.TokenType.OpenParenthesis)
                    throw new SyntaxException("Missing Open Parenthesis in the randoms(); declaration");
                tokens.MoveNext();
                if (tokens.Current.Type != Token.TokenType.CloseParenthesis)
                    throw new SyntaxException("Missing Closed Parenthesis in the randoms(); declaration");
                tokens.MoveNext();
                return randoms;
            case Token.TokenType.OpenParenthesis:
                tokens.MoveNext();
                MyExpression m = ParseExpression(context);
                tokens.MoveNext();
                return m;
            case Token.TokenType.OpenLlaves:
                return ParseSequence(context);
            case Token.TokenType.Resta:
                tokens.MoveNext();
                return new NegativeNumber(ParseTerm(context));
            case Token.TokenType.Negation:
                tokens.MoveNext();
                return new Negation(ParseTerm(context));

        }

        return CheckIsVarFun(context);
        
    }

    private MyExpression ParseSequence(Context context)
    {
        int start;
        tokens.MoveNext();
        List<MyExpression> parametros = new ();
        if (int.TryParse(tokens.Current.Value, out start))
        {
            tokens.MoveNext();
            if (tokens.Current.Type == Token.TokenType.PuntosSuspensivos)
            {
                tokens.MoveNext();
                if (int.TryParse(tokens.Current.Value, out int end))
                {
                    tokens.MoveNext();
                    if (tokens.Current.Type != Token.TokenType.ClosedLLaves)
                        throw new SyntaxException("Cierre las llaves de la sequencia declarada");
                    tokens.MoveNext();
                    return new Sequence(start, end);
                }
                tokens.MoveNext();
                return new Sequence(start);
            }
            parametros.Add(new Number(start.ToString()));
            tokens.MoveNext();
        }
        foreach (var x in GetParams(context, Token.TokenType.ClosedLLaves))
        {
            parametros.Add(x);
        }
        return new Sequence(parametros);
    }

    private MyExpression ParseCircle(Context context)
    {
        if (tokens.Current.Type == Token.TokenType.OpenParenthesis)
        {
            tokens.MoveNext();
            return new GCircle(GetParams(context));
        }
        if (tokens.Current.Type != Token.TokenType.ID)
            throw new SyntaxException("VarName expected on Circle declaration");
        GPoint center = new GPoint();
        Measure measure = new Measure(new List<MyExpression>{center, new GPoint()});
        context.AddVar(
            new Variable(
                tokens.Current.Value,
                new GCircle(
                    new List<MyExpression>{new GPoint(), measure}
                )
            )
        );
        tokens.MoveNext();
        return null!;
    }

    private MyExpression CheckIsVarFun(Context context)
    {
        Variable? v = context.FindVar(tokens.Current.Value);
        if (v != null)
        {
            tokens.MoveNext();
            return v.VarTree;
        }
        
        UserFunction? f = UserFunction.FindFunction(tokens.Current.Value);
        if (f != null)
        {
            Context c = new Context();
            tokens.MoveNext();
            if (tokens.Current.Type != Token.TokenType.OpenParenthesis)
                throw new SyntaxException("Missing Open Parenthesis in fun declaration");
            tokens.MoveNext();
            List<MyExpression> list = GetParams(context);
            for (int i = 0; i < list.Count; i++)
            {
                string vName = f.ParamNames[i];
                MyExpression vTree = list[i];
                
                c.AddVar(new Variable(vName, vTree));
            }
            
            IEnumerator<Token> t = tokens;
            //current--;
            tokens = f.FunBody.GetEnumerator();
            tokens.MoveNext();
            MyExpression result = ParseExpression(c);
            tokens.MoveNext();
            tokens = t;
            return result;
        }
        
        if(tokens.Current.Type == Token.TokenType.ID)
        {
            ObjectDecl(context);
            return null!;
        }
        throw new SyntaxException("Invalid Expression '"+tokens.Current.Value+"'");
    }

    private void ParseFunDeclaration()
    {
        string funName;
        List<Token> funBody;
        List<string> funParams;
        if (tokens.Current.Type == Token.TokenType.ID)
            funName = tokens.Current.Value;
        else
            throw new SyntaxException("FuncName expected in function declaration");

        tokens.MoveNext();
        if (tokens.Current.Type == Token.TokenType.OpenParenthesis)
        {
            tokens.MoveNext();
            funParams = GetFunDeclParams();
        }
        else
            throw new SyntaxException("Open Parenthesis expected in function declaration");

        tokens.MoveNext();
        if (tokens.Current.Type != Token.TokenType.Arrow)
            throw new SyntaxException("'=>' expected in function declaration");
            
        tokens.MoveNext();
        funBody = new List<Token>();
        while (tokens.Current.Type != Token.TokenType.Semicolon){
            funBody.Add(tokens.Current);
            tokens.MoveNext();
        }
        funBody.Add(tokens.Current); //Para que el ; tamb se añada al cuerpo
        UserFunction.AddFunction(new UserFunction(funName, funParams, funBody));

    }
    private List<MyExpression> GetParams(Context context, Token.TokenType tokenTypeClose = Token.TokenType.CloseParenthesis)
    {
        //tokens.Current debe estar parado en el primer parametro despues del parentesis abierto
        List<MyExpression> paramExpressions = new List<MyExpression>();
        while (tokens.Current.Type != tokenTypeClose)
        {
            paramExpressions.Add(ParseExpression(context));
            if (tokens.Current.Type == tokenTypeClose)
                break;
            if(tokens.Current.Type != Token.TokenType.Comma)
                throw new SyntaxException("Expected ',' between parameters");
            tokens.MoveNext();
            if (tokens.Current.Type == Token.TokenType.Semicolon)
                throw new SyntaxException("Missing Parenthesis after function Declaration");
        }

        tokens.MoveNext();
        return paramExpressions;
    }
    private List<string> GetFunDeclParams(Token.TokenType tokenTypeClose = Token.TokenType.CloseParenthesis)
    {
        List<string> varNames = new List<string>();
        while (tokens.Current.Type != tokenTypeClose)
        {
            if (tokens.Current.Type != Token.TokenType.ID)
                throw new SyntaxException(tokens.Current.Value + " is not a valid VarName");
            varNames.Add(tokens.Current.Value);
            tokens.MoveNext();
            if(tokens.Current.Type == tokenTypeClose) break;
            if (tokens.Current.Type != Token.TokenType.Comma &&
                tokens.Current.Type != tokenTypeClose)
                throw new SyntaxException("Missing Comma Function Params");
            tokens.MoveNext();
        }

        return varNames;

    }
    private MyExpression ParseIfDeclaration(Context context)
    {
        if (tokens.Current.Type == Token.TokenType.OpenParenthesis)
        {
            if (bool.TryParse(ParseExpression(context).Evaluate(), out bool coditionResult))
            {
                if (coditionResult)
                {
                    MyExpression m = ParseExpression(context);
                    tokens.MoveNext();
                    return m;
                }
                while (tokens.Current.Type != Token.TokenType.Else)
                    tokens.MoveNext();

                tokens.MoveNext();
                MyExpression m1 = ParseExpression(context);
                tokens.MoveNext();
                return m1;
            }
        }
        throw new SyntaxException("If");
    }

    private MyExpression ParseVarDeclaration(Context context)
    {
        Context clonedContext = context.Clone();
        do
        {
            string varName;
            if (tokens.Current.Type == Token.TokenType.ID)
                varName = tokens.Current.Value;
            else
                throw new SyntaxException("VarName expected in let-in expression");
            tokens.MoveNext();
            if (tokens.Current.Type != Token.TokenType.Igual)
                throw new SyntaxException("'=' expected in let-in expression");
            tokens.MoveNext();
            MyExpression varValue = ParseExpression(clonedContext);
            clonedContext.AddVar(new Variable(varName, varValue));
        } while (tokens.Current.Type is Token.TokenType.Comma or Token.TokenType.Semicolon && tokens.MoveNext());

        
        if (tokens.Current.Type != Token.TokenType.VarInKeyWord)
            throw new SyntaxException("Missing 'in' KeyWord on let-in expression");
        
        tokens.MoveNext();
        return ParseExpression(clonedContext);
    }
}