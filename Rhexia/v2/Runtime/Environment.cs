using Rhexia.v2.Runtime.Values;

namespace Rhexia.v2.Runtime;

public class Environment
{
    public Dictionary<string, Value> Values { get; } = [];

    public void Add(string key, Value value)
    {
        if (Values.TryAdd(key, value))
        {
            throw new InvalidOperationException($"Could not add {{ key: {key}, value: {value} }}");
        }
    }

    public Value? Get(string key)
    {
        return Values.GetValueOrDefault(key);
    }

    public void Remove(string key)
    {
        Values.Remove(key);
    }
}