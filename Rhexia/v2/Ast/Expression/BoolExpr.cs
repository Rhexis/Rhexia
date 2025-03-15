namespace Rhexia.v2.Ast.Expression;

public record BoolExpr(bool Boolean) : Expr
{
    public override string ToString()
    {
        return $"Bool({Boolean})";
    }
}