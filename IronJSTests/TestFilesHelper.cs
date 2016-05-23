using System.IO;

namespace IronJSTests
{
    static class TestFilesHelper
    {
        public static string ReadHelloWorldTestFile()
        {
            return File.ReadAllText("hello-world.js");
        }

        public static string ReadIfElseIfElseTestFile()
        {
            return File.ReadAllText("if-else-if-else.js");
        }

        public static string ReadWhileTestFile()
        {
            return File.ReadAllText("while.js");
        }

        public static string ReadNestedExpressionsTestFile()
        {
            return File.ReadAllText("nested-expressions.js");
        }

        public static string ReadCommentsTestFile()
        {
            return File.ReadAllText("comments.js");
        }
    }
}
