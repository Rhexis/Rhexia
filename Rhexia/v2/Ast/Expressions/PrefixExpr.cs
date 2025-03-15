namespace Rhexia.v2.Ast.Expressions;

public record PrefixExpr(Op Op, Expr Expr) : Expr
{
    public override string ToString()
    {
        return $"{{ Op: {Op}, Expr: {Expr} }}";
    }
}