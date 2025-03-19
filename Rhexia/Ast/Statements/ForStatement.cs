using Rhexia.Ast.Expressions;

namespace Rhexia.Ast.Statements;

public record ForStatement(VarStatement Init, Expr Condition, Expr Increment, List<Statement> Body)
    : Statement(StatementKind.For)
{
    public override string ToString()
    {
        return $"For: {{ Init: {Init}, Condition: {Condition}, Increment: {Increment}, Body: [{string.Join(", ", Body)}] }}";
    }
}