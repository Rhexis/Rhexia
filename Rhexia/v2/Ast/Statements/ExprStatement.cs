using Rhexia.v2.Ast.Expressions;

namespace Rhexia.v2.Ast.Statements;

public record ExprStatement(Expr Expr) : Statement(StatementKind.Expression)
{
    public override string ToString()
    {
        return $"Expr: {{ Expr: {Expr} }}";
    }
}