namespace FluxInterpreter;

public class Environment(Environment? enclosing)
{
    private readonly Dictionary<Token, object?> _values = new();

    internal object? Get(Token varToken)
    {
        Token? foundToken = _values.Keys.FirstOrDefault(token => token.Lexeme == varToken.Lexeme);
        if (foundToken != null)
            return _values[foundToken] ?? null;
        if (enclosing != null) return enclosing.Get(varToken);
        throw new RuntimeError(varToken, $"Undefined variable '{varToken}'.");
    }

    internal void Define(Token varToken, object? value)
    {
        Token? alreadyDefinedToken = _values.Keys.FirstOrDefault(token => token.Lexeme == varToken.Lexeme);
        if (alreadyDefinedToken != null)
            throw new RuntimeError(varToken,
                $"Variable '{varToken.Lexeme}' is already defined at line {alreadyDefinedToken.Line}.");
        _values[varToken] = value;
    }

    internal void Assign(Token varToken, object? value)
    {
        Token? foundToken = _values.Keys.FirstOrDefault(token => token.Lexeme == varToken.Lexeme);
        if (foundToken != null)
        {
            _values[foundToken] = value;
            return;
        }

        if (enclosing != null)
        {
            enclosing.Assign(varToken, value);
            return;
        }

        throw new RuntimeError(varToken, $"Undefined variable '{varToken.Lexeme}'.");
    }
}