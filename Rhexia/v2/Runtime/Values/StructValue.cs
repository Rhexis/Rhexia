using Rhexia.v2.Ast;

namespace Rhexia.v2.Runtime.Values;

public record StructValue
(
    string Name,
    List<Parameter> Parameters,
    Dictionary<string, Value> Methods
) : Value(ValueType.Struct);