namespace Rhexia.Frontend.Semantic.Runtime.Values;

public record NullValue(string _ = "null") : Value(ValueType.Null);