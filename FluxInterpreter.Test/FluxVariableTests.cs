using FluxInterpreter;

namespace FluxInterpreter.Test;

public class FluxVariableTests
{
    [Fact]
    public void TestVariableDeclaration_WithInitializer()
    {
        // Arrange & Act: Test "var x = 42; print x;"
        string output = FluxTestHelpers.RunFluxCodeAndCaptureOutput(@"
            var x = 42;
            print x;
        ");

        // Assert
        Assert.Equal("42", output.Trim());
    }

    [Fact]
    public void TestVariableDeclaration_WithoutInitializer()
    {
        // Arrange & Act: Test "var x; print x;" (should print nil)
        string output = FluxTestHelpers.RunFluxCodeAndCaptureOutput(@"
            var x;
            print x;
        ");

        // Assert
        Assert.Equal("nil", output.Trim());
    }

    [Fact]
    public void TestVariableDeclaration_WithStringValue()
    {
        // Arrange & Act: Test variable with string value
        string output = FluxTestHelpers.RunFluxCodeAndCaptureOutput(@"
            var message = ""Hello, Flux!"";
            print message;
        ");

        // Assert
        Assert.Equal("Hello, Flux!", output.Trim());
    }

    [Fact]
    public void TestVariableDeclaration_WithExpressionInitializer()
    {
        // Arrange & Act: Test variable initialized with expression
        string output = FluxTestHelpers.RunFluxCodeAndCaptureOutput(@"
            var result = 2 + 3 * 4;
            print result;
        ");

        // Assert
        Assert.Equal("14", output.Trim());
    }

    [Fact]
    public void TestVariableDeclaration_WithBooleanValue()
    {
        // Arrange & Act: Test variable with boolean value
        string output = FluxTestHelpers.RunFluxCodeAndCaptureOutput(@"
            var isTrue = true;
            var isFalse = false;
            print isTrue;
            print isFalse;
        ");

        // Assert
        string[] lines = output.Trim().Split('\n', StringSplitOptions.RemoveEmptyEntries);
        Assert.Equal(2, lines.Length);
        Assert.Equal("True", lines[0].Trim());
        Assert.Equal("False", lines[1].Trim());
    }

    [Fact]
    public void TestMultipleVariableDeclarations()
    {
        // Arrange & Act: Test multiple variable declarations
        string output = FluxTestHelpers.RunFluxCodeAndCaptureOutput(@"
            var a = 10;
            var b = 20;
            var c = 30;
            print a;
            print b;
            print c;
        ");

        // Assert
        string[] lines = output.Trim().Split('\n', StringSplitOptions.RemoveEmptyEntries);
        Assert.Equal(3, lines.Length);
        Assert.Equal("10", lines[0].Trim());
        Assert.Equal("20", lines[1].Trim());
        Assert.Equal("30", lines[2].Trim());
    }

    [Fact]
    public void TestVariableInExpression()
    {
        // Arrange & Act: Test using variables in expressions
        string output = FluxTestHelpers.RunFluxCodeAndCaptureOutput(@"
            var x = 5;
            var y = 3;
            print x + y;
            print x * y;
            print x - y;
        ");

        // Assert
        string[] lines = output.Trim().Split('\n', StringSplitOptions.RemoveEmptyEntries);
        Assert.Equal(3, lines.Length);
        Assert.Equal("8", lines[0].Trim());   // 5 + 3
        Assert.Equal("15", lines[1].Trim());  // 5 * 3
        Assert.Equal("2", lines[2].Trim());   // 5 - 3
    }

    [Fact]
    public void TestVariableWithComplexExpression()
    {
        // Arrange & Act: Test variable in complex expression
        string output = FluxTestHelpers.RunFluxCodeAndCaptureOutput(@"
            var a = 2;
            var b = 3;
            var c = 4;
            print (a + b) * c;
        ");

        // Assert
        Assert.Equal("20", output.Trim()); // (2 + 3) * 4 = 20
    }

    [Fact]
    public void TestVariableRedeclaration()
    {
        // Arrange & Act: Test variable redeclaration (should work)
        string output = FluxTestHelpers.RunFluxCodeAndCaptureOutput("""
            var x = 10;
            print x;
            var x = 20;
            print x;
        """);

        // Assert
        string[] lines = output.Trim().Split('\n', StringSplitOptions.RemoveEmptyEntries);
        Assert.Equal(2, lines.Length);
        Assert.Equal("10", lines[0].Trim());
        Assert.Equal("[line 3] RuntimeError: Variable 'x' is already defined at line 1.", lines[1].Trim());
    }

    [Fact]
    public void TestVariableWithNilValue()
    {
        // Arrange & Act: Test variable explicitly set to nil
        string output = FluxTestHelpers.RunFluxCodeAndCaptureOutput(@"
            var empty = nil;
            print empty;
        ");

        // Assert
        Assert.Equal("nil", output.Trim());
    }

    [Fact]
    public void TestVariableWithUnaryOperations()
    {
        // Arrange & Act: Test variables with unary operations
        string output = FluxTestHelpers.RunFluxCodeAndCaptureOutput(@"
            var positive = 42;
            var negative = -positive;
            var truth = true;
            var falsehood = !truth;
            print negative;
            print falsehood;
        ");

        // Assert
        string[] lines = output.Trim().Split('\n', StringSplitOptions.RemoveEmptyEntries);
        Assert.Equal(2, lines.Length);
        Assert.Equal("-42", lines[0].Trim());
        Assert.Equal("False", lines[1].Trim());
    }

    [Fact]
    public void TestVariableWithComparison()
    {
        // Arrange & Act: Test variables in comparison operations
        string output = FluxTestHelpers.RunFluxCodeAndCaptureOutput(@"
            var x = 10;
            var y = 5;
            print x > y;
            print x < y;
            print x == 10;
            print y != 10;
        ");

        // Assert
        string[] lines = output.Trim().Split('\n', StringSplitOptions.RemoveEmptyEntries);
        Assert.Equal(4, lines.Length);
        Assert.Equal("True", lines[0].Trim());   // 10 > 5
        Assert.Equal("False", lines[1].Trim());  // 10 < 5
        Assert.Equal("True", lines[2].Trim());   // 10 == 10
        Assert.Equal("True", lines[3].Trim());   // 5 != 10
    }

    [Fact]
    public void TestVariableWithStringConcatenation()
    {
        // Arrange & Act: Test string variables with concatenation
        string output = FluxTestHelpers.RunFluxCodeAndCaptureOutput(@"
            var first = ""Hello"";
            var second = ""World"";
            print first + "" "" + second;
        ");

        // Assert
        Assert.Equal("Hello World", output.Trim());
    }
}
