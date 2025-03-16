namespace Rhexia.v2.Ast.Expressions;

public record CallExpr(Expr Expr, List<Expr> Args) : Expr
{
    public override string ToString()
    {
        return $"Call: {{ Expr: {Expr}, Args: [{string.Join(", ", Args)}] }}";
    }
}