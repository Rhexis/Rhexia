namespace Rhexia.Frontend.Syntax.Expressions;

public record GetExpr(Expr Expr, string Field) : Expr
{
    public override string ToString()
    {
        return $"{{ Expr: {Expr}, Field: {Field} }}";
    }
}