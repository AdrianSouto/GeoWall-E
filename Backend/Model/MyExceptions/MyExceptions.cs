using System;

namespace GeoWalle.Backend.Model.MyExceptions;

abstract class MyException : Exception{
    public MyException(string msg, string helpLink) : base(msg){
        base.HelpLink = helpLink;
    }
}
class LexicalException : MyException{
    public LexicalException(string tokenValue) : base(
        "! LEXICAL ERROR: "+tokenValue+" is not valid token.",
        "https://github.com/matcom/programming/tree/main/projects/hulk#error-l%C3%A9xico"){}
    
}
class SyntaxException : MyException{
    public SyntaxException(string msg) : base(
        "! SYNTAX ERROR: "+msg,
        "https://github.com/matcom/programming/tree/main/projects/hulk#error-sint%C3%A1tico"
    ){}
}
class SemanticException : MyException{
    public SemanticException(string msg) : base(
        "! SEMANTIC ERROR: "+msg,
        "https://github.com/matcom/programming/tree/main/projects/hulk#error-sem%C3%A1ntico"
    ){}
}