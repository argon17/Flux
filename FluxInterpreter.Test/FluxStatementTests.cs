namespace FluxInterpreter.Test;

public class FluxStatementTests
{
    [Fact]
    public void TestPrintStatement_WithNumber()
    {
        // Arrange: Test "print 42;"
        string output = FluxTestHelpers.RunFluxCodeAndCaptureOutput("print 42;");

        // Assert
        Assert.Equal("42", output.Trim());
    }

    [Fact]
    public void TestPrintStatement_WithString()
    {
        // Arrange: Test "print \"Hello, Flux!\";"
        string output = FluxTestHelpers.RunFluxCodeAndCaptureOutput("print \"Hello, Flux!\";");

        // Assert
        Assert.Equal("Hello, Flux!", output.Trim());
    }

    [Fact]
    public void TestPrintStatement_WithExpression()
    {
        // Arrange: Test "print 2 + 3 * 4;"
        string output = FluxTestHelpers.RunFluxCodeAndCaptureOutput("print 2 + 3 * 4;");

        // Assert
        Assert.Equal("14", output.Trim());
    }

    [Fact]
    public void TestPrintStatement_WithBooleanExpression()
    {
        // Arrange: Test "print 5 > 3;"
        string output = FluxTestHelpers.RunFluxCodeAndCaptureOutput("print 5 > 3;");

        // Assert
        Assert.Equal("True", output.Trim());
    }

    [Fact]
    public void TestMultipleStatements()
    {
        // Arrange: Test multiple statements
        string output = FluxTestHelpers.RunFluxCodeAndCaptureOutput(@"
            print ""First line"";
            print ""Second line"";
            print 3 * 7;
        ");

        // Assert
        string[] lines = output.Trim().Split('\n', StringSplitOptions.RemoveEmptyEntries);
        Assert.Equal(3, lines.Length);
        Assert.Equal("First line", lines[0].Trim());
        Assert.Equal("Second line", lines[1].Trim());
        Assert.Equal("21", lines[2].Trim());
    }

    [Fact]
    public void TestPrintStatement_WithNil()
    {
        // Arrange: Test "print nil;"
        string output = FluxTestHelpers.RunFluxCodeAndCaptureOutput("print nil;");

        // Assert
        Assert.Equal("nil", output.Trim());
    }

    [Fact]
    public void TestPrintStatement_WithGrouping()
    {
        // Arrange: Test "print (2 + 3) * 4;"
        string output = FluxTestHelpers.RunFluxCodeAndCaptureOutput("print (2 + 3) * 4;");

        // Assert
        Assert.Equal("20", output.Trim());
    }

    [Fact]
    public void TestPrintStatement_WithUnaryExpression()
    {
        // Arrange: Test "print -42;"
        string output = FluxTestHelpers.RunFluxCodeAndCaptureOutput("print -42;");

        // Assert
        Assert.Equal("-42", output.Trim());
    }

    [Fact]
    public void TestPrintStatement_WithLogicalNegation()
    {
        // Arrange: Test "print !true;"
        string output = FluxTestHelpers.RunFluxCodeAndCaptureOutput("print !true;");

        // Assert
        Assert.Equal("False", output.Trim());
    }
}