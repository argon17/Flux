using Xunit;

namespace FluxInterpreter.Test
{
    public class FluxWhileLoopTests
    {
        [Fact]
        public void TestWhileLoop_SimpleCounter()
        {
            // Arrange & Act: Test simple while loop with counter
            string output = FluxTestHelpers.RunFluxCodeAndCaptureOutput("""
                var i = 0;
                while (i < 3) {
                    print i;
                    i = i + 1;
                }
            """);

            // Assert
            string[] lines = output.Trim().Split('\n', StringSplitOptions.RemoveEmptyEntries);
            Assert.Equal(3, lines.Length);
            Assert.Equal("0", lines[0].Trim());
            Assert.Equal("1", lines[1].Trim());
            Assert.Equal("2", lines[2].Trim());
        }

        [Fact]
        public void TestWhileLoop_FalseCondition()
        {
            // Arrange & Act: Test while loop with false condition (should not execute)
            string output = FluxTestHelpers.RunFluxCodeAndCaptureOutput("""
                var executed = false;
                while (false) {
                    executed = true;
                    print "should not execute";
                }
                print executed;
            """);

            // Assert
            Assert.Equal("False", output.Trim());
        }

        [Fact]
        public void TestWhileLoop_TrueConditionWithBreakLogic()
        {
            // Arrange & Act: Test while loop that modifies condition variable
            string output = FluxTestHelpers.RunFluxCodeAndCaptureOutput("""
                var condition = true;
                var count = 0;
                while (condition) {
                    print count;
                    count = count + 1;
                    if (count >= 2) {
                        condition = false;
                    }
                }
                print "done";
            """);

            // Assert
            string[] lines = output.Trim().Split('\n', StringSplitOptions.RemoveEmptyEntries);
            Assert.Equal(3, lines.Length);
            Assert.Equal("0", lines[0].Trim());
            Assert.Equal("1", lines[1].Trim());
            Assert.Equal("done", lines[2].Trim());
        }

        [Fact]
        public void TestWhileLoop_NestedLoop()
        {
            // Arrange & Act: Test nested while loops
            string output = FluxTestHelpers.RunFluxCodeAndCaptureOutput("""
                var i = 0;
                while (i < 2) {
                    var j = 0;
                    while (j < 2) {
                        print i * 2 + j;
                        j = j + 1;
                    }
                    i = i + 1;
                }
            """);

            // Assert
            string[] lines = output.Trim().Split('\n', StringSplitOptions.RemoveEmptyEntries);
            Assert.Equal(4, lines.Length);
            Assert.Equal("0", lines[0].Trim()); // i=0, j=0: 0*2+0=0
            Assert.Equal("1", lines[1].Trim()); // i=0, j=1: 0*2+1=1
            Assert.Equal("2", lines[2].Trim()); // i=1, j=0: 1*2+0=2
            Assert.Equal("3", lines[3].Trim()); // i=1, j=1: 1*2+1=3
        }

        [Fact]
        public void TestWhileLoop_WithLogicalCondition()
        {
            // Arrange & Act: Test while loop with logical operators in condition
            string output = FluxTestHelpers.RunFluxCodeAndCaptureOutput("""
                var x = 1;
                var y = 10;
                while (x < 5 and y > 5) {
                    print x;
                    x = x + 1;
                    y = y - 1;
                }
            """);

            // Assert
            string[] lines = output.Trim().Split('\n', StringSplitOptions.RemoveEmptyEntries);
            Assert.Equal(4, lines.Length);
            Assert.Equal("1", lines[0].Trim());
            Assert.Equal("2", lines[1].Trim());
            Assert.Equal("3", lines[2].Trim());
            Assert.Equal("4", lines[3].Trim());
        }

        [Fact]
        public void TestWhileLoop_WithAssignmentInCondition()
        {
            // Arrange & Act: Test while loop with assignment in condition
            string output = FluxTestHelpers.RunFluxCodeAndCaptureOutput("""
                var x = 0;
                var result;
                while (result = x < 3) {
                    print x;
                    x = x + 1;
                }
                print result;
            """);

            // Assert
            string[] lines = output.Trim().Split('\n', StringSplitOptions.RemoveEmptyEntries);
            Assert.Equal(4, lines.Length);
            Assert.Equal("0", lines[0].Trim());
            Assert.Equal("1", lines[1].Trim());
            Assert.Equal("2", lines[2].Trim());
            Assert.Equal("False", lines[3].Trim()); // Final condition result
        }

        [Fact]
        public void TestWhileLoop_EmptyBody()
        {
            // Arrange & Act: Test while loop with empty body
            string output = FluxTestHelpers.RunFluxCodeAndCaptureOutput("""
                var x = 5;
                while (x > 10) {
                }
                print "after loop";
            """);

            // Assert
            Assert.Equal("after loop", output.Trim());
        }

        [Fact]
        public void TestWhileLoop_SimpleStatementBody()
        {
            // Arrange & Act: Test while loop with simple statement (no block)
            string output = FluxTestHelpers.RunFluxCodeAndCaptureOutput("""
                var i = 0;
                while (i < 2) i = i + 1;
                print i;
            """);

            // Assert
            Assert.Equal("2", output.Trim());
        }

        [Fact(Skip = "String concatenation with integer is not supported yet")]
        public void TestWhileLoop_WithVariableScoping()
        {
            // Arrange & Act: Test while loop with variable scoping
            string output = FluxTestHelpers.RunFluxCodeAndCaptureOutput("""
                var outer = "outer";
                var i = 0;
                while (i < 2) {
                    var inner = "inner" + i;
                    print outer;
                    print inner;
                    i = i + 1;
                }
                print outer;
            """);

            // Assert
            string[] lines = output.Trim().Split('\n', StringSplitOptions.RemoveEmptyEntries);
            Assert.Equal(5, lines.Length);
            Assert.Equal("outer", lines[0].Trim());
            Assert.Equal("inner0", lines[1].Trim());
            Assert.Equal("outer", lines[2].Trim());
            Assert.Equal("inner1", lines[3].Trim());
            Assert.Equal("outer", lines[4].Trim());
        }

        [Fact(Skip = "String concatenation with integer is not supported yet")]
        public void TestWhileLoop_StringConcatenation()
        {
            // Arrange & Act: Test while loop building a string
            string output = FluxTestHelpers.RunFluxCodeAndCaptureOutput("""
                var result = "";
                var i = 1;
                while (i <= 3) {
                    result = result + i;
                    i = i + 1;
                }
                print result;
            """);

            // Assert
            Assert.Equal("123", output.Trim());
        }

        [Fact]
        public void TestWhileLoop_ComplexCondition()
        {
            // Arrange & Act: Test while loop with complex condition
            string output = FluxTestHelpers.RunFluxCodeAndCaptureOutput("""
                var a = 1;
                var b = 10;
                var c = 5;
                while (a < c and b > c) {
                    print a + b;
                    a = a + 1;
                    b = b - 1;
                }
            """);

            // Assert
            string[] lines = output.Trim().Split('\n', StringSplitOptions.RemoveEmptyEntries);
            Assert.Equal(4, lines.Length);
            Assert.Equal("11", lines[0].Trim()); // 1 + 10
            Assert.Equal("11", lines[1].Trim()); // 2 + 9
            Assert.Equal("11", lines[2].Trim()); // 3 + 8
            Assert.Equal("11", lines[3].Trim()); // 4 + 7
        }
    }
}
