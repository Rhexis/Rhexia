namespace Rhexia.v2.Ast.Expression;

public record NumericExpr(double Literal) : Expr
{
    public override string ToString()
    {
        return $"Number({Literal})";
    }
}