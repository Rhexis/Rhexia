using Rhexia.Ast;
using Rhexia.Ast.Expressions;
using Rhexia.Ast.Statements;
using Rhexia.Runtime.StandardLib;
using Rhexia.Runtime.Values;

namespace Rhexia.Runtime;

public class Interpreter(AbstractSyntaxTree ast)
{
    public Environment Env { get; set; } = new();

    public Dictionary<string, Value> Globals { get; } = new()
    {
        ["print"] = new NativeFunctionValue(Functions.Print),
    };

    public bool Execute()
    {
        // try
        // {
        foreach (var statement in ast.Statements)
        {
            Interpret(statement);
        }
        return true;
        // }
        // catch (Exception e)
        // {
        //     Console.WriteLine(e);
        //     return false;
        // }
    }

    private Value? Interpret(Statement statement)
    {
        switch (statement.Kind)
        {
            case StatementKind.Var:
                var varStatement = (VarStatement)statement;
                var varValue = CompileExpr(varStatement.Expr);
                if (varValue is not FunctionValue)
                {
                    Env.Add(varStatement.Name, varValue);
                }
                else
                {
                    Globals.Add(varStatement.Name, varValue);
                }
                break;
            
            case StatementKind.Function:
                var functionStatement = (FunctionStatement)statement;
                Globals.Add
                (
                    functionStatement.Name,
                    new FunctionValue(functionStatement.Name, functionStatement.Parameters, functionStatement.Body)
                );
                break;
            
            // TODO :: Work out if we need to shift struct to statement from expr
            // case StatementKind.Struct:
            //     var structStatement = (StructStatement)statement;
            //     Globals.Add
            //     (
            //         structStatement.Name,
            //         new StructValue(structStatement.Name, structStatement.Fields, structStatement.Methods)
            //     );
            //     break;
            
            case StatementKind.For:
                var forStatement = (ForStatement)statement;
                Interpret(forStatement.Init);
                while (((BoolValue)CompileExpr(forStatement.Condition)).Value)
                {
                    foreach (var forBodyStatement in forStatement.Body)
                    {
                        Interpret(forBodyStatement);
                    }
                    CompileExpr(forStatement.Increment);
                }
                break;
            
            case StatementKind.While:
                var whileStatement = (WhileStatement)statement;
                while (((BoolValue)CompileExpr(whileStatement.Condition)).Value)
                {
                    foreach (var whileBodyStatement in whileStatement.Body)
                    {
                        Interpret(whileBodyStatement);
                    }
                }
                break;
            
            case StatementKind.IfElse:
                var ifElseStatement = (IfElseStatement)statement;
                var condition = (BoolValue)CompileExpr(ifElseStatement.Condition);
                if (condition.Value)
                {
                    foreach (var then in ifElseStatement.Then)
                    {
                        Interpret(then);
                    }
                }
                else
                {
                    foreach (var @else in ifElseStatement.Else)
                    {
                        Interpret(@else);
                    }
                }
                break;
            
            case StatementKind.Expression:
                return CompileExpr(((ExprStatement)statement).Expr);
            
            case StatementKind.Return:
                var returnStatement = (ReturnStatement)statement;
                return CompileExpr(returnStatement.Expr);
            
            default:
                throw new Exception($"Unknown statement kind: {statement.Kind}");
        }

        return null;
    }

    private Value CompileExpr(Expr expr)
    {
        return CompileSimpleExpr(expr)
            ?? CompileComplexExpr(expr)
            ?? CompilePrefixExpr(expr)
            ?? CompileInfixExpr(expr)
            ?? CompilePostfixExpr(expr)
            ?? throw new Exception($"Unknown expression: {expr}");
    }

    private Value? CompileSimpleExpr(Expr expr)
    {
        return expr switch
        {
            NumericExpr numericExpr => new NumericValue(numericExpr.Literal),
            StringExpr stringExpr => new StringValue(stringExpr.Literal),
            BoolExpr boolExpr => new BoolValue(boolExpr.Literal),
            IdentifierExpr identifierExpr => Env.Get(identifierExpr.Identifier) ??
                                             throw new Exception($"Undefined variable: {identifierExpr.Identifier}"),
            _ => null
        };
    }

