using Rhexia.Frontend.Syntax.Expressions;

namespace Rhexia.Frontend.Syntax.Statements;

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