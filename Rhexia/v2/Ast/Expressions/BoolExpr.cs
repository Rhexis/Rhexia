namespace Rhexia.v2.Ast.Expressions;

public record BoolExpr(bool Boolean) : Expr
{
    public override string ToString()
    {
        return $"Bool({Boolean})";
    }
}