using Rhexia.Extensions;
using Rhexia.Frontend.Semantic.Runtime.StandardLib;
using Rhexia.Frontend.Semantic.Runtime.Values;
using ValueType = Rhexia.Frontend.Semantic.Runtime.Values.ValueType;

namespace Rhexia.Frontend.Semantic.Runtime;

public class Environment
{
    public Environment? Parent { get; set; }
    public Dictionary<string, Value> Values { get; } = [];
    public Dictionary<string, Value> Functions { get; } = [];
    public Dictionary<string, Value> Globals { get; } = new()
    {
        ["print"] = new NativeFunctionValue(NativeFunctions.Print),
        ["write"] = new NativeFunctionValue(NativeFunctions.Write),
    };

    public void Add(string key, Value value, ValueType? type = null)
    {
        var success = type switch
        {
            ValueType.Function => Functions.TryAdd(key, value),
            _ => Values.TryAdd(key, value)
        };

        if (!success)
        {
            throw new InvalidOperationException($"Could not add {{ key: {key}, value: {value} }}");
        }
    }

    public void Set(string key, Value value, ValueType? type = null)
    {
        var success = type switch
        {
            ValueType.Function => Functions.TrySet(key, value),
            _ => Values.TrySet(key, value)
        };

        if (!success)
        {
            Parent?.Set(key, value, type);
        }
    }

    public Value? Get(string key, ValueType? type = null)
    {
        return type switch
        {
            ValueType.Function => Functions.GetValueOrDefault(key) ?? Globals.GetValueOrDefault(key),
            _ => Values.GetValueOrDefault(key) ?? Parent?.Get(key, type)
        };
    }
}