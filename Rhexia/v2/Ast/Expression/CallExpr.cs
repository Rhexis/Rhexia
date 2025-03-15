namespace Rhexia.v2.Ast.Expression;

public record CallExpr(Expr Expr, List<Expr> Args) : Expr
{
    public override string ToString()
    {
        return $"{{ Expr: {Expr}, Args: [{string.Join(", ", Args)}] }}";
    }
}