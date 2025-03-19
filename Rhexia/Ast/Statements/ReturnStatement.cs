using Rhexia.Ast.Expressions;

namespace Rhexia.Ast.Statements;

public record ReturnStatement(Expr Expr) : Statement(StatementKind.Return)
{
    public override string ToString()
    {
        return $"Return: {{ Expr: {Expr} }}";
    }
}