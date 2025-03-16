namespace Rhexia.v2.Ast.Expressions;

public record ListExpr(List<Expr> Items) : Expr
{
    public override string ToString()
    {
        return $"List: [{string.Join(", ", Items)}]";
    }
}

public record ListIndexExpr(Expr Expr, Expr? Index) : Expr
{
    public override string ToString()
    {
        return $"{{ Expr: {Expr}, Index: [{Index}] }}";
    }
}