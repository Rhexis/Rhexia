namespace Rhexia.v1;

public class Env(Env? parent = null)
{
    private Env? _parent = parent;
    private readonly Dictionary<string, Value> _variables = new();
    private readonly List<string> _constants = [];

    public Value Declare(string name, Value value, bool constant)
    {
        if (!_variables.TryAdd(name, value)) throw new Exception($"Variable {name} is already declared");
        if (constant)
        {
            _constants.Add(name);
        }
        return value;
    }

    public Value Assign(string name, Value value)
    {
        if (_constants.Contains(name)) throw new Exception($"You cannot reassign a constant val: [{name}]");
        var env = Resolve(name);
        env._variables[name] = value;
        return value;
    }

    public Value Lookup(string name)
    {
        var env = Resolve(name);
        return env._variables[name];
    }
    
    public Env Resolve(string name)
    {
        if (_variables.ContainsKey(name))
        {
            return this;
        }
        
        if (_parent == null) throw new Exception($"Cannot resolve: [{name}]");
        
        return _parent.Resolve(name);
    }
}