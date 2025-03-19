namespace Rhexia.Ast.Expressions;

public record StructExpr(IdentifierExpr Identifier, Dictionary<string, Expr> Fields) : Expr
{
    public override string ToString()
    {
        return $"Struct: {{ Identifier: {Identifier}, Fields: {Fields} }}";
    }
}