namespace Rhexia.Frontend.Syntax.Expressions;

public record NumericExpr(double Literal) : Expr
{
    public override string ToString()
    {
        return $"Numeric({Literal})";
    }
}