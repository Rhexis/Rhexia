namespace Rhexia.v2.Ast.Expressions;

public record ListExpr(List<Expr> Items) : Expr
{
    public override string ToString()
    {
        return $"List: [{string.Join(", ", Items)}]";
    }
}

public record ListIndexExpr(IdentifierExpr Identifier, Expr? Index) : Expr
{
    public override string ToString()
    {
        return $"List Index: {{ Identifier: {Identifier}, Index: [{Index}] }}";
    }
}