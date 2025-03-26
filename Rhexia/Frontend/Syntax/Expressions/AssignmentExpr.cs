namespace Rhexia.Frontend.Syntax.Expressions;

public record AssignmentExpr(Expr Left, Expr Right) : Expr
{
    public override string ToString()
    {
        return $"{{ Left: {Left}, Right: {Right} }}";
    }
}