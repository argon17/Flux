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

    [Fact]
    public void TestAssignmentExpression_SimpleAssignment()
    {
        // Arrange & Act: Test simple variable assignment
        string output = FluxTestHelpers.RunFluxCodeAndCaptureOutput("""
            var x = 5;
            x = 10;
            print x;
        """);

        // Assert
        Assert.Equal("10", output.Trim());
    }

    [Fact]
    public void TestAssignmentExpression_AssignmentInExpression()
    {
        // Arrange & Act: Test assignment within an expression
        string output = FluxTestHelpers.RunFluxCodeAndCaptureOutput("""
            var x = 5;
            var y = 10;
            print x = y;
            print x;
        """);

        // Assert
        string[] lines = output.Trim().Split('\n', StringSplitOptions.RemoveEmptyEntries);
        Assert.Equal(2, lines.Length);
        Assert.Equal("10", lines[0].Trim()); // Assignment returns the assigned value
        Assert.Equal("10", lines[1].Trim()); // x now contains the assigned value
    }

    [Fact]
    public void TestAssignmentExpression_ChainedAssignment()
    {
        // Arrange & Act: Test chained assignment
        string output = FluxTestHelpers.RunFluxCodeAndCaptureOutput("""
            var x;
            var y;
            var z;
            z = y = x = 42;
            print x;
            print y;
            print z;
        """);

        // Assert
        string[] lines = output.Trim().Split('\n', StringSplitOptions.RemoveEmptyEntries);
        Assert.Equal(3, lines.Length);
        Assert.Equal("42", lines[0].Trim());
        Assert.Equal("42", lines[1].Trim());
        Assert.Equal("42", lines[2].Trim());
    }

    [Fact]
    public void TestLogicalExpression_AndOperator_TrueTrue()
    {
        // Arrange & Act: Test logical AND with both true
        string output = FluxTestHelpers.RunFluxCodeAndCaptureOutput("""
            print true and true;
        """);

        // Assert
        Assert.Equal("True", output.Trim());
    }

    [Fact]
    public void TestLogicalExpression_AndOperator_TrueFalse()
    {
        // Arrange & Act: Test logical AND with true and false
        string output = FluxTestHelpers.RunFluxCodeAndCaptureOutput("""
            print true and false;
        """);

        // Assert
        Assert.Equal("False", output.Trim());
    }

    [Fact]
    public void TestLogicalExpression_AndOperator_FalseTrue()
    {
        // Arrange & Act: Test logical AND with false and true (short-circuit)
        string output = FluxTestHelpers.RunFluxCodeAndCaptureOutput("""
            print false and true;
        """);

        // Assert
        Assert.Equal("False", output.Trim());
    }

    [Fact]
    public void TestLogicalExpression_OrOperator_TrueTrue()
    {
        // Arrange & Act: Test logical OR with both true
        string output = FluxTestHelpers.RunFluxCodeAndCaptureOutput("""
            print true or true;
        """);

        // Assert
        Assert.Equal("True", output.Trim());
    }

    [Fact]
    public void TestLogicalExpression_OrOperator_TrueFalse()
    {
        // Arrange & Act: Test logical OR with true and false (short-circuit)
        string output = FluxTestHelpers.RunFluxCodeAndCaptureOutput("""
            print true or false;
        """);

        // Assert
        Assert.Equal("True", output.Trim());
    }

    [Fact]
    public void TestLogicalExpression_OrOperator_FalseFalse()
    {
        // Arrange & Act: Test logical OR with both false
        string output = FluxTestHelpers.RunFluxCodeAndCaptureOutput("""
            print false or false;
        """);

        // Assert
        Assert.Equal("False", output.Trim());
    }

    [Fact]
    public void TestLogicalExpression_ShortCircuitAnd()
    {
        // Arrange & Act: Test short-circuit behavior of AND
        string output = FluxTestHelpers.RunFluxCodeAndCaptureOutput("""
            var x = 5;
            false and (x = 10);
            print x;
        """);

        // Assert
        Assert.Equal("5", output.Trim()); // x should not be modified due to short-circuit
    }

    [Fact]
    public void TestLogicalExpression_ShortCircuitOr()
    {
        // Arrange & Act: Test short-circuit behavior of OR
        string output = FluxTestHelpers.RunFluxCodeAndCaptureOutput("""
            var x = 5;
            true or (x = 10);
            print x;
        """);

        // Assert
        Assert.Equal("5", output.Trim()); // x should not be modified due to short-circuit
    }

    [Fact]
    public void TestLogicalExpression_ComplexLogical()
    {
        // Arrange & Act: Test complex logical expressions
        string output = FluxTestHelpers.RunFluxCodeAndCaptureOutput("""
            var a = true;
            var b = false;
            var c = true;
            print a and b or c;
            print a or b and c;
        """);

        // Assert
        string[] lines = output.Trim().Split('\n', StringSplitOptions.RemoveEmptyEntries);
        Assert.Equal(2, lines.Length);
        Assert.Equal("True", lines[0].Trim());  // (true and false) or true = true
        Assert.Equal("True", lines[1].Trim());  // true or (false and true) = true
    }

    [Fact]
    public void TestIfStatement_ComplexConditions()
    {
        // Arrange & Act: Test if statement with complex logical conditions
        string output = FluxTestHelpers.RunFluxCodeAndCaptureOutput("""
            var x = 10;
            var y = 5;
            if (x > y and y > 0) print "both conditions true";
        """);

        // Assert
        Assert.Equal("both conditions true", output.Trim());
    }

    [Fact]
    public void TestIfStatement_WithAssignmentInCondition()
    {
        // Arrange & Act: Test if statement with assignment in condition
        string output = FluxTestHelpers.RunFluxCodeAndCaptureOutput("""
            var x = 0;
            if (x = 5) print "assignment worked";
            print x;
        """);

        // Assert
        string[] lines = output.Trim().Split('\n', StringSplitOptions.RemoveEmptyEntries);
        Assert.Equal(2, lines.Length);
        Assert.Equal("assignment worked", lines[0].Trim());
        Assert.Equal("5", lines[1].Trim());
    }

    [Fact]
    public void TestAssignmentExpression_InNestedScopes()
    {
        // Arrange & Act: Test assignment in nested scopes
        string output = FluxTestHelpers.RunFluxCodeAndCaptureOutput("""
            var x = "global";
            {
                x = "outer";
                {
                    x = "inner";
                    print x;
                }
                print x;
            }
            print x;
        """);

        // Assert
        string[] lines = output.Trim().Split('\n', StringSplitOptions.RemoveEmptyEntries);
        Assert.Equal(3, lines.Length);
        Assert.Equal("inner", lines[0].Trim());
        Assert.Equal("inner", lines[1].Trim());
        Assert.Equal("inner", lines[2].Trim());
    }

    [Fact]
    public void TestLogicalExpression_WithVariables()
    {
        // Arrange & Act: Test logical expressions with variables
        string output = FluxTestHelpers.RunFluxCodeAndCaptureOutput("""
            var isTrue = true;
            var isFalse = false;
            print isTrue and isFalse;
            print isTrue or isFalse;
        """);

        // Assert
        string[] lines = output.Trim().Split('\n', StringSplitOptions.RemoveEmptyEntries);
        Assert.Equal(2, lines.Length);
        Assert.Equal("False", lines[0].Trim());
        Assert.Equal("True", lines[1].Trim());
    }

    [Fact]
    public void TestIfElseStatement_WithLogicalConditions()
    {
        // Arrange & Act: Test if-else with logical conditions
        string output = FluxTestHelpers.RunFluxCodeAndCaptureOutput("""
            var age = 20;
            var hasLicense = true;
            if (age >= 18 and hasLicense) {
                print "Can drive";
            } else {
                print "Cannot drive";
            }
        """);

        // Assert
        Assert.Equal("Can drive", output.Trim());
    }

    [Fact]
    public void TestOrderOfOperations_LogicalAndComparison()
    {
        // Arrange & Act: Test order of operations with logical and comparison operators
        string output = FluxTestHelpers.RunFluxCodeAndCaptureOutput("""
            var x = 5;
            var y = 10;
            var z = 15;
            print x < y and y < z;
            print x > y or y < z;
        """);

        // Assert
        string[] lines = output.Trim().Split('\n', StringSplitOptions.RemoveEmptyEntries);
        Assert.Equal(2, lines.Length);
        Assert.Equal("True", lines[0].Trim());   // 5 < 10 and 10 < 15 = true
        Assert.Equal("True", lines[1].Trim());   // 5 > 10 or 10 < 15 = true
    }
}