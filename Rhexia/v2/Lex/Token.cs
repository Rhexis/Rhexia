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
    Identifier,
    Var,
    Assign,
    
    LeftCurlyBracket,
    RightCurlyBracket,
    LeftRoundBracket,
    RightRoundBracket,
    LeftSquareBracket,
    RightSquareBracket,
    
    LessThan,
    GreaterThan,
    LessThanOrEqualTo,
    GreaterThanOrEqualTo,
    EqualTo,
    NotEqualTo,
    
    Not,
    And,
    Or,
    True,
    False,
    Null,
    
    Comma,
    Colon,
    Semicolon,
    
    StringLiteral,
    NumericLiteral,
    
    While,
    For,
    If,
    Else,
    
    Plus,
    Minus,
    Divide,
    Multiply,
    Modulus,
    
    Function,
    In,
    Dot,
    Ampersand,
    Pipe,
    Return,
    
    EndOfFile,
}