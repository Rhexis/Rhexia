using Rhexia.Core;

namespace Rhexia.Runtime;

public static class Interpreter
{
    public static Value Evaluate(Ast.Statement statement, Env env)
    {
        return statement.Kind switch
        {
            // Statements
            Ast.NodeType.Program => Evaluate((Ast.Program)statement, env),
            Ast.NodeType.Variable => Evaluate((Ast.Variable)statement, env),
            Ast.NodeType.Function => Evaluate((Ast.Function)statement, env),
            
            // Expressions
            Ast.NodeType.BinaryExpression => Evaluate((Ast.BinaryExpression)statement, env),
            Ast.NodeType.AssignmentExpression => Evaluate((Ast.AssignmentExpression)statement, env),
            Ast.NodeType.MemberExpression => Evaluate((Ast.MemberExpression)statement, env),
            Ast.NodeType.CallExpression => Evaluate((Ast.CallExpression)statement, env),
            
            // Literals
            Ast.NodeType.IdentifierLiteral => Evaluate((Ast.IdentifierLiteral)statement, env),
            Ast.NodeType.ObjectLiteral => Evaluate((Ast.ObjectLiteral)statement, env),
            Ast.NodeType.NumericLiteral => new NumberValue(((Ast.NumericLiteral)statement).Value),
            Ast.NodeType.BoolLiteral => new BoolValue(((Ast.BoolLiteral)statement).Value),
            Ast.NodeType.NullLiteral => new NullValue(),
            
            _ => throw new ArgumentOutOfRangeException($"Unknown Ast statement:[{statement.Kind}]")
        };
    }

    // Statements
    private static Value Evaluate(Ast.Program program, Env env)
    {
        Value last = new NullValue();

        foreach (var statement in program.Body)
        {
            last = Evaluate(statement, env);
        }
        
        return last;
    }

    private static Value Evaluate(Ast.Variable variable, Env env)
    {
        var value = variable.Value != null ? Evaluate(variable.Value, env) : new NullValue();
        return env.Declare(variable.Identifier, value, variable.Constant);
    }

    private static Value Evaluate(Ast.Function func, Env env)
    {
        return env.Declare(func.Name, new FunctionValue(func.Name, func.Parameters, env, func.Body), true);
    }

    // Expressions
    private static Value Evaluate(Ast.BinaryExpression expr, Env env)
    {
         Value left = Evaluate(expr.Left, env);
         Value right = Evaluate(expr.Right, env);

         if (left.Type == ValueTypes.Number && right.Type == ValueTypes.Number)
         {
             return Evaluate((NumberValue)left, (NumberValue)right, expr.Operator);
         }

         return new NullValue();
    }

    private static Value Evaluate(Ast.AssignmentExpression expr, Env env)
    {
        if (expr.Assignee.Kind != Ast.NodeType.IdentifierLiteral) throw new Exception($"Invalid assignment: [{expr.Assignee}]");

        var assignee = (Ast.IdentifierLiteral)expr.Assignee;
        
        return env.Assign(assignee.Value, Evaluate(expr.Value, env));
    }

    private static Value Evaluate(Ast.MemberExpression expr, Env env)
    {
        // TODO :: FIX, LITERALLY ONLY WORKS FOR CURRENT PROGRAM INPUT
        
        if (expr.Object.Kind == Ast.NodeType.MemberExpression)
        {
            var obj = (Ast.MemberExpression)expr.Object;

            if (obj.Object.Kind == Ast.NodeType.IdentifierLiteral)
            {
                var identifier = (Ast.IdentifierLiteral)obj.Object;
                
                var root = (ObjectValue)env.Lookup(identifier.Value);
                
                var complexValue = (ObjectValue)root.Properties[((Ast.IdentifierLiteral)obj.Property).Value];
                
                return complexValue.Properties[((Ast.IdentifierLiteral)expr.Property).Value];
            }
        }
        
        return new NullValue();
    }

    private static Value Evaluate(Ast.CallExpression expr, Env env)
    {
        var args = expr.Args.Select(arg => Evaluate(arg, env)).ToArray();
        var call = Evaluate(expr.Call, env);

        if (call is FunctionValue func)
        {
            var scope = new Env(func.Env);

            for (int i = 0; i < func.Parameters.Count; i++)
            {
                scope.Declare(func.Parameters[i], args[i], false);
            }
            
            Value result = new NullValue();

            foreach (var statement in func.Body)
            {
                result = Evaluate(statement, scope);
            }
            
            return result;
        }

        throw new Exception($"Cannot call value that is not a function: [{call}]");
    }
    
    // Literals
    private static Value Evaluate(Ast.IdentifierLiteral identifierLiteral, Env env)
    {
        return env.Lookup(identifierLiteral.Value);
    }

    private static Value Evaluate(Ast.ObjectLiteral objectLiteral, Env env)
    {
        var properties = new Dictionary<string, Value>();

        foreach (var property in objectLiteral.Properties)
        {
            var value = property.Value != null
                ? Evaluate(property.Value, env)
                : env.Lookup(property.Key);
            
            properties.Add(property.Key, value);
        }
        
        return new ObjectValue(properties);
    }
    
    private static Value Evaluate(NumberValue left, NumberValue right, string op)
    {
        return new NumberValue(op switch
        {
            "+" => left.Value + right.Value,
            "-" => left.Value - right.Value,
            "*" => left.Value * right.Value,
            "/" => left.Value / right.Value,
            "%" => left.Value % right.Value,
            
            _ => throw new InvalidOperationException($"Unknown operator: [{op}]")
        });
    }
}