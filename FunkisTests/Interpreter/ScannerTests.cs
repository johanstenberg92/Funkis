using Microsoft.VisualStudio.TestTools.UnitTesting;
using IronJS.Interpreter;

namespace IronJSTests.Interpreter
{
    [TestClass]
    public class ScannerTests
    {
        [TestMethod]
        [DeploymentItem(@"TestFiles\hello-world.funk")]
        public void ScanHelloWorldTest()
        {
            var text = TestFilesHelper.ReadHelloWorldTestFile();

            var scanner = new Scanner(text);

            char[] characters = {
                'l', 'e', 't', ' ', 'f', 'u', 'n', 'c', ' ', 'h',
                'e', 'l', 'l', 'o', '_', 'w', 'o', 'r', 'l', 'd',
                ' ', '=', ' ', '\n', ' ', ' ', ' ', ' ', 'p', 'r',
                'i', 'n', 't', 'l', 'n', ' ', '(', '"', 'h', 'e',
                'l', 'l', 'o', ' ', 'w', 'o', 'r', 'l', 'd', '!', '"',
                ')', '\n',  '\n', 'l', 'e', 't', ' ', '(', ')', ' ', '=',
                ' ', 'h', 'e', 'l', 'l', 'o', '_', 'w', 'o', 'r',
                'l', 'd', ' ', '(', ')', unchecked ((char) -1)
            };

            int[] columns = {
                0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14,
                15, 16, 17, 18, 19, 20, 21, 22, 23, 0, 1, 2,
                3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16,
                17, 18, 19, 20, 21, 22, 23, 24, 25, 26, 27, 28, 0,
                0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14,
                15, 16, 17, 18, 19, 20, 21, 22, -1
            };

            int[] rows = {
                0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
                0, 0, 0, 0, 0, 0, 0, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1,
                1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1,
                1, 1, 2, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3,
                3, 3, 3, 3, 3, 3, 3, 3, 3, -1
            };

            Assert.AreEqual(characters.Length, columns.Length);
            Assert.AreEqual(rows.Length, columns.Length);

            for (var i = 0; i < characters.Length; ++i)
            {
                var character = characters[i];

                var scannerPeek = scanner.Peek();
                Assert.AreEqual(character, scannerPeek);

                var position = scanner.PeekPosition();
                
                int column = columns[i];
                int scannerColumn = position.Column;
                Assert.AreEqual(column, scannerColumn);

                int row = rows[i];
                int scannerRow = position.Row;
                Assert.AreEqual(row, scannerRow);

                var scannerRead = scanner.Read();
                Assert.AreEqual(character, scannerRead);
            }

            Assert.AreEqual(Scanner._eof, scanner.Peek());
            Assert.AreEqual(Scanner._eof, scanner.Read());
        }
    }
}
