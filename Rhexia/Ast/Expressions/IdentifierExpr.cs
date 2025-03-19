namespace Rhexia.Ast.Expressions;

public record IdentifierExpr(string Identifier) : Expr
{
    public override string ToString()
    {
        return $"Identifier({Identifier})";
    }
}