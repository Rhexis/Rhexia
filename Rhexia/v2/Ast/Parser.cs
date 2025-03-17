using Rhexia.v2.Ast.Expressions;
using Rhexia.v2.Ast.Statements;
using Rhexia.v2.Lex;

namespace Rhexia.v2.Ast;

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
        
        var block = ParseBlock();
        Expect(TokenKind.RightCurlyBracket);
        
        return new ForStatement(init, condition, increment, block);
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
            Expect(TokenKind.RightCurlyBracket);
            return new IfElseStatement(condition, then);
        }

        Eat(TokenKind.RightCurlyBracket);
        Eat(TokenKind.Else);
        var block = ParseBlock();
        Expect(TokenKind.RightCurlyBracket);
        
        return new IfElseStatement(condition, then, block);
    }

    private ReturnStatement ParseReturn()
    {
        Eat(TokenKind.Return);
        // TODO :: Implement
        return null;//new ReturnStatement();
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
        return new FunctionStatement(name, parameters, ParseBlock());
    }

    private WhileStatement ParseWhile()
    {
        Eat(TokenKind.While);
        // TODO :: Implement
        return null;
    }

    private List<Statement> ParseBlock()
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
        }
        
        Expect(TokenKind.RightCurlyBracket);
        return block;
    }

    private Expr ParseExpr(Token? eaten = null)
    {
        Expr left = ParseSimpleExpr()
            ?? ParseClosure()
            ?? ParsePrefixExpr()
            ?? ParseListExpr()
            ?? throw new Exception($"Unknown token: {_current}");

        while (_current.Kind != TokenKind.EndOfFile && _current.Kind != TokenKind.Semicolon)
        {
            var postfix = ParsePostfixExpr(left);
            var infix = ParseInfixExpr(left);

            if (postfix != null)
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

    private ClosureExpr? ParseClosure()
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
                // Might be double eating the square brackets? 
                // TODO :: Need to confirm.
                Eat(TokenKind.LeftSquareBracket);
                var index = _current.Kind == TokenKind.RightSquareBracket 
                    ? null
                    : ParseExpr();
                Eat(TokenKind.RightSquareBracket);
                return new ListIndexExpr(left, index);
            
            case TokenKind.LeftCurlyBracket:
                Eat(TokenKind.LeftCurlyBracket);
                var fields = new Dictionary<string, Expr>();
                while (_current.Kind != TokenKind.RightCurlyBracket)
                {
                    var key = Eat().ToString();
                    Expr value;
                    if (_current.Kind == TokenKind.Colon)
                    {
                        Eat(TokenKind.Colon);
                        value = ParseExpr();
                    }
                    else
                    {
                        value = new IdentifierExpr(key);
                    }
                    fields.Add(key, value);
                    if (_current.Kind == TokenKind.Comma)
                    {
                        Eat(TokenKind.Comma);
                    }
                }
                Eat(TokenKind.RightCurlyBracket);
                return new StructExpr(left, fields);
            
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