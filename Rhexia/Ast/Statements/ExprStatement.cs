using Rhexia.Ast.Expressions;

namespace Rhexia.Ast.Statements;

public record ExprStatement(Expr Expr) : Statement(StatementKind.Expression)
{
    public override string ToString()
    {
        return $"Expr: {{ Expr: {Expr} }}";
    }
}