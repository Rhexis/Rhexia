using Rhexia.Ast.Expressions;

namespace Rhexia.Ast.Statements;

public record WhileStatement(Expr Condition, List<Statement> Body) : Statement(StatementKind.While)
{
    public override string ToString()
    {
        return $"While: {{ Condition: {Condition}, Body: [{string.Join(", ", Body)}] }}";
    }
}