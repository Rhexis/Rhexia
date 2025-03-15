namespace Rhexia.v2.Ast.Expression;

public record GetExpr(Expr Expr, string Field) : Expr
{
    public override string ToString()
    {
        return $"{{ Expr: {Expr}, Field: {Field} }}";
    }
}