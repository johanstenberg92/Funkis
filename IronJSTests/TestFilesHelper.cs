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
    }
}
