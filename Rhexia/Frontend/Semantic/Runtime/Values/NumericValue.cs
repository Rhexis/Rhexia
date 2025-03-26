namespace Rhexia.Frontend.Semantic.Runtime.Values;

public record NumericValue(double Value) : Value(ValueType.Numeric);