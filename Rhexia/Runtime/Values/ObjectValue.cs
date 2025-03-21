namespace Rhexia.Runtime.Values;

public record ObjectValue
(
    string Name,
    List<string> Fields,
    List<string> Functions,
    Environment Env
) : Value(ValueType.Object);