    private Value? CompileComplexExpr(Expr expr)
    {
        if (expr is ListExpr listExpr)
        {
            var values = new List<Value>();
            foreach (var item in listExpr.Items)
            {
                values.Add(CompileExpr(item));
            }
            return new ListValue(values);
        }
        
        if (expr is ListIndexExpr listIndexExpr)
        {
            var list = (ListValue)CompileExpr(listIndexExpr.Identifier);
            var index = (NumericValue)CompileExpr(listIndexExpr.Index ?? throw new Exception($"Undefined list index: {listIndexExpr}"));
            return list.Values.ElementAt(Convert.ToInt32(index.Value));
        }

        if (expr is AssignmentExpr assignmentExpr)
        {
            var value = CompileExpr(assignmentExpr.Right);

            if (assignmentExpr.Left is IdentifierExpr assignIdentifierExpr)
            {
                Env.Add(assignIdentifierExpr.Identifier, value);
                return new NullValue();
            }
            
            if (assignmentExpr.Left is ListIndexExpr assignListIndexExpr)
            {
                var list = (ListValue)CompileExpr(assignListIndexExpr.Identifier);
                if (assignListIndexExpr.Index != null)
                {
                    var index = (NumericValue)CompileExpr(assignListIndexExpr.Index);
                    list.Values[(int)index.Value] = value;
                }
                return new NullValue();
            }
            
            if (assignmentExpr.Left is GetExpr assignGetExpr)
            {
                // TODO :: IMPLEMENT
                return new NullValue();
            }
            
            throw new Exception($"Undefined assignment expression: {assignmentExpr}");
        }

        if (expr is CallExpr callExpr)
        {
            var identifier = (IdentifierExpr)callExpr.Expr;
            if (!Globals.TryGetValue(identifier.Identifier, out var call)) throw new Exception($"Undefined function call: {identifier.Identifier}");
            var args = new List<Value>();
            
            foreach (var callExprArg in callExpr.Args)
            {
                args.Add(CompileExpr(callExprArg));
            }

            return Call(call, args);
        }

        if (expr is StructExpr structExpr)
        {
            // TODO :: IMPLEMENT
        }

        if (expr is ClosureExpr closureExpr)
        {
            return new FunctionValue("<Closure>", closureExpr.Parameters, closureExpr.Body, Env);
        }

        if (expr is GetExpr getExpr)
        {
            var obj = CompileExpr(getExpr.Expr);
            // TODO :: Rethink this
            if (obj is ListValue list)
            {
                if (getExpr.Field == "Length")
                {
                    return new NumericValue(list.Values.Count);
                }
            }
            throw new Exception($"Unhandled get expression: {getExpr.Expr}");
        }
        
        return null;
    }

    private Value? CompilePrefixExpr(Expr expr)
    {
        if (expr is not PrefixExpr prefixExpr) return null;
        
        var right = CompileExpr(prefixExpr.Expr);
        return prefixExpr.Op switch
        {
            Op.Not => new BoolValue(!((BoolValue)right).Value),
            Op.Minus => new NumericValue(-((NumericValue)right).Value),
            
            _ => throw new Exception($"Unhandled prefix operator: {prefixExpr.Op}"),
        };
    }
    
