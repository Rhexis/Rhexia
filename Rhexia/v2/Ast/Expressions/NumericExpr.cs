namespace Rhexia.v2.Ast.Expressions;

public record NumericExpr(double Literal) : Expr
{
    public override string ToString()
    {
        return $"Numeric({Literal})";
    }
}