namespace Rhexia.v2.Ast.Expressions;

public record StructExpr(Expr Expr, Dictionary<string, Expr> Fields) : Expr
{
    public override string ToString()
    {
        return $"{{ Expr: {Expr}, Fields: {Fields} }}";
    }
}