    private Value? CompileInfixExpr(Expr expr)
    {
        if (expr is not InfixExpr infixExpr) return null;
        
        var left = CompileExpr(infixExpr.Left);
        var right = CompileExpr(infixExpr.Right);

        return (left, infixExpr.Op, right) switch
        {
            // Numeric
            (NumericValue l, Op.Plus, NumericValue r) => new NumericValue(l.Value + r.Value),
            (NumericValue l, Op.Minus, NumericValue r) => new NumericValue(l.Value - r.Value),
            (NumericValue l, Op.Multiply, NumericValue r) => new NumericValue(l.Value * r.Value),
            (NumericValue l, Op.Divide, NumericValue r) => new NumericValue(l.Value / r.Value),
            (NumericValue l, Op.Modulus, NumericValue r) => new NumericValue(l.Value % r.Value),
            // String
            (NumericValue l, Op.Plus, StringValue r) => new StringValue($"{l.Value}{r.Value}"),
            (StringValue l, Op.Plus, NumericValue r) => new StringValue($"{l.Value}{r.Value}"),
            (StringValue l, Op.Plus, StringValue r) => new StringValue($"{l.Value}{r.Value}"),
            (StringValue l, Op.Plus, BoolValue r) => new StringValue($"{l.Value}{r.Value}"),
            (BoolValue l, Op.Plus, StringValue r) => new StringValue($"{l.Value}{r.Value}"),
            // Bool :: Equal To
            (NumericValue l, Op.EqualTo, NumericValue r) => new BoolValue(Math.Abs(l.Value - r.Value) < 0.0000001),
            (StringValue l, Op.EqualTo, StringValue r) => new BoolValue(l.Value == r.Value),
            (BoolValue l, Op.EqualTo, BoolValue r) => new BoolValue(l.Value == r.Value),
            // Bool :: Not Equal To
            (NumericValue l, Op.NotEqualTo, NumericValue r) => new BoolValue(Math.Abs(l.Value - r.Value) > 0.0000001),
            (StringValue l, Op.NotEqualTo, StringValue r) => new BoolValue(l.Value != r.Value),
            (BoolValue l, Op.NotEqualTo, BoolValue r) => new BoolValue(l.Value != r.Value),
            // Bool :: Less Than
            (NumericValue l, Op.LessThan, NumericValue r) => new BoolValue(l.Value < r.Value),
            // Bool :: Greater Than
            (NumericValue l, Op.GreaterThan, NumericValue r) => new BoolValue(l.Value > r.Value),
            // Bool :: Less Than Or Equal To
            (NumericValue l, Op.LessThanOrEqualTo, NumericValue r) => new BoolValue(l.Value <= r.Value),
            // Bool :: Greater Than Or Equal To
            (NumericValue l, Op.GreaterThanOrEqualTo, NumericValue r) => new BoolValue(l.Value >= r.Value),
            // Bool :: AND
            (BoolValue l, Op.And, BoolValue r) => new BoolValue(l.Value && r.Value),
            // Bool :: OR
            (BoolValue l, Op.Or, BoolValue r) => new BoolValue(l.Value || r.Value),

            _ => throw new Exception("Unhandled infix expression"),
        };
    }
    
    private Value? CompilePostfixExpr(Expr expr)
    {
        if (expr is not PostfixExpr postfixExpr) return null;

        var left = (NumericValue)CompileExpr(postfixExpr.Expr);
        var identifier = ((IdentifierExpr)postfixExpr.Expr).Identifier;

        switch (postfixExpr.Op)
        {
            case Op.Increment:
                Env.Set(identifier, new NumericValue(left.Value + 1));
                return new NumericValue(left.Value);
            
            case Op.Decrement:
                Env.Set(identifier, new NumericValue(left.Value - 1));
                return new NumericValue(left.Value);
            
            default:
                throw new Exception($"Unhandled postfix operator: {postfixExpr.Op}");
        }
    }

    private Value? Call(Value? call, List<Value> args)
    {
        if (call is FunctionValue func)
        {
            if (func.Parameters.Count != args.Count)
            {
                throw new Exception($"Invalid number of arguments for function {func.Name}");
            }

            // Snapshot environment
            var outerEnv = Env;
            
            var innerEnv = func.Env ?? new Environment();
            // TODO :: think more about this.
            if (func.Context != null) //&& func.Parameters.Any())
            {
                var context = CompileExpr(func.Context);
                innerEnv.Add("this", context);
            }

            var zipped = func.Parameters.Where(x => x.Name != "this").Zip(args);
            foreach (var (param, arg) in zipped)
            {
                innerEnv.Add(param.Name, arg);
            }
            
            // Change to inner environment
            Env = innerEnv;
            Value? result = null;
            foreach (var statement in func.Body)
            {
                result = Interpret(statement);
            }
            // Reset back to outer environment
            Env = outerEnv;
            
            return result;
        }

        if (call is NativeFunctionValue nativeFunc)
        {
            return nativeFunc.Call(args);
        }
        
        return null;
    }
}