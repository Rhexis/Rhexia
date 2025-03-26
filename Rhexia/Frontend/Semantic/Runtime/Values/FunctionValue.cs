using Rhexia.Frontend.Syntax;
using Rhexia.Frontend.Syntax.Statements;

namespace Rhexia.Frontend.Semantic.Runtime.Values;

public record FunctionValue
(
    string Name,
    List<Parameter> Parameters,
    List<Statement> Body
) : Value(ValueType.Function);