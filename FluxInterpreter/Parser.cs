namespace FluxInterpreter;

public class Parser(List<Token> tokens)
{
    private readonly List<Token> _tokens = tokens;
    private int _current = 0;
    

    public List<Stmt> Parse()
    {
        List<Stmt> statements = new();
        while (!IsAtEnd())
        {
            statements.Add(ParseDeclaration());
        }
        return statements;
    }

    private Stmt ParseDeclaration()
    {
        try
        {
            if (Match(TokenType.Var)) return ParseVarDeclaration();
            return ParseStatement();
        }
        catch (ParseError)
        {
            Synchronize();
            return null;
        }
    }

    private Stmt ParseVarDeclaration()
    {
        Token name = Consume(TokenType.Identifier, "Expect variable name.");
        Expr initializer = null;
        if (Match(TokenType.Equal))
        {
            initializer = ParseExpression();
        }
        Consume(TokenType.Semicolon, "Expect ';' after variable declaration.");
        return new Stmt.Var(name, initializer);
    }

    private Stmt ParseStatement()
    {
        if (Match(TokenType.Print)) return ParsePrintStatement();
        return ParseExpressionStatement();
    }

    private Stmt ParseExpressionStatement()
    {
        Expr expr = ParseExpression();
        Consume(TokenType.Semicolon, "Expect ';' after expression.");
        return new Stmt.fcExpression(expr);
    }

    private Stmt ParsePrintStatement()
    {
        Expr value = ParseExpression();
        Consume(TokenType.Semicolon, "Expect ';' after value.");
        return new Stmt.Print(value);
    }

    private Expr ParseExpression()
    {
        return ParseEquality();
    }
    
    private Expr ParseEquality()
    {
        Expr expr = ParseComparison();

        while (Match(TokenType.NegEqual, TokenType.EqualEqual))
        {
            Token operatorToken = Previous();
            Expr right = ParseComparison();
            expr = new Expr.Binary(expr, operatorToken, right);
        }

        return expr;
    }
    
    private Expr ParseComparison()
    {
        Expr expr = ParseTerm();

        while (Match(TokenType.Greater, TokenType.GreaterEqual, TokenType.Less, TokenType.LessEqual))
        {
            Token operatorToken = Previous();
            Expr right = ParseTerm();
            expr = new Expr.Binary(expr, operatorToken, right);
        }

        return expr;
    }
    
    private Expr ParseTerm()
    {
        Expr expr = ParseFactor();

        while (Match(TokenType.Minus, TokenType.Plus))
        {
            Token operatorToken = Previous();
            Expr right = ParseFactor();
            expr = new Expr.Binary(expr, operatorToken, right);
        }

        return expr;
    }
    
    private Expr ParseFactor()
    {
        Expr expr = ParseUnary();

        while (Match(TokenType.Slash, TokenType.Star))
        {
            Token operatorToken = Previous();
            Expr right = ParseUnary();
            expr = new Expr.Binary(expr, operatorToken, right);
        }

        return expr;
    }
    
    private Expr ParseUnary()
    {
        if (Match(TokenType.Neg, TokenType.Minus))
        {
            Token operatorToken = Previous();
            Expr right = ParseUnary();
            return new Expr.Unary(operatorToken, right);
        }

        return ParsePrimary();
    }
    
    private Expr ParsePrimary()
    {
        if (Match(TokenType.False)) return new Expr.Literal(false);
        if (Match(TokenType.True)) return new Expr.Literal(true);
        if (Match(TokenType.Nil)) return new Expr.Literal(null!);

        if (Match(TokenType.Number, TokenType.String))
        {
            return new Expr.Literal(Previous().Literal!);
        }

        if (Match(TokenType.Identifier))
        {
            return new Expr.Variable(Previous());
        }

        if (Match(TokenType.LeftParen))
        {
            Expr expr = ParseExpression();
            Consume(TokenType.RightParen, "Expect ')' after expression.");
            return new Expr.Grouping(expr);
        }

        throw Error(Peek(), "Expect expression.");
    }
    
    private bool Match(params TokenType[] types)
    {
        foreach (TokenType type in types)
        {
            if (Check(type))
            {
                Advance();
                return true;
            }
        }

        return false;
    }
    
    private Token Consume(TokenType type, string message)
    {
        if (Check(type)) return Advance();

        throw Error(Peek(), message);
    }
    
    private bool Check(TokenType type)
    {
        if (IsAtEnd()) return false;
        return Peek().Type == type;
    }
    
    private Token Advance()
    {
        if (!IsAtEnd()) _current++;
        return Previous();
    }
    
    private bool IsAtEnd()
    {
        return Peek().Type == TokenType.Eof;
    }
    
    private Token Peek()
    {
        return _tokens[_current];
    }
    
    private Token Previous()
    {
        return _tokens[_current - 1];
    }
    
    private ParseError Error(Token token, string message)
    {
        Flux.Error(token, message);
        return new ParseError();
    }
    
    private void Synchronize()
    {
        Advance();

        while (!IsAtEnd())
        {
            if (Previous().Type == TokenType.Semicolon) return;

            switch (Peek().Type)
            {
                case TokenType.Class:
                case TokenType.Fun:
                case TokenType.Var:
                case TokenType.For:
                case TokenType.If:
                case TokenType.While:
                case TokenType.Print:
                case TokenType.Return:
                    return;
            }

            Advance();
        }
    }
}

public class ParseError : Exception
{
}