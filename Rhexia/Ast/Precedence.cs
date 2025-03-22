using Rhexia.Lex;

namespace Rhexia.Ast;

public enum Precedence
{
    Lowest,
    Statement,
    Assign,
    Sum,
    Product,
    LessThanGreaterThan,
    Equals,
    AndOr,
    Identifier,
    Prefix,
    Postfix,
    Call,
}

public static class PrecedenceExtensions
{
    public static Precedence ToPrecedence(this TokenKind kind)
    {
        return kind switch
        {
            TokenKind.LeftRoundBracket 
                or TokenKind.Dot 
                or TokenKind.LeftSquareBracket
                => Precedence.Call,
            
            TokenKind.Increment
                or TokenKind.Decrement 
                => Precedence.Postfix,
            
            TokenKind.Identifier
                => Precedence.Identifier,
            
            TokenKind.And 
                or TokenKind.Or 
                => Precedence.AndOr,
            
            TokenKind.EqualTo 
                or TokenKind.NotEqualTo 
                => Precedence.Equals,
            
            TokenKind.LessThan 
                or TokenKind.GreaterThan 
                or TokenKind.LessThanOrEqualTo 
                or TokenKind.GreaterThanOrEqualTo
                => Precedence.LessThanGreaterThan,
            
            TokenKind.Multiply 
                or TokenKind.Divide 
                => Precedence.Product,
            
            TokenKind.Plus 
                or TokenKind.Minus 
                => Precedence.Sum,
            
            TokenKind.While
                or TokenKind.If
                or TokenKind.Else
                or TokenKind.For
                => Precedence.Statement,
            
            TokenKind.Assign => Precedence.Assign,
            
            _ => Precedence.Lowest
        };
    }
}