namespace Rhexia.Runtime.Values;

public record NullValue(string _ = "null") : Value(ValueType.Null);