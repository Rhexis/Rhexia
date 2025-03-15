using Rhexia.v2.Ast.Expressions;

namespace Rhexia.v2.Ast.Statements;

public record WhileStatement(Expr Condition, List<Statement> Body) : Statement(StatementKind.While)
{
    public override string ToString()
    {
        return $"While: {{ Condition: {Condition}, Body: [{string.Join(", ", Body)}] }}";
    }
}