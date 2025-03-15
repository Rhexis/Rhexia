using Rhexia.v2.Ast.Expressions;

namespace Rhexia.v2.Ast.Statements;

public record ForStatement(Expr Iterable, string Value, string? Index, List<Statement> Body)
    : Statement(StatementKind.For)
{
    public override string ToString()
    {
        return $"For: {{ Iterable: {Iterable}, Value: {Value}, Index: {Index}, Body: [{string.Join(", ", Body)}] }}";
    }
}