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
    Reserved,
    Number,
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
    LessThanOrEqual,
    GreaterThanOrEqual,
    Equal,
    NotEqual,
    
    Not,
    And,
    Or,
    True,
    False,
    Null,
    
    Comma,
    Colon,
    Semicolon,
    
    SingleQuote,
    DoubleQuote,
    
    StringLiteral,
    NumberLiteral,
    BooleanLiteral,
    
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
    Dot,
    Ampersand,
    Pipe,
    
    EndOfFile,
}