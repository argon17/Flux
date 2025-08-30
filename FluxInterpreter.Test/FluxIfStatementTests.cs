using Xunit;

namespace FluxInterpreter.Test
{
    public class FluxIfStatementTests
    {
        [Fact]
        public void TestIfStatement_TrueCondition_SimpleStatement()
        {
            // Arrange & Act: Test if statement with true condition and simple statement
            string output = FluxTestHelpers.RunFluxCodeAndCaptureOutput("""
                if (true) print "executed";
            """);

            // Assert
            Assert.Equal("executed", output.Trim());
        }

        [Fact]
        public void TestIfStatement_FalseCondition_SimpleStatement()
        {
            // Arrange & Act: Test if statement with false condition
            string output = FluxTestHelpers.RunFluxCodeAndCaptureOutput("""
                if (false) print "should not execute";
                print "after if";
            """);

            // Assert
            Assert.Equal("after if", output.Trim());
        }

        [Fact]
        public void TestIfStatement_TrueCondition_BlockStatement()
        {
            // Arrange & Act: Test if statement with true condition and block statement
            string output = FluxTestHelpers.RunFluxCodeAndCaptureOutput("""
                if (true) {
                    print "first line";
                    print "second line";
                    var x = 42;
                    print x;
                }
            """);

            // Assert
            string[] lines = output.Trim().Split('\n', StringSplitOptions.RemoveEmptyEntries);
            Assert.Equal(3, lines.Length);
            Assert.Equal("first line", lines[0].Trim());
            Assert.Equal("second line", lines[1].Trim());
            Assert.Equal("42", lines[2].Trim());
        }

        [Fact]
        public void TestIfStatement_FalseCondition_BlockStatement()
        {
            // Arrange & Act: Test if statement with false condition and block statement
            string output = FluxTestHelpers.RunFluxCodeAndCaptureOutput("""
                if (false) {
                    print "should not execute";
                    var x = 42;
                    print x;
                }
                print "after if block";
            """);

            // Assert
            Assert.Equal("after if block", output.Trim());
        }

        [Fact]
        public void TestIfElseStatement_TrueCondition_BothBlocks()
        {
            // Arrange & Act: Test if-else with true condition, both using blocks
            string output = FluxTestHelpers.RunFluxCodeAndCaptureOutput("""
                if (true) {
                    print "true branch";
                    print "executed";
                } else {
                    print "false branch";
                    print "not executed";
                }
            """);

            // Assert
            string[] lines = output.Trim().Split('\n', StringSplitOptions.RemoveEmptyEntries);
            Assert.Equal(2, lines.Length);
            Assert.Equal("true branch", lines[0].Trim());
            Assert.Equal("executed", lines[1].Trim());
        }

        [Fact]
        public void TestIfElseStatement_FalseCondition_BothBlocks()
        {
            // Arrange & Act: Test if-else with false condition, both using blocks
            string output = FluxTestHelpers.RunFluxCodeAndCaptureOutput("""
                if (false) {
                    print "true branch";
                    print "not executed";
                } else {
                    print "false branch";
                    print "executed";
                }
            """);

            // Assert
            string[] lines = output.Trim().Split('\n', StringSplitOptions.RemoveEmptyEntries);
            Assert.Equal(2, lines.Length);
            Assert.Equal("false branch", lines[0].Trim());
            Assert.Equal("executed", lines[1].Trim());
        }

        [Fact]
        public void TestIfStatement_VariableCondition_Block()
        {
            // Arrange & Act: Test if statement with variable condition and block
            string output = FluxTestHelpers.RunFluxCodeAndCaptureOutput("""
                var condition = true;
                if (condition) {
                    print "variable condition";
                    condition = false;
                    print condition;
                }
            """);

            // Assert
            string[] lines = output.Trim().Split('\n', StringSplitOptions.RemoveEmptyEntries);
            Assert.Equal(2, lines.Length);
            Assert.Equal("variable condition", lines[0].Trim());
            Assert.Equal("False", lines[1].Trim());
        }

        [Fact]
        public void TestIfStatement_ComparisonCondition_Block()
        {
            // Arrange & Act: Test if statement with comparison condition and block
            string output = FluxTestHelpers.RunFluxCodeAndCaptureOutput("""
                var x = 10;
                var y = 5;
                if (x > y) {
                    print "x is greater";
                    print x - y;
                }
            """);

            // Assert
            string[] lines = output.Trim().Split('\n', StringSplitOptions.RemoveEmptyEntries);
            Assert.Equal(2, lines.Length);
            Assert.Equal("x is greater", lines[0].Trim());
            Assert.Equal("5", lines[1].Trim());
        }

