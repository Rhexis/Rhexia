using Rhexia.Ast.Expressions;
using Rhexia.Ast.Statements;
using Rhexia.Lex;

namespace Rhexia.Ast;

public class Parser
{
    public Lexer Lexer { get; }

    private Token _current = new (TokenKind.EndOfFile, Lexer.EOF);
    private Token _next = new (TokenKind.EndOfFile, Lexer.EOF);

    public Parser(Lexer lexer)
    {
        Lexer = lexer;
        Next();
    }

    /// <summary>
    /// Snapshots & returns the current token and moves one step forward.
    /// </summary>
    /// <param name="expected">Expected token kind to return</param>
    /// <returns></returns>
    /// <exception cref="Exception">Thrown when expected token doesn't match the current token</exception>
    private Token Eat(TokenKind? expected = null)
    {
        if (expected != null && _current.Kind != expected) 
            throw new Exception($"Tried to eat: {expected} but found: {_current}");
        
        var token = _current;
        Next();
        return token;
    }

    /// <summary>
    /// Assert that a specific TokenKind was expected without eating it.
    /// </summary>
    /// <param name="expected"></param>
    /// <exception cref="Exception"></exception>
    private void Expect(TokenKind expected)
    {
        if (_current.Kind != expected) 
            throw new Exception($"Unexpected token: {_current}, expected: {expected}");
    }

    /// <summary>
    /// Makes one step forward & gets the next token.
    /// </summary>
    private void Next()
    {
        _current = _next;
        _next = Lexer.Analyse();
    }

    public AbstractSyntaxTree Parse()
    {
        var tree = new AbstractSyntaxTree();

        while (true)
        {
            Next();
            if (_current.Kind == TokenKind.EndOfFile) break;
            var statement = ParseStatement();
            if (statement == null) break;
            tree.Statements.Add(statement);
        }
        
        return tree;
    }

    private Statement? ParseStatement()
    {
        if (_current.Kind is TokenKind.EndOfFile or TokenKind.Semicolon)
        {
            return null;
        }
        
        return _current.Kind switch
        {
            TokenKind.For => ParseFor(),
            TokenKind.Function => ParseFunction(true),
            TokenKind.If => ParseIfElse(),
            TokenKind.Return => ParseReturn(),
            TokenKind.Var => ParseVar(),
            TokenKind.While => ParseWhile(),
            TokenKind.Object => ParseObject(),
            _ => new ExprStatement(ParseExpr())
        };
    }

    private VarStatement ParseVar()
    {
        Eat(TokenKind.Var);
        var name = Eat(TokenKind.Identifier).Literal.ToString()!;

        return new VarStatement(name, _current.Kind switch
        {
            TokenKind.Assign => ParseExpr(Eat(TokenKind.Assign)),
            _ => new NullExpr()
        });
    }

    private ForStatement ParseFor()
    {
        Eat(TokenKind.For);

        Eat(TokenKind.LeftRoundBracket);
        var init = ParseVar();
        Eat(TokenKind.Semicolon);
        var condition = ParseExpr();
        Eat(TokenKind.Semicolon);
        var increment = ParseExpr();
        Eat(TokenKind.RightRoundBracket);
        
        return new ForStatement(init, condition, increment, ParseBlock());
    }

    private IfElseStatement ParseIfElse()
    {
        Eat(TokenKind.If);
        Eat(TokenKind.LeftRoundBracket);
        var condition = ParseExpr();
        Eat(TokenKind.RightRoundBracket);
        
        var then = ParseBlock();
        
        if (_next.Kind != TokenKind.Else)
        {
            return new IfElseStatement(condition, then);
        }

        Eat(TokenKind.RightCurlyBracket);
        Eat(TokenKind.Else);
        
        return new IfElseStatement(condition, then, ParseBlock());
    }

    private ReturnStatement ParseReturn()
    {
        Eat(TokenKind.Return);
        return new ReturnStatement(ParseExpr());
    }

