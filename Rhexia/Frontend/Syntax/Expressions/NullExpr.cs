namespace Rhexia.Frontend.Syntax.Expressions;

public record NullExpr(string _ = "null") : Expr
{
    public override string ToString()
    {
        return "null";
    }
}