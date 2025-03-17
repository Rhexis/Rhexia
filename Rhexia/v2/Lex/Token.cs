namespace Rhexia.v2.Lex;

public record Token(TokenKind Kind, object Literal)
{
    public override string ToString()
    {
        return $"Token {{ kind: {Kind}, literal: \"{Literal}\" }}, ";
    }
}

public enum TokenKind
{
    Identifier, // A string of letters that isn't reserved word
    Var,        // var
    Assign,     // =
    
    LeftCurlyBracket,     // {
    RightCurlyBracket,    // }
    LeftRoundBracket,     // (
    RightRoundBracket,    // )
    LeftSquareBracket,    // [
    RightSquareBracket,   // ]
    
    LessThan,             // <
    GreaterThan,          // >
    LessThanOrEqualTo,    // <=
    GreaterThanOrEqualTo, // >=
    EqualTo,              // ==
    NotEqualTo,           // !=
    
    Not,       // not
    And,       // and
    Or,        // or
    True,      // true
    False,     // false
    Null,      // null
    
    Comma,     // ,
    Colon,     // :
    Semicolon, // ;
    
    StringLiteral,  // Everything between "..."
    NumericLiteral, // 0123456789._
    
    While,     // while
    For,       // for
    If,        // if
    Else,      // else
    
    Plus,      // +
    Minus,     // -
    Divide,    // /
    Multiply,  // *
    Modulus,   // %
    
    Increment, // ++
    Decrement, // --
    
    Struct,    // struct
    Function,  // function
    In,        // in
    Dot,       // .
    Ampersand, // &
    Pipe,      // |
    Return,    // return
    
    EndOfFile, // EOF char '\0'
}