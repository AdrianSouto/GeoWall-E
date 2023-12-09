namespace GeoWalle.Backend.Model.Objects;

public class Token{

    public enum TokenType
    {
        Number,
        Sum,
        Resta,
        Product,
        Div,
        Pow,
        OpenParenthesis,
        CloseParenthesis,
        ID,
        Igual,
        Text,
        Semicolon,
        Sin,
        Cos,
        Tan,
        Cot,
        Sqrt,
        Log,
        Print,
        VarDeclarationKeyWord,
        VarInKeyWord,
        FunDeclarationKeyWord,
        Concat,
        Comma,
        If,
        Else,
        Comparar,
        Negation,
        Different,
        And,
        Or,
        Resto,
        Bool,
        MayorQ,
        Arrow,
        MenorQ,
        MayorIgual,
        MenorIgual,
        OpenLlaves,
        ClosedLLaves,
        Comillas,
        PointDecl,
        CircleDecl,
        LineDecl,
        SegmentDecl,
        RayDecl,
        ArcDecl,
        Draw,
        Color,
        Restore,
        Intersect,
        PointsFigure,
        SampleCanvas,
        RandomsValuesN,
        MeasureDecl,
        PuntosSuspensivos
    }

    public string Value{get;}
    public TokenType Type{get;}

    public Token(string value, TokenType type){
        this.Value = value;
        this.Type = type;
    }
}