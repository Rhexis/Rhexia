using Rhexia.v2.Ast;
using Rhexia.v2.Ast.Expressions;
using Rhexia.v2.Ast.Statements;
using Rhexia.v2.Runtime.Values;

namespace Rhexia.v2.Runtime;

public class Interpreter(AbstractSyntaxTree ast)
{
    public Environment Env { get; } = new();

    public bool Execute()
    {
        try
        {
            foreach (var statement in ast.Statements)
            {
                Interpret(statement);
            }
            return true;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return false;
        }
    }

    private void Interpret(Statement statement)
    {
        switch (statement.Kind)
        {
            case StatementKind.Var:
                var varStatement = (VarStatement)statement;
                Env.Add(varStatement.Name, CompileExpr(varStatement.Expr));
                break;
            
            case StatementKind.Function:
                var functionStatement = (FunctionStatement)statement;
                // TODO :: IMPLEMENT
                break;
            
            case StatementKind.For:
                var forStatement = (ForStatement)statement;
                // TODO :: IMPLEMENT
                break;
            
            case StatementKind.While:
                var whileStatement = (WhileStatement)statement;
                // TODO :: IMPLEMENT
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
            
            case StatementKind.Return:
                var returnStatement = (ReturnStatement)statement;
                // TODO :: IMPLEMENT
                break;
            
            case StatementKind.Expression:
                CompileExpr(((ExprStatement)statement).Expr);
                break;
            
            default:
                throw new Exception($"Unknown statement kind: {statement.Kind}");
        }
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
            _ => null!
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

        if (expr is GetExpr getExpr)
        {
            // TODO :: Handle Struct Expr then this.
        }

        if (expr is AssignmentExpr assignmentExpr)
        {
            var value = CompileExpr(assignmentExpr.Right);

            if (assignmentExpr.Left is IdentifierExpr assignIdentifierExpr)
            {
                Env.Add(assignIdentifierExpr.Identifier, value);
            }
            else if (assignmentExpr.Left is ListIndexExpr assignListIndexExpr)
            {
                // TODO :: IMPLEMENT
            }
            else if (assignmentExpr.Left is GetExpr assignGetExpr)
            {
                // TODO :: IMPLEMENT
            }
            else
            {
                throw new Exception($"Undefined assignment expression: {assignmentExpr}");
            }
        }

        if (expr is CallExpr callExpr)
        {
            // TODO :: IMPLEMENT
        }

        if (expr is StructExpr structExpr)
        {
            // TODO :: IMPLEMENT
        }

        if (expr is ClosureExpr closureExpr)
        {
            // TODO :: IMPLEMENT
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

        var left = CompileExpr(postfixExpr.Expr);
        return postfixExpr.Op switch
        {
            Op.Increment => new NumericValue(((NumericValue)left).Value + 1),
            Op.Decrement => new NumericValue(((NumericValue)left).Value - 1),
            
            _ => throw new Exception($"Unhandled postfix operator: {postfixExpr.Op}"),
        };
    }
}