    private FunctionStatement ParseFunction(bool hasIdentifier)
    {
        Eat(TokenKind.Function);
        var name = hasIdentifier ? Eat(TokenKind.Identifier).Literal.ToString()! : "<Closure>";
        
        Eat(TokenKind.LeftRoundBracket);
        var parameters = new List<Parameter>();
        while (_current.Kind is not TokenKind.EndOfFile and not TokenKind.RightRoundBracket)
        {
            // Shouldn't enter here for functions with no parameters
            parameters.Add(new Parameter(Eat(TokenKind.Identifier).Literal.ToString()!));
            if (_current.Kind == TokenKind.Comma)
            {
                // Eat commas for functions with multiple parameters
                // Should allow for trailing commas
                Eat(TokenKind.Comma);
            }
        }
        Eat(TokenKind.RightRoundBracket);
        // TODO :: Decide if we should allow nesting functions.
        return new FunctionStatement(name, parameters, ParseBlock());
    }

    private WhileStatement ParseWhile()
    {
        Eat(TokenKind.While);
        
        Eat(TokenKind.LeftRoundBracket);
        var condition = ParseExpr();
        Eat(TokenKind.RightRoundBracket);
        
        return new WhileStatement(condition, ParseBlock());
    }

    private ObjectStatement ParseObject()
    {
        Eat(TokenKind.Object);
        var name = new IdentifierExpr(Eat(TokenKind.Identifier).Literal.ToString()!);

        var body = ParseBlock(true);
        if (body.Any(x => x is not (VarStatement or FunctionStatement)))
        {
            throw new Exception("Object can only contain variables & functions");
        }
        
        var bodyVars = body
            .Where(x => x is VarStatement)
            .Cast<VarStatement>()
            .ToList();
        var fields = new Dictionary<string, VarStatement>();
        foreach (var bodyVar in bodyVars)
        {
            fields.Add(bodyVar.Name, bodyVar);
        }
        
        var bodyFuncs = body
            .Where(x => x is FunctionStatement)
            .Cast<FunctionStatement>()
            .ToList();
        var functions = new Dictionary<string, FunctionStatement>();
        foreach (var bodyFunc in bodyFuncs)
        {
            functions.Add(bodyFunc.Name, bodyFunc);
        }
        
        return new ObjectStatement(name, fields, functions);
    }

    private List<Statement> ParseBlock(bool nested = false)
    {
        Eat(TokenKind.LeftCurlyBracket);
        var block = new List<Statement>();
        while (_current.Kind != TokenKind.RightCurlyBracket)
        {
            var statement = ParseStatement();
            if (statement != null)
            {
                block.Add(statement);
            }

            if (_current.Kind == TokenKind.Semicolon)
            {
                Eat(TokenKind.Semicolon);
            }

            if (_current.Kind == TokenKind.RightCurlyBracket && nested)
            {
                Eat(TokenKind.RightCurlyBracket);
            }
        }
        // Only expect the closing '}', don't eat it.
        // Leave eating up to the discretion of the caller
        Expect(TokenKind.RightCurlyBracket);
        return block;
    }

    private Expr ParseExpr(Token? eaten = null)
    {
        Expr left = ParseSimpleExpr()
            ?? ParseClosureExpr()
            ?? ParsePrefixExpr()
            ?? ParseListExpr()
            ?? throw new Exception($"Unknown token: {_current}");

        while (_current.Kind != TokenKind.EndOfFile && _current.Kind != TokenKind.Semicolon)
        {
            var postfix = ParsePostfixExpr(left);
            var infix = ParseInfixExpr(postfix ?? left);

            if (postfix != null && infix == null)
            {
                left = postfix;
            }
            else if (infix != null)
            {
                left = infix;
            }
            else
            {
                break;
            }
        }
        
        return left;
    }

