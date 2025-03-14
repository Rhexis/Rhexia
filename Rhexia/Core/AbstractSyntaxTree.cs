namespace Rhexia.Core;

/// <summary>
/// Abstract Syntax Tree
/// </summary>
public static class Ast
{
    public enum NodeType
    {
        // Statements
        Program,
        Variable,
        Function,
        
        // Expressions
        BinaryExpression,
        AssignmentExpression,
        MemberExpression,
        CallExpression,
        
        // Literals
        PropertyLiteral,
        ObjectLiteral,
        NumericLiteral,
        BoolLiteral,
        IdentifierLiteral,
        NullLiteral,
    }

    public record Statement(NodeType Kind);

    // Statements
    public record Program(IEnumerable<Statement> Body) : Statement(NodeType.Program);
    
    public record Variable(bool Constant, string Identifier, Expression? Value = null) : Statement(NodeType.Variable);
    
    public record Function(string Name, List<string> Parameters, List<Statement> Body) : Statement(NodeType.Variable);

    // Expressions
    public record Expression(NodeType Kind) : Statement(Kind);

    public record BinaryExpression(Expression Left, Expression Right, string Operator): Expression(NodeType.BinaryExpression);

    public record AssignmentExpression(Expression Assignee, Expression Value): Expression(NodeType.AssignmentExpression);

    public record MemberExpression(Expression Object, Expression Property, bool Computed): Expression(NodeType.MemberExpression);

    public record CallExpression(List<Expression> Args, Expression Call): Expression(NodeType.CallExpression);

    // Literals
    public record PropertyLiteral(string Key, Expression? Value = null): Expression(NodeType.PropertyLiteral);
    
    public record ObjectLiteral(List<PropertyLiteral> Properties): Expression(NodeType.ObjectLiteral);
    
    public record NumericLiteral(int Value): Expression(NodeType.NumericLiteral);
    
    public record BoolLiteral(bool Value): Expression(NodeType.BoolLiteral);

    public record IdentifierLiteral(string Value): Expression(NodeType.IdentifierLiteral);

    public record NullLiteral() : Expression(NodeType.NullLiteral)
    {
        public string Value { get; init; } = "null";
    }
}