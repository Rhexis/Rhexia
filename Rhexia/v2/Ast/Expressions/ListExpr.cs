namespace Rhexia.v2.Ast.Expressions;

public record ListExpr(List<Expr> Items) : Expr
{
    public override string ToString()
    {
        return $"List: [{string.Join(", ", Items)}]";
    }
}