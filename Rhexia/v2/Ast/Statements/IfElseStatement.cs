using Rhexia.v2.Ast.Expressions;

namespace Rhexia.v2.Ast.Statements;

public record IfElseStatement(Expr Condition, List<Statement> Then, List<Statement> Else)
    : Statement(StatementKind.IfElse)
{
    public IfElseStatement(Expr Condition, List<Statement> Then) : this(Condition, Then, [])
    {
    }
    
    public override string ToString()
    {
        return $"{{ If: {Condition}, Then: [{string.Join(", ", Then)}], Else: [{string.Join(", ", Else)}] }}";
    }
}