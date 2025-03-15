namespace Rhexia.v1;

public class Parser
{
    private List<Lexer.Token> _tokens = [];
    private Lexer.Token At() => _tokens[0];
    private Lexer.Token Eat(Lexer.TokenType? expectedToken = null)
    {
        if (expectedToken != null && At().Type != expectedToken) throw new Exception($"Expected token {expectedToken} but got {At().Type}!");
        
        Lexer.Token previous = At();
        _tokens.RemoveAt(0);
        
        return previous;
    }

    private bool NotEOF => At().Type != Lexer.TokenType.EOF;
    
    public Ast.Program Parse(string code)
    {
        _tokens = Lexer.Tokenize(code);
        var body = new List<Ast.Statement>();

        while (NotEOF)
        {
            if (At().Type == Lexer.TokenType.Semicolon)
            {
                Eat(Lexer.TokenType.Semicolon); // TODO :: Find out where this fits.
            }
            else
            {
                body.Add(ParseStatement());
            }
        }
        
        return new Ast.Program(body);
    }

    private Ast.Statement ParseStatement()
    {
        return At().Type switch
        {
            Lexer.TokenType.Var or Lexer.TokenType.Val => ParseDeclaration(),
            _ => ParseExpression()
        };
    }

    /// <summary>
    /// Handles variable declarations.
    /// var = changeable variable
    /// val = constant variable
    /// </summary>
    /// <returns></returns>
    /// <exception cref="Exception"></exception>
    private Ast.Statement ParseDeclaration()
    {
        var isConst = Eat().Type == Lexer.TokenType.Val;
        var identifier = Eat(Lexer.TokenType.Identifier).Value;

        if (At().Type == Lexer.TokenType.Semicolon)
        {
            Eat();
            if (isConst) throw new Exception("A val must have a constant value!");
            
            return new Ast.Variable(isConst, identifier);
        }

        Eat(Lexer.TokenType.Equals);
        var variable = new Ast.Variable(isConst, identifier, ParseExpression());
        Eat(Lexer.TokenType.Semicolon);
        
        return variable;
    }

    /// <summary>
    /// Handles non-grouping expressions
    /// </summary>
    /// <returns></returns>
    private Ast.Expression ParseExpression()
    {
        return ParseAssignment();
    }

    /// <summary>
    /// Handles variable assignment expressions
    /// </summary>
    /// <returns></returns>
    private Ast.Expression ParseAssignment()
    {
        var left = ParseObject();

        if (At().Type == Lexer.TokenType.Equals)
        {
            Eat(Lexer.TokenType.Equals);
            return new Ast.AssignmentExpression(left, ParseAssignment());
        }

        return left;
    }

    /// <summary>
    /// Handles object expressions
    /// </summary>
    /// <returns></returns>
    private Ast.Expression ParseObject()
    {
        if (At().Type != Lexer.TokenType.OpenCurlyBracket)
        {
            return ParseAdditive();
        }

        Eat(Lexer.TokenType.OpenCurlyBracket);
        var properties = new List<Ast.PropertyLiteral>();
        while (NotEOF && At().Type != Lexer.TokenType.CloseCurleyBracket)
        {
            var key = Eat(Lexer.TokenType.Identifier).Value;

            if (At().Type == Lexer.TokenType.Comma)
            {
                Eat(Lexer.TokenType.Comma);
                properties.Add(new Ast.PropertyLiteral(key));
                continue;
            }

            if (At().Type == Lexer.TokenType.CloseCurleyBracket)
            {
                properties.Add(new Ast.PropertyLiteral(key));
                continue;
            }

            Eat(Lexer.TokenType.Colon);
            properties.Add(new Ast.PropertyLiteral(key, ParseExpression()));

            if (At().Type != Lexer.TokenType.CloseCurleyBracket)
            {
                Eat(Lexer.TokenType.Comma);
            }
        }
        Eat(Lexer.TokenType.CloseCurleyBracket);
        
        return new Ast.ObjectLiteral(properties);
    }
    
