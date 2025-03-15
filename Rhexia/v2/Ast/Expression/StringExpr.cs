namespace Rhexia.v2.Ast.Expression;

public record StringExpr(string Literal) : Expr
{
    public override string ToString()
    {
        return $"String({Literal})";
    }
}