using Rhexia.Frontend.Semantic.Runtime.Values;
using ValueType = Rhexia.Frontend.Semantic.Runtime.Values.ValueType;

namespace Rhexia.Frontend.Semantic.Runtime.StandardLib;

public delegate Value NativeFunctionCall(List<Value> args);

public record NativeFunctionValue(NativeFunctionCall Call) : Value(ValueType.Native);