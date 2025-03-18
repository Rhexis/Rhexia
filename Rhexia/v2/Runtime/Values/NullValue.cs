namespace Rhexia.v2.Runtime.Values;

public record NullValue(string _ = "null") : Value(ValueType.Null);