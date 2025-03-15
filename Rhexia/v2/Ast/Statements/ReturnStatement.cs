using Rhexia.v2.Ast.Expressions;

namespace Rhexia.v2.Ast.Statements;

public record ReturnStatement(Expr Expr) : Statement(StatementKind.Return)
{
    public override string ToString()
    {
        return $"Return: {{ Expr: {Expr} }}";
    }
}