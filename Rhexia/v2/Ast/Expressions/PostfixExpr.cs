namespace Rhexia.v2.Ast.Expressions;

public record PostfixExpr(Expr Expr, Op Op) : Expr
{
    public override string ToString()
    {
        return $"{{ Expr: {Expr}, Op: {Op} }}";
    }
}