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
            Stmt? declaration = ParseDeclaration();
            if (declaration != null)
            {
                statements.Add(declaration);
            }
        }

        return statements;
    }

    private Stmt? ParseDeclaration()
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
        Expr? initializer = null;
        if (Match(TokenType.Equal))
        {
            initializer = ParseExpression();
        }

        Consume(TokenType.Semicolon, "Expect ';' after variable declaration.");
        return new Stmt.Var(name, initializer);
    }

    private Stmt ParseStatement()
    {
        if (Match(TokenType.For)) return ParseForStatement();
        if (Match(TokenType.If)) return ParseIfStatement();
        if (Match(TokenType.Print)) return ParsePrintStatement();
        if (Match(TokenType.While)) return ParseWhileStatement();
        if (Match(TokenType.LeftBrace)) return new Stmt.Block(ParseBlock());
        return ParseExpressionStatement();
    }

    private Stmt ParseForStatement()
    {
        Consume(TokenType.LeftParen, "Expect '(' after 'for'.");

        Stmt? initializer;
        if (Match(TokenType.Semicolon))
        {
            initializer = null;
        }
        else if (Match(TokenType.Var))
        {
            initializer = ParseVarDeclaration();
        }
        else
        {
            initializer = ParseExpressionStatement();
        }

        Expr? condition = null;
        if (!Check(TokenType.Semicolon))
        {
            condition = ParseExpression();
        }

        Consume(TokenType.Semicolon, "Expect ';' after loop condition.");

        Expr? increment = null;
        if (!Check(TokenType.RightParen))
        {
            increment = ParseExpression();
        }

        Consume(TokenType.RightParen, "Expect ')' after for clauses.");

        Stmt body = ParseStatement();
        if (increment != null)
        {
            body = new Stmt.Block([
                body,
                new Stmt.fcExpression(increment)
            ]);
        }

        condition ??= new Expr.Literal(true);
        body = new Stmt.While(condition, body);

        if (initializer != null)
        {
            body = new Stmt.Block([initializer, body]);
        }

        return body;
    }

    private Stmt ParseWhileStatement()
    {
        Consume(TokenType.LeftParen, "Expect '(' after 'while'.");
        Expr condition = ParseExpression();
        Consume(TokenType.RightParen, "Expect ')' after condition.");
        Stmt body = ParseStatement();

        return new Stmt.While(condition, body);
    }

    private Stmt ParseIfStatement()
    {
        Consume(TokenType.LeftParen, "Expect '(' after 'if'.");
        Expr condition = ParseExpression();
        Consume(TokenType.RightParen, "Expect ')' after if condition.");

        Stmt thenBranch = ParseStatement();
        Stmt? elseBranch = null;
        if (Match(TokenType.Else))
        {
            elseBranch = ParseStatement();
        }

        return new Stmt.If(condition, thenBranch, elseBranch);
    }

    private List<Stmt> ParseBlock()
    {
        List<Stmt> statements = new();
        while (!Check(TokenType.RightBrace) && !IsAtEnd())
        {
            Stmt? stmt = ParseDeclaration();
            if (stmt != null)
            {
                statements.Add(stmt);
            }
        }

        Consume(TokenType.RightBrace, "Expect '}' after block.");
        return statements;
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
        return ParseAssignment();
    }

    private Expr ParseAssignment()
    {
        Expr expr = ParseOr();
        if (Match(TokenType.Equal))
        {
            Token equals = Previous();
            Expr value = ParseAssignment();

            if (expr is Expr.Variable variable)
            {
                Token name = variable.Name;
                return new Expr.Assign(name, value);
            }

            Error(equals, "Invalid assignment target.");
        }

        return expr;
    }

    private Expr ParseOr()
    {
        Expr expr = ParseAnd();

        while (Match(TokenType.Or))
        {
            Token operatorToken = Previous();
            Expr right = ParseAnd();
            expr = new Expr.Logical(expr, operatorToken, right);
        }

        return expr;
    }

    private Expr ParseAnd()
    {
        Expr expr = ParseEquality();

        while (Match(TokenType.And))
        {
            Token operatorToken = Previous();
            Expr right = ParseEquality();
            expr = new Expr.Logical(expr, operatorToken, right);
        }

        return expr;
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