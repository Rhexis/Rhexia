namespace Rhexia.v2.Ast.Expression;

public record PrefixExpr(Op Op, Expr Expr) : Expr
{
    public override string ToString()
    {
        return $"{{ Op: {Op}, Expr: {Expr} }}";
    }
}