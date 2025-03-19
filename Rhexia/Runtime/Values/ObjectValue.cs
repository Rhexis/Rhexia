namespace Rhexia.Runtime.Values;

public record ObjectValue
(
    string Name,
    Dictionary<string, Value> Fields
) : Value(ValueType.Object);