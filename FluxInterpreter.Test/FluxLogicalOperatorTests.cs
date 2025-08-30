using Xunit;

namespace FluxInterpreter.Test
{
    public class FluxLogicalOperatorTests
    {
        [Fact]
        public void TestLogicalAnd_ShortCircuitBehavior()
        {
            // Arrange & Act: Test that AND short-circuits when left operand is false
            string output = FluxTestHelpers.RunFluxCodeAndCaptureOutput("""
                var sideEffect = false;
                var result = false and (sideEffect = true);
                print result;
                print sideEffect;
            """);

            // Assert
            string[] lines = output.Trim().Split('\n', StringSplitOptions.RemoveEmptyEntries);
            Assert.Equal(2, lines.Length);
            Assert.Equal("False", lines[0].Trim());
            Assert.Equal("False", lines[1].Trim()); // Side effect should not occur
        }

        [Fact]
        public void TestLogicalOr_ShortCircuitBehavior()
        {
            // Arrange & Act: Test that OR short-circuits when left operand is true
            string output = FluxTestHelpers.RunFluxCodeAndCaptureOutput("""
                var sideEffect = false;
                var result = true or (sideEffect = true);
                print result;
                print sideEffect;
            """);

            // Assert
            string[] lines = output.Trim().Split('\n', StringSplitOptions.RemoveEmptyEntries);
            Assert.Equal(2, lines.Length);
            Assert.Equal("True", lines[0].Trim());
            Assert.Equal("False", lines[1].Trim()); // Side effect should not occur
        }

        [Fact]
        public void TestLogicalAnd_NoShortCircuit()
        {
            // Arrange & Act: Test that AND evaluates right operand when left is true
            string output = FluxTestHelpers.RunFluxCodeAndCaptureOutput("""
                var sideEffect = false;
                var result = true and (sideEffect = true);
                print result;
                print sideEffect;
            """);

            // Assert
            string[] lines = output.Trim().Split('\n', StringSplitOptions.RemoveEmptyEntries);
            Assert.Equal(2, lines.Length);
            Assert.Equal("True", lines[0].Trim());
            Assert.Equal("True", lines[1].Trim()); // Side effect should occur
        }

        [Fact]
        public void TestLogicalOr_NoShortCircuit()
        {
            // Arrange & Act: Test that OR evaluates right operand when left is false
            string output = FluxTestHelpers.RunFluxCodeAndCaptureOutput("""
                var sideEffect = false;
                var result = false or (sideEffect = true);
                print result;
                print sideEffect;
            """);

            // Assert
            string[] lines = output.Trim().Split('\n', StringSplitOptions.RemoveEmptyEntries);
            Assert.Equal(2, lines.Length);
            Assert.Equal("True", lines[0].Trim());
            Assert.Equal("True", lines[1].Trim()); // Side effect should occur
        }

        [Fact]
        public void TestLogicalOperators_Precedence()
        {
            // Arrange & Act: Test precedence of logical operators (AND has higher precedence than OR)
            string output = FluxTestHelpers.RunFluxCodeAndCaptureOutput("""
                print true or false and false;
                print false and false or true;
            """);

            // Assert
            string[] lines = output.Trim().Split('\n', StringSplitOptions.RemoveEmptyEntries);
            Assert.Equal(2, lines.Length);
            Assert.Equal("True", lines[0].Trim());  // true or (false and false) = true
            Assert.Equal("True", lines[1].Trim());  // (false and false) or true = true
        }

        [Fact]
        public void TestLogicalOperators_WithComparisons()
        {
            // Arrange & Act: Test logical operators with comparison expressions
            string output = FluxTestHelpers.RunFluxCodeAndCaptureOutput("""
                var x = 10;
                var y = 5;
                var z = 15;
                print x > y and y < z;
                print x < y or z > y;
                print x == 10 and y != 10;
            """);

            // Assert
            string[] lines = output.Trim().Split('\n', StringSplitOptions.RemoveEmptyEntries);
            Assert.Equal(3, lines.Length);
            Assert.Equal("True", lines[0].Trim());   // 10 > 5 and 5 < 15 = true
            Assert.Equal("True", lines[1].Trim());   // 10 < 5 or 15 > 5 = true
            Assert.Equal("True", lines[2].Trim());   // 10 == 10 and 5 != 10 = true
        }

        [Fact]
        public void TestLogicalOperators_ReturnValues()
        {
            // Arrange & Act: Test that logical operators return the last evaluated value
            string output = FluxTestHelpers.RunFluxCodeAndCaptureOutput("""
                print "hello" or "world";
                print nil or "default";
                print "first" and "second";
                print false and "never";
            """);

            // Assert
            string[] lines = output.Trim().Split('\n', StringSplitOptions.RemoveEmptyEntries);
            Assert.Equal(4, lines.Length);
            Assert.Equal("hello", lines[0].Trim());    // "hello" is truthy, so return it
            Assert.Equal("default", lines[1].Trim());  // nil is falsy, so evaluate and return "default"
            Assert.Equal("second", lines[2].Trim());   // "first" is truthy, so evaluate and return "second"
            Assert.Equal("False", lines[3].Trim());    // false is falsy, so return it without evaluating right
        }

        [Fact]
        public void TestLogicalOperators_NestedExpressions()
        {
            // Arrange & Act: Test nested logical expressions
            string output = FluxTestHelpers.RunFluxCodeAndCaptureOutput("""
                var a = true;
                var b = false;
                var c = true;
                var d = false;
                print (a and b) or (c and d);
                print (a or b) and (c or d);
            """);

            // Assert
            string[] lines = output.Trim().Split('\n', StringSplitOptions.RemoveEmptyEntries);
            Assert.Equal(2, lines.Length);
            Assert.Equal("False", lines[0].Trim());  // (true and false) or (true and false) = false
            Assert.Equal("True", lines[1].Trim());   // (true or false) and (true or false) = true
        }

        [Fact]
        public void TestLogicalOperators_WithNilValues()
        {
            // Arrange & Act: Test logical operators with nil values
            string output = FluxTestHelpers.RunFluxCodeAndCaptureOutput("""
                var x = nil;
                var y = "value";
                print x or y;
                print y and x;
                print x and y;
            """);

            // Assert
            string[] lines = output.Trim().Split('\n', StringSplitOptions.RemoveEmptyEntries);
            Assert.Equal(3, lines.Length);
            Assert.Equal("value", lines[0].Trim());  // nil or "value" = "value"
            Assert.Equal("nil", lines[1].Trim());    // "value" and nil = nil
            Assert.Equal("nil", lines[2].Trim());    // nil and "value" = nil
        }
    }
}
