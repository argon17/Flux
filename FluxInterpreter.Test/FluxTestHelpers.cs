namespace FluxInterpreter.Test;

/// <summary>
/// Helper utilities for testing Flux interpreter functionality
/// </summary>
public static class FluxTestHelpers
{
    /// <summary>
    /// Helper method to run Flux source code through the interpreter
    /// </summary>
    /// <param name="source">The Flux source code to execute</param>
    private static void RunFluxCode(string source)
    {
        Scanner scanner = new Scanner(source);
        List<Token> tokens = scanner.ScanTokens();
        
        Parser parser = new Parser(tokens);
        List<Stmt> statements = parser.Parse();
        
        Interpreter interpreter = new Interpreter();
        interpreter.Interpret(statements);
    }

    /// <summary>
    /// Helper method to capture console output for testing
    /// </summary>
    /// <param name="action">The action to execute while capturing output</param>
    /// <returns>The captured console output as a string</returns>
    private static string CaptureOutput(Action action)
    {
        // Use lock to prevent concurrent access to Console.Out
        lock (typeof(Console))
        {
            var originalOut = Console.Out;
            try
            {
                using var stringWriter = new StringWriter();
                Console.SetOut(stringWriter);
                action();
                return stringWriter.ToString();
            }
            finally
            {
                Console.SetOut(originalOut);
            }
        }
    }

    /// <summary>
    /// Convenience method that combines RunFluxCode with CaptureOutput
    /// </summary>
    /// <param name="source">The Flux source code to execute</param>
    /// <returns>The output produced by the Flux code</returns>
    public static string RunFluxCodeAndCaptureOutput(string source)
    {
        return CaptureOutput(() => RunFluxCode(source));
    }
}
