using System.IO;

namespace FunkisTests
{
    static class TestFilesHelper
    {
        public static string ReadHelloWorldTestFile()
        {
            return File.ReadAllText("hello-world.funk");
        }

        public static string ReadCommentsTestFile()
        {
            return File.ReadAllText("comments.funk");
        }

        public static string ReadAddTestFile()
        {
            return File.ReadAllText("add.funk");
        }

        public static string ReadMatchTestFile()
        {
            return File.ReadAllText("match.funk");
        }
    }
}
