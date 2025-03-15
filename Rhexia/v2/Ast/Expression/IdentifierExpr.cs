namespace Rhexia.v2.Ast.Expression;

public record IdentifierExpr(string Identifier) : Expr
{
    public override string ToString()
    {
        return $"Identifier({Identifier})";
    }
}