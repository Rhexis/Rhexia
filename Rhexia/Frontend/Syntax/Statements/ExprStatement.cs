using Rhexia.Frontend.Syntax.Expressions;

namespace Rhexia.Frontend.Syntax.Statements;

public record ExprStatement(Expr Expr) : Statement(StatementKind.Expression)
{
    public override string ToString()
    {
        return $"Expr: {{ Expr: {Expr} }}";
    }
}