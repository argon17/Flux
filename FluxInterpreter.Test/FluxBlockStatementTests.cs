using Xunit;

namespace FluxInterpreter.Test
{
    public class FluxBlockStatementTests
    {
        [Fact]
        public void TestBlockStatements_NestedVariableScoping()
        {
            // Arrange & Act: Test nested block statements with variable scoping
            string output = FluxTestHelpers.RunFluxCodeAndCaptureOutput("""
                var a = "global a";
                var b = "global b";
                var c = "global c";
                {
                    var a = "outer a";
                    var b = "outer b";
                    {
                        var a = "inner a";
                        print a;
                        print b;
                        print c;
                    }
                    print a;
                    print b;
                    print c;
                }
                print a;
                print b;
                print c;
            """);

            // Assert: Verify the expected output shows proper variable scoping
            string[] lines = output.Trim().Split('\n');
            
            // Inner block should print: "inner a", "outer b", "global c"
            Assert.Equal("inner a", lines[0].Trim());
            Assert.Equal("outer b", lines[1].Trim());
            Assert.Equal("global c", lines[2].Trim());
            
            // Outer block should print: "outer a", "outer b", "global c"
            Assert.Equal("outer a", lines[3].Trim());
            Assert.Equal("outer b", lines[4].Trim());
            Assert.Equal("global c", lines[5].Trim());
            
            // Global scope should print: "global a", "global b", "global c"
            Assert.Equal("global a", lines[6].Trim());
            Assert.Equal("global b", lines[7].Trim());
            Assert.Equal("global c", lines[8].Trim());
        }

        [Fact]
        public void TestBlockStatements_SimpleBlock()
        {
            // Arrange & Act: Test simple block statement
            string output = FluxTestHelpers.RunFluxCodeAndCaptureOutput("""
                var x = "outer";
                {
                    var x = "inner";
                    print x;
                }
                print x;
            """);

            // Assert
            string[] lines = output.Trim().Split('\n');
            Assert.Equal("inner", lines[0].Trim());
            Assert.Equal("outer", lines[1].Trim());
        }

        [Fact]
        public void TestBlockStatements_EmptyBlock()
        {
            // Arrange & Act: Test empty block statement
            string output = FluxTestHelpers.RunFluxCodeAndCaptureOutput("""
                var x = "before";
                {
                }
                print x;
            """);

            // Assert
            Assert.Equal("before", output.Trim());
        }

        [Fact]
        public void TestBlockStatements_VariableAccessFromOuterScope()
        {
            // Arrange & Act: Test accessing variables from outer scope within block
            string output = FluxTestHelpers.RunFluxCodeAndCaptureOutput("""
                var outer = "accessible";
                {
                    print outer;
                    var inner = "local";
                    print inner;
                }
            """);

            // Assert
            string[] lines = output.Trim().Split('\n');
            Assert.Equal("accessible", lines[0].Trim());
            Assert.Equal("local", lines[1].Trim());
        }
    }
}
