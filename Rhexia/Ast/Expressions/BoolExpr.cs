namespace Rhexia.Ast.Expressions;

public record BoolExpr(bool Literal) : Expr
{
    public override string ToString()
    {
        return $"Bool({Literal})";
    }
}