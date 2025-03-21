using Rhexia.Extensions;
using Rhexia.Runtime.StandardLib;
using Rhexia.Runtime.Values;
using ValueType = Rhexia.Runtime.Values.ValueType;

namespace Rhexia.Runtime;

public class Environment
{
    public Environment? Parent { get; set; }
    public Dictionary<string, Value> Values { get; } = [];
    public Dictionary<string, Value> Functions { get; } = [];
    public Dictionary<string, Value> Globals { get; } = new()
    {
        ["print"] = new NativeFunctionValue(NativeFunctions.Print),
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