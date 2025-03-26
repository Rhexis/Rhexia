namespace Rhexia.Frontend.Semantic.Runtime.Values;

public abstract record Value(ValueType Type);

public enum ValueType
{
    Numeric,
    String,
    Bool,
    Null,
    Function,
    Object,
    List,
    Native,
}