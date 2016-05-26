using System.IO;

namespace IronJSTests
{
    static class TestFilesHelper
    {
        public static string ReadHelloWorldTestFile()
        {
            return File.ReadAllText("hello-world.funk");
        }
    }
}
