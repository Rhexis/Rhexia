using Rhexia.Ast;

namespace Rhexia.Runtime.Values;

public record StructValue
(
    string Name,
    List<Parameter> Parameters,
    Dictionary<string, Value> Methods
) : Value(ValueType.Struct);