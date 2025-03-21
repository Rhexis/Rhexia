using Rhexia.Ast;
using Rhexia.Ast.Expressions;
using Rhexia.Ast.Statements;

namespace Rhexia.Runtime.Values;

public record FunctionValue
(
    string Name,
    List<Parameter> Parameters,
    List<Statement> Body
) : Value(ValueType.Function);