namespace Rhexia.v2.Ast.Expressions;

public record AssignmentExpr(Expr Left, Expr Right) : Expr
{
    public override string ToString()
    {
        return $"{{ Left: {Left}, Right: {Right} }}";
    }
}