namespace FluxInterpreter;

public class Environment
{
    private readonly Dictionary<Token, object> _values = new();

    internal object Get(Token varToken)
    {
        Token? foundToken = _values.Keys.FirstOrDefault(token => token.Lexeme == varToken.Lexeme);
        if (foundToken != null)
            return _values[foundToken];
        throw new RuntimeError(varToken, $"Undefined variable '{varToken}'.");
    }

    internal void Define(Token varToken, object value)
    {
        Token? alreadyDefinedToken = _values.Keys.FirstOrDefault(token => token.Lexeme == varToken.Lexeme);
        if (alreadyDefinedToken != null)
            throw new RuntimeError(varToken, $"Variable '{varToken.Lexeme}' is already defined at line {alreadyDefinedToken.Line}.");
        _values[varToken] = value;
    }
}