using Rhexia.Frontend.Syntax.Expressions;

namespace Rhexia.Frontend.Syntax.Statements;

public record ReturnStatement(Expr Expr) : Statement(StatementKind.Return)
{
    public override string ToString()
    {
        return $"Return: {{ Expr: {Expr} }}";
    }
}