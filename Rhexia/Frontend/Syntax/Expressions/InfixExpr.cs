namespace Rhexia.Frontend.Syntax.Expressions;

public record InfixExpr(Expr Left, Op Op, Expr Right) : Expr
{
    public override string ToString()
    {
        return $"{{ Left: {Left}, Op: {Op}, Right: {Right} }}";
    }
}