        [Fact]
        public void TestNestedIfStatements_WithBlocks()
        {
            // Arrange & Act: Test nested if statements with blocks
            string output = FluxTestHelpers.RunFluxCodeAndCaptureOutput("""
                var x = 10;
                if (x > 5) {
                    print "outer condition true";
                    if (x > 8) {
                        print "inner condition true";
                        var y = x * 2;
                        print y;
                    }
                    print "back to outer";
                }
            """);

            // Assert
            string[] lines = output.Trim().Split('\n', StringSplitOptions.RemoveEmptyEntries);
            Assert.Equal(4, lines.Length);
            Assert.Equal("outer condition true", lines[0].Trim());
            Assert.Equal("inner condition true", lines[1].Trim());
            Assert.Equal("20", lines[2].Trim());
            Assert.Equal("back to outer", lines[3].Trim());
        }

        [Fact]
        public void TestIfStatement_BlockWithVariableScoping()
        {
            // Arrange & Act: Test if statement block with variable scoping
            string output = FluxTestHelpers.RunFluxCodeAndCaptureOutput("""
                var outer = "outer value";
                if (true) {
                    var inner = "inner value";
                    print outer;
                    print inner;
                    outer = "modified";
                }
                print outer;
            """);

            // Assert
            string[] lines = output.Trim().Split('\n', StringSplitOptions.RemoveEmptyEntries);
            Assert.Equal(3, lines.Length);
            Assert.Equal("outer value", lines[0].Trim());
            Assert.Equal("inner value", lines[1].Trim());
            Assert.Equal("modified", lines[2].Trim());
        }

        [Fact]
        public void TestIfElseStatement_MixedBlockAndSimple()
        {
            // Arrange & Act: Test if-else with block for if and simple statement for else
            string output = FluxTestHelpers.RunFluxCodeAndCaptureOutput("""
                var score = 85;
                if (score >= 90) {
                    print "A grade";
                    print "Excellent work";
                } else print "Not an A";
            """);

            // Assert
            Assert.Equal("Not an A", output.Trim());
        }

        [Fact]
        public void TestIfStatement_LogicalCondition_Block()
        {
            // Arrange & Act: Test if statement with logical operators in condition and block
            string output = FluxTestHelpers.RunFluxCodeAndCaptureOutput("""
                var age = 20;
                var hasLicense = true;
                if (age >= 18 and hasLicense) {
                    print "Can drive";
                }
            """);

            // Assert
            Assert.Equal("Can drive", output.Trim());
        }


        [Fact]
        public void TestIfElseStatement_NestedBlocks()
        {
            // Arrange & Act: Test if-else with nested blocks and complex logic
            string output = FluxTestHelpers.RunFluxCodeAndCaptureOutput("""
                var temperature = 25;
                if (temperature > 30) {
                    print "Hot day";
                    if (temperature > 35) {
                        print "Very hot!";
                    }
                } else {
                    print "Not hot";
                    if (temperature < 10) {
                        print "Cold day";
                    } else {
                        print "Moderate temperature";
                    }
                }
            """);

            // Assert
            string[] lines = output.Trim().Split('\n', StringSplitOptions.RemoveEmptyEntries);
            Assert.Equal(2, lines.Length);
            Assert.Equal("Not hot", lines[0].Trim());
            Assert.Equal("Moderate temperature", lines[1].Trim());
        }

        [Fact]
        public void TestIfStatement_EmptyBlock()
        {
            // Arrange & Act: Test if statement with empty block
            string output = FluxTestHelpers.RunFluxCodeAndCaptureOutput("""
                var x = 10;
                if (x > 5) {
                }
                print "after empty if block";
            """);

            // Assert
            Assert.Equal("after empty if block", output.Trim());
        }

        [Fact]
        public void TestIfElseStatement_BothEmptyBlocks()
        {
            // Arrange & Act: Test if-else with both empty blocks
            string output = FluxTestHelpers.RunFluxCodeAndCaptureOutput("""
                if (true) {
                } else {
                }
                print "after empty if-else blocks";
            """);

            // Assert
            Assert.Equal("after empty if-else blocks", output.Trim());
        }
    }
}
