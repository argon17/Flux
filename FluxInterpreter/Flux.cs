namespace FluxInterpreter;

public abstract class Flux
{
    private static readonly Interpreter Interpreter = new();
    private static bool _hadError = false;
    private static bool _hadRuntimeError = false;

    public static void Main(string[] args)
    {
        if (args.Length > 1)
        {
            Console.WriteLine("Usage: flux [filePath]");
            System.Environment.Exit(64);
        }
        else if (args.Length == 1)
        {
            RunFile(args[0]);
        }
        else
        {
            RunPrompt();
        }
    }

    private static void RunFile(string path)
    {
        string script = File.ReadAllText(path);
        Run(script);
        if (_hadError) System.Environment.Exit(65);
        if (_hadRuntimeError) System.Environment.Exit(70);
    }

    private static void RunPrompt()
    {
        while (true)
        {
            Console.Write("> ");
            string? line = Console.ReadLine();
            if (line == null) break;
            Run(line);
            _hadError = false;
        }
    }

    private static void Run(string source)
    {
        Scanner scanner = new Scanner(source);
        List<Token> tokens = scanner.ScanTokens();

        Parser parser = new Parser(tokens);
        
        List<Stmt> statements = parser.Parse();
        
        if (_hadError) return;
        Interpreter.Interpret(statements);
    }

    internal static void Error(int line, string message)
    {
        Report(line, "", message);
    }

    internal static void Error(Token token, string message)
    {
        if (token.Type == TokenType.Eof)
        {
            Report(token.Line, " at end", message);
        }
        else
        {
            Report(token.Line, $" at '{token.Lexeme}'", message);
        }
    }

    private static void Report(int line, string where, string message)
    {
        Console.WriteLine($"[line {line}] Error {where}: {message}");
        _hadError = true;
    }

    internal static void RuntimeError(RuntimeError error)
    {
        Console.WriteLine($"[line {error.Token.Line}] RuntimeError: {error.Message}");
        _hadRuntimeError = true;
    }
}