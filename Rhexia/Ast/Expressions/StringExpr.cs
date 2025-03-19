namespace Rhexia.Ast.Expressions;

public record StringExpr(string Literal) : Expr
{
    public override string ToString()
    {
        return $"String({Literal})";
    }
}