    private Expr? ParseSimpleExpr()
    {
        return _current.Kind switch
        {
            TokenKind.StringLiteral => new StringExpr(Eat(TokenKind.StringLiteral).Literal.ToString()!),
            TokenKind.Null => new NullExpr(Eat(TokenKind.Null).Literal.ToString()!),
            TokenKind.NumericLiteral => new NumericExpr(double.Parse(Eat(TokenKind.NumericLiteral).Literal.ToString()!)),
            TokenKind.True => new BoolExpr(bool.Parse(Eat(TokenKind.True).Literal.ToString()!)),
            TokenKind.False => new BoolExpr(bool.Parse(Eat(TokenKind.False).Literal.ToString()!)),
            TokenKind.Identifier => new IdentifierExpr(Eat(TokenKind.Identifier).Literal.ToString()!),
            _ => null
        };
    }

    private ClosureExpr? ParseClosureExpr()
    {
        if (_current.Kind != TokenKind.Function) return null;
        
        var function = ParseFunction(false);
        return new ClosureExpr(function.Parameters, function.Body);
    }

    private Expr? ParsePrefixExpr()
    {
        return _current.Kind switch
        {
            TokenKind.Minus => new PrefixExpr(Op.Minus, ParseExpr(Eat(TokenKind.Minus))),
            TokenKind.Not => new PrefixExpr(Op.Not, ParseExpr(Eat(TokenKind.Not))),
            _ => null
        };
    }

    private ListExpr? ParseListExpr()
    {
        if (_current.Kind != TokenKind.LeftSquareBracket) return null;
        
        Eat(TokenKind.LeftSquareBracket);
        var items = new List<Expr>();

        while (_current.Kind != TokenKind.RightSquareBracket)
        {
            // Should end up in ParsePostfixExpr & return a ListIndexExpr(...)
            items.Add(ParseExpr());
            if (_current.Kind == TokenKind.Comma)
            {
                Eat(TokenKind.Comma);
            }
        }
        
        Eat(TokenKind.RightSquareBracket);
        
        return new ListExpr(items);
    }

    private Expr? ParsePostfixExpr(Expr left)
    {
        switch (_current.Kind)
        {
            case TokenKind.Increment:
                return new PostfixExpr(left, Eat(TokenKind.Increment).Kind.ToOp());
            
            case TokenKind.Decrement:
                return new PostfixExpr(left, Eat(TokenKind.Decrement).Kind.ToOp());
            
            case TokenKind.Dot:
                Eat(TokenKind.Dot);
                return new GetExpr(left, Eat(TokenKind.Identifier).Literal.ToString()!);
            
            case TokenKind.LeftSquareBracket:
                Eat(TokenKind.LeftSquareBracket);
                var index = _current.Kind == TokenKind.RightSquareBracket 
                    ? null
                    : ParseExpr();
                Eat(TokenKind.RightSquareBracket);
                return new ListIndexExpr((IdentifierExpr)left, index);
            
            case TokenKind.LeftRoundBracket:
                Eat(TokenKind.LeftRoundBracket);
                var args = new List<Expr>();
                while (_current.Kind != TokenKind.RightRoundBracket)
                {
                    args.Add(ParseExpr());
                    if (_current.Kind == TokenKind.Comma)
                    {
                        Eat(TokenKind.Comma);
                    }
                }
                Eat(TokenKind.RightRoundBracket);
                return new CallExpr(left, args);
            
            default:
                return null;
        }
    }

    private Expr? ParseInfixExpr(Expr left)
    {
        switch (_current.Kind)
        {
            case TokenKind.Plus:
            case TokenKind.Minus:
            case TokenKind.Multiply:
            case TokenKind.Divide:
            case TokenKind.EqualTo:
            case TokenKind.NotEqualTo:
            case TokenKind.LessThanOrEqualTo:
            case TokenKind.LessThan:
            case TokenKind.GreaterThan:
            case TokenKind.GreaterThanOrEqualTo:
            case TokenKind.And:
            case TokenKind.Or:
                var token = _current;
                Next();
                return new InfixExpr(left, token.Kind.ToOp(), ParseExpr());

            case TokenKind.Assign:
                Next();
                return new AssignmentExpr(left, ParseExpr());

            default:
                return null;
        }
    }
}