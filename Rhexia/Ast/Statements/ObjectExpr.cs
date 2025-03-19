using Rhexia.Ast.Expressions;

namespace Rhexia.Ast.Statements;

public record ObjectStatement
(
    IdentifierExpr Identifier,
    Dictionary<string, VarStatement> Fields,
    Dictionary<string, FunctionStatement> Functions
) : Statement(StatementKind.Object)
{
    public override string ToString()
    {
        return $"Object: {{ Identifier: {Identifier}, Fields: {Fields}, Functions: {Functions} }}";
    }
}