    /// <summary>
    /// Handles Addition & Subtraction operators
    /// </summary>
    /// <returns></returns>
    private Ast.Expression ParseAdditive()
    {
        var left = ParseMultiplicative();

        while (At().Value == "+" || At().Value == "-")
        {
            var op = Eat().Value;
            var right = ParseMultiplicative();
            left = new Ast.BinaryExpression(left, right, op);
        }

        return left;
    }

    /// <summary>
    /// Handles Multiplication, Division & Modulus operators
    /// </summary>
    /// <returns></returns>
    private Ast.Expression ParseMultiplicative()
    {
        var left = ParseSecondary();

        while (At().Value == "*" || At().Value == "/" || At().Value == "%")
        {
            var op = Eat().Value;
            var right = ParseSecondary();
            left = new Ast.BinaryExpression(left, right, op);
        }

        return left;
    }

    private Ast.Expression ParseSecondary()
    {
        var member = ParseMember();

        if (At().Type == Lexer.TokenType.OpenRoundBracket)
        {
            return ParseCall(member);
        }

        return member;
    }

    private Ast.CallExpression ParseCall(Ast.Expression call)
    {
        var callExpr = new Ast.CallExpression(ParseArgs(), call);

        if (At().Type == Lexer.TokenType.OpenRoundBracket)
        {
            callExpr = ParseCall(callExpr);
        }

        return callExpr;
    }

    private List<Ast.Expression> ParseArgs()
    {
        Eat(Lexer.TokenType.OpenRoundBracket);
        var args = new List<Ast.Expression>();

        if (At().Type != Lexer.TokenType.CloseRoundBracket)
        {
            do
            {
                args.Add(ParseAssignment());
            } while (At().Type == Lexer.TokenType.Comma);
        }
        
        Eat(Lexer.TokenType.CloseRoundBracket);
        return args;
    }

    private Ast.Expression ParseMember()
    {
        var obj = ParsePrimary();

        while (At().Type == Lexer.TokenType.Dot || At().Type == Lexer.TokenType.OpenSquareBracket)
        {
            var op = Eat();
            Ast.Expression prop;
            bool computed;

            if (op.Type == Lexer.TokenType.Dot)
            {
                computed = false;
                prop = ParsePrimary();
                if (prop.Kind != Ast.NodeType.IdentifierLiteral) throw new Exception("Expected .property to be an identifier");
            }
            else
            {
                computed = true;
                prop = ParseExpression();
                Eat(Lexer.TokenType.CloseSquareBracket);
            }

            obj = new Ast.MemberExpression(obj, prop, computed);
        }

        return obj;
    }

    /// <summary>
    /// Handles literal values & grouping expressions
    /// </summary>
    /// <returns></returns>
    /// <exception cref="ArgumentOutOfRangeException"></exception>
    private Ast.Expression ParsePrimary()
    {
        return At().Type switch
        {
            Lexer.TokenType.Number => new Ast.NumericLiteral(int.Parse(Eat().Value)),
            Lexer.TokenType.Bool => new Ast.BoolLiteral(bool.Parse(Eat().Value)),
            Lexer.TokenType.Identifier => new Ast.IdentifierLiteral(Eat().Value),
            Lexer.TokenType.OpenRoundBracket => ParseParenthesis(),
            Lexer.TokenType.Null => ParseNull(),
            
            _ => throw new ArgumentOutOfRangeException($"Unexpected token found during parsing: [{At().Type}]")
        };
    }

    /// <summary>
    /// Handles grouping expressions, e.g. wrapped in ( ... )
    /// </summary>
    /// <returns></returns>
    private Ast.Expression ParseParenthesis()
    {
        Eat(Lexer.TokenType.OpenRoundBracket);
        Ast.Expression value = ParseExpression();
        Eat(Lexer.TokenType.CloseRoundBracket);
        return value;
    }

    /// <summary>
    /// Handles null literals
    /// </summary>
    /// <returns></returns>
    private Ast.NullLiteral ParseNull()
    {
        Eat(Lexer.TokenType.Null);
        return new Ast.NullLiteral();
    }
}