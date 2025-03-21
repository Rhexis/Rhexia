using Rhexia.Lex;

namespace Rhexia.Ast;

public enum Op
{
    Assign,
    
    Plus,
    Minus,
    Divide,
    Multiply,
    Modulus,
    
    Increment,
    Decrement,
    
    LessThan,
    GreaterThan,
    LessThanOrEqualTo,
    GreaterThanOrEqualTo,
    EqualTo,
    NotEqualTo,
    
    Not,
    And,
    Or,
    
    In,
    NotIn,
}

public static class OpExtensions
{
    public static Op ToOp(this TokenKind kind)
    {
        return kind switch
        {
            TokenKind.Plus => Op.Plus,
            TokenKind.Minus => Op.Minus,
            TokenKind.Multiply => Op.Multiply,
            TokenKind.Divide => Op.Divide,
            TokenKind.Not => Op.Not,
            TokenKind.NotEqualTo => Op.NotEqualTo,
            TokenKind.Modulus => Op.Modulus,
            TokenKind.EqualTo => Op.EqualTo,
            TokenKind.Assign => Op.Assign,
            TokenKind.LessThan => Op.LessThan,
            TokenKind.GreaterThan => Op.GreaterThan,
            TokenKind.LessThanOrEqualTo => Op.LessThanOrEqualTo,
            TokenKind.GreaterThanOrEqualTo => Op.GreaterThanOrEqualTo,
            TokenKind.And => Op.And,
            TokenKind.Or => Op.Or,
            TokenKind.Increment => Op.Increment,
            TokenKind.Decrement => Op.Decrement,
            
            _ => throw new Exception($"Unexpected token: {kind}")
        